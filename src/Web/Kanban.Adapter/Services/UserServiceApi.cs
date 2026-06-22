using Kanban.Communication.Dtos;
using Kanban.Communication.Requests.User;
using Kanban.Communication.Responses.User;

namespace Kanban.Adapter.Services;

public class UserServiceApi(HttpClient httpClient) : ApiServiceBase(httpClient: httpClient)
{
    private const string BaseUri = "User";
    
    public async Task<RegisteredUserResponse> Register(RegisterUserRequest request)
    {
        return await PostAsync<RegisterUserRequest, RegisteredUserResponse>(uri: BaseUri, request: request);
    }

    public async Task<LoginResponse> Login(LoginRequest request)
    {
        return await PostAsync<LoginRequest, LoginResponse>(uri: $"{BaseUri}/login", request: request);
    }
    
    public async Task<UserDto?> Get()
    {
        return await GetAsync<UserDto>(uri: BaseUri);
    }
    
    public async Task Update(UpdateUserRequest request)
    {
        await PutAsync(uri: BaseUri, request: request);
    }
    
    public async Task UpdatePassword(UpdatePasswordRequest request)
    {
        await PutAsync(uri: $"{BaseUri}/password", request: request);
    }

    public async Task ForgotPassword(ForgotPasswordRequest request)
    {
        await PostAsync<ForgotPasswordRequest, object>(uri: $"{BaseUri}/forgot-password", request: request);
    }

    public async Task<ValidateResetCodeResponse> ValidateResetCode(ValidateResetCodeRequest request)
    {
        return await PostAsync<ValidateResetCodeRequest, ValidateResetCodeResponse>(uri: $"{BaseUri}/validate-reset-code", request: request);   
    }

    public async Task ResetPassword(ResetPasswordRequest request)
    {
        await PutAsync(uri: $"{BaseUri}/reset-password", request: request);
    }

    public async Task Delete() => await DeleteAsync(uri: BaseUri);
}