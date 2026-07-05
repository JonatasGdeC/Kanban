using CommomTestsUtilies.Entities;
using Kanban.Domain.Security.Cryptography;
using Kanban.Domain.Security.Tokens;
using Kanban.Infrastructure.DataAccess;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.Tests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    public Kanban.Domain.Entities.User User { get; private set; } = null!;
    public string Token { get; private set; } = null!;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment(environment: "Test").ConfigureServices(configureServices: services =>
        {
            ServiceProvider provider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

            services.AddDbContext<KanbanDbContext>(optionsAction: options =>
            {
                options.UseInMemoryDatabase(databaseName: "InMemoryDbForTesting");
                options.UseInternalServiceProvider(serviceProvider: provider);
            });

            using IServiceScope scope = services.BuildServiceProvider().CreateScope();

            KanbanDbContext dbContext = scope.ServiceProvider.GetRequiredService<KanbanDbContext>();
            IEncrypter encrypter = scope.ServiceProvider.GetRequiredService<IEncrypter>();
            IAccessTokenGenerator tokenGenerator = scope.ServiceProvider.GetRequiredService<IAccessTokenGenerator>();

            StartDataBase(dbContext: dbContext, encrypter: encrypter);
            Token = tokenGenerator.Generate(user: User);
        });
    }

    private void StartDataBase(KanbanDbContext dbContext, IEncrypter encrypter)
    {
        AddUser(dbContext: dbContext, encrypter: encrypter);
        dbContext.SaveChanges();
    }

    private void AddUser(KanbanDbContext dbContext, IEncrypter encrypter)
    {
        User = UserBuilder.Build();

        Kanban.Domain.Entities.User persistedUser = new()
        {
            Id = User.Id,
            Name = User.Name,
            Email = User.Email,
            Password = encrypter.Encrypt(value: User.Password)
        };

        dbContext.Users.Add(entity: persistedUser);
    }
}
