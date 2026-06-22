using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace Kanban.Adapter.Auth;

public sealed class CookieAuthenticationStateProvider(IJSRuntime jsRuntime, HttpClient httpClient) : AuthenticationStateProvider
{
    private const string TokenCookieName = "cashflow_auth_token";
    private static readonly ClaimsPrincipal Anonymous = new(identity: new ClaimsIdentity());

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        string? token = await GetTokenAsync();

        if (string.IsNullOrWhiteSpace(value: token))
        {
            ClearAuthorizationHeader();
            return new AuthenticationState(user: Anonymous);
        }

        ClaimsPrincipal user = CreateClaimsPrincipal(token: token);

        if (user.Identity?.IsAuthenticated != true)
        {
            await RemoveTokenAsync();
            return new AuthenticationState(user: Anonymous);
        }

        SetAuthorizationHeader(token: token);
        return new AuthenticationState(user: user);
    }

    public async Task SetTokenAsync(string token, int expiresInDays = 7)
    {
        await jsRuntime.InvokeVoidAsync(
            identifier: "cashFlowAuthCookies.set",
            TokenCookieName,
            token,
            expiresInDays);

        SetAuthorizationHeader(token: token);
        NotifyAuthenticationStateChanged(task: Task.FromResult(result: new AuthenticationState(user: CreateClaimsPrincipal(token: token))));
    }

    public async Task RemoveTokenAsync()
    {
        await jsRuntime.InvokeVoidAsync(identifier: "cashFlowAuthCookies.remove", TokenCookieName);
        ClearAuthorizationHeader();
        NotifyAuthenticationStateChanged(task: Task.FromResult(result: new AuthenticationState(user: Anonymous)));
    }

    public async Task<string?> GetTokenAsync()
    {
        return await jsRuntime.InvokeAsync<string?>(
            identifier: "cashFlowAuthCookies.get",
            TokenCookieName);
    }

    private void SetAuthorizationHeader(string token)
    {
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            scheme: "Bearer",
            parameter: token);
    }

    private void ClearAuthorizationHeader()
    {
        httpClient.DefaultRequestHeaders.Authorization = null;
    }

    private static ClaimsPrincipal CreateClaimsPrincipal(string token)
    {
        try
        {
            IReadOnlyList<Claim> claims = ParseClaimsFromJwt(token: token);

            Claim? expirationClaim = claims.FirstOrDefault(predicate: claim => claim.Type == "exp");
            if (expirationClaim is not null
                && long.TryParse(s: expirationClaim.Value, result: out long expirationUnixTime)
                && DateTimeOffset.FromUnixTimeSeconds(seconds: expirationUnixTime) <= DateTimeOffset.UtcNow)
            {
                return Anonymous;
            }

            return new ClaimsPrincipal(identity: new ClaimsIdentity(
                claims: claims,
                authenticationType: "jwt"));
        }
        catch
        {
            return Anonymous;
        }
    }

    private static IReadOnlyList<Claim> ParseClaimsFromJwt(string token)
    {
        string[] tokenParts = token.Split(separator: '.');
        if (tokenParts.Length < 2)
        {
            return [];
        }

        byte[] payloadBytes = ParseBase64WithoutPadding(base64: tokenParts[1]);
        Dictionary<string, JsonElement>? payload = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(
            utf8Json: payloadBytes);

        if (payload is null)
        {
            return [];
        }

        List<Claim> claims = [];

        foreach ((string claimType, JsonElement claimValue) in payload)
        {
            if (claimValue.ValueKind == JsonValueKind.Array)
            {
                foreach (JsonElement value in claimValue.EnumerateArray())
                {
                    claims.Add(item: new Claim(type: claimType, value: value.ToString()));
                }

                continue;
            }

            claims.Add(item: new Claim(type: claimType, value: claimValue.ToString()));
        }

        return claims;
    }

    private static byte[] ParseBase64WithoutPadding(string base64)
    {
        string paddedBase64 = base64.Replace(oldChar: '-', newChar: '+').Replace(oldChar: '_', newChar: '/');

        paddedBase64 += (paddedBase64.Length % 4) switch
        {
            2 => "==",
            3 => "=",
            _ => string.Empty
        };

        return Convert.FromBase64String(s: paddedBase64);
    }
}
