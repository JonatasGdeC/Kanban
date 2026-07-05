using Bogus;
using CommomTestsUtilies.Cryptography;
using Kanban.Domain.Entities;
using Kanban.Domain.Security.Cryptography;

namespace CommomTestsUtilies.Entities;

public class UserBuilder
{
    public static User Build()
    {
        IEncrypter passwordEncripter = new PasswordEncrypterBuilder().Build();

        Faker<User>? user = new Faker<User>()
            .RuleFor(property: u => u.Id, value: Guid.NewGuid())
            .RuleFor(property: u => u.Name, setter: faker => faker.Person.FirstName)
            .RuleFor(property: u => u.Email, setter: (faker, user) => faker.Internet.Email(firstName: user.Name))
            .RuleFor(property: u => u.Password, setter: (_, user) => "Tests@123");

        return user;
    }
}