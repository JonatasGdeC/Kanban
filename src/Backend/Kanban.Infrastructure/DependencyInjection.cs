using Kanban.Domain.Repositories;
using Kanban.Domain.Repositories.Boad;
using Kanban.Domain.Repositories.Column;
using Kanban.Domain.Repositories.PasswordResetCode;
using Kanban.Domain.Repositories.SubTask;
using Kanban.Domain.Repositories.Task;
using Kanban.Domain.Repositories.User;
using Kanban.Domain.Security.CodeGenerator;
using Kanban.Domain.Security.Cryptography;
using Kanban.Domain.Security.Tokens;
using Kanban.Domain.Services.LoggedUser;
using Kanban.Domain.Services.MailKit;
using Kanban.Infrastructure.DataAccess;
using Kanban.Infrastructure.DataAccess.Repositories;
using Kanban.Infrastructure.Security.CodeGenerator;
using Kanban.Infrastructure.Security.Tokens;
using Kanban.Infrastructure.Services.LoggedUser;
using Kanban.Infrastructure.Services.MailKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Kanban.Infrastructure;
using Security.Cryptography;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, IConfigurationManager configurationManager)
    {
        AddRepositories(services: services);
        AddDbContext(services: services, connectionString: configurationManager.GetConnectionString(name: "connection")!);
        AddUserToken(services: services, configurationManager: configurationManager);
        AddEmailSettings(services: services, configurationManager: configurationManager);
        AddPasswordResetToken(services: services, configurationManager: configurationManager);
        
        services.AddScoped<IEncrypter, BCrypt>();
        services.AddScoped<ILoggedUser, LoggedUser>();
    }

    private static void AddDbContext(IServiceCollection services, string connectionString)
    {
        services.AddDbContext<KanbanDbContext>(optionsAction: options => options.UseNpgsql(connectionString: connectionString));
    }

    private static void AddEmailSettings(IServiceCollection services, IConfigurationManager configurationManager)
    {
        IConfigurationSection emailSettings = configurationManager.GetSection(key: "EmailSettings");
        services.Configure<EmailSettings>(config: emailSettings);
        
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<ICodeGenerator, CodeGenerator>();
    }
    
    private static void AddUserToken(IServiceCollection services, IConfigurationManager configurationManager)
    {
        IConfigurationSection expirationTimeMinutes = configurationManager.GetSection(key: "Settings:Jwt:ExpiresMinutes");
        IConfigurationSection signingKey = configurationManager.GetSection(key: "Settings:Jwt:SigningKey");

        services.AddScoped<IAccessTokenGenerator>(implementationFactory: _ => 
            new JwtTokenGenerator(expirationTimeMinutes: uint.Parse(s: expirationTimeMinutes.Value!) , signingKey: signingKey.Value!));
    }

    private static void AddPasswordResetToken(IServiceCollection services, IConfigurationManager configurationManager)
    {
        IConfigurationSection expirationTimeMinutes = configurationManager.GetSection(key: "Settings:PasswordResetToken:ExpiresMinutes");
        IConfigurationSection signingKey = configurationManager.GetSection(key: "Settings:Jwt:SigningKey");
        
        services.AddScoped<IPasswordResetTokenGenerator>(implementationFactory: _ =>
            new PasswordResetTokenGenerator(expirationTimeMinutes: uint.Parse(s: expirationTimeMinutes.Value!), signingKey: signingKey.Value!));
        
        services.AddScoped<IVerifyTokenResetCode>(implementationFactory: provider => new VerifyTokenResetCode(signingKey: signingKey.Value!));
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserReadRepository, UserRepository>();
        services.AddScoped<IUserWriteRepository, UserRepository>();
        services.AddScoped<IBoardReadRepository, BoardRepository>();
        services.AddScoped<IBoardWriteRepository, BoardRepository>();
        services.AddScoped<IColumnReadRepository, ColumnRepository>();
        services.AddScoped<IColumWriteRepository, ColumnRepository>();
        services.AddScoped<ITaskReadRepository, TaskRepository>();
        services.AddScoped<ITaskWriteRepository, TaskRepository>();
        services.AddScoped<ISubTaskReadRepository, SubTaskRepository>();
        services.AddScoped<ISubTaskWriteRepository, SubTaskRepository>();
        services.AddScoped<IPasswordResetCodeRepository, PasswordResetCodeRepository>();
    }
}