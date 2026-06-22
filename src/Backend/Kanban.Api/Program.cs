using System.Text;
using Kanban.Api.Filter;
using Kanban.Api.Middleware;
using Kanban.Api.Token;
using Kanban.Application;
using Kanban.Domain.Security.Tokens;
using Kanban.Infrastructure;
using Kanban.Infrastructure.DataAccess.Migrations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args: args);

const string corsPolicyName = "Frontend";

builder.Services.AddCors(setupAction: options =>
{
    options.AddPolicy(name: corsPolicyName, configurePolicy: policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:5072",
                "https://kanban-jgc.vercel.app"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});


builder.Services.AddOpenApi();
builder.Services.AddInfrastructure(configurationManager: builder.Configuration);
builder.Services.AddApplication();

builder.Services.AddControllers(configure: options =>
{
    options.Filters.Add(filterType: typeof(ExceptionFilter));
});

builder.Services.AddSwaggerGen(setupAction: config =>
{
    config.AddSecurityDefinition(name: "Bearer", securityScheme: new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = """
                      JWT Authorization header using the Bearer scheme.
                      Enter 'Bearer' [space] and then your token.
                      Example: 'Bearer 12345abcdef'
                      """,
        In = ParameterLocation.Header,
        Scheme = "Bearer",
        Type = SecuritySchemeType.ApiKey
    });

    config.AddSecurityRequirement(securityRequirement: document => new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecuritySchemeReference(referenceId: "Bearer", hostDocument: document),
            []
        }
    });
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication(configureOptions: options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(configureOptions: options =>
    {
        string signingKey = builder.Configuration.GetValue<string>(key: "Settings:Jwt:SigningKey")!;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero,
            IssuerSigningKey = new SymmetricSecurityKey(key: Encoding.UTF8.GetBytes(s: signingKey))
        };
    });

builder.Services.AddScoped<ITokenProvider, HttpContextTokenValue>();
builder.Services.AddHealthChecks();

WebApplication app = builder.Build();

app.UseMiddleware<CultureMiddleware>();

app.UseCors(policyName: corsPolicyName);

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapHealthChecks(pattern: "/Health", options: new HealthCheckOptions
{
    AllowCachingResponses = false,
    ResultStatusCodes =
    {
        [key: HealthStatus.Healthy] = StatusCodes.Status200OK,
        [key: HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable,
    }
});

app.MapControllers();

await MigrateDatabase(); 

app.Run();

async Task MigrateDatabase()  
{  
    await using AsyncServiceScope scope = app.Services.CreateAsyncScope();  
    await DataBaseMigration.MigrateDatabase(serviceProvider: scope.ServiceProvider);  
}
