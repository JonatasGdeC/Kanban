using Kanban.Domain.Security.Cryptography;
using BC = BCrypt.Net.BCrypt;

namespace Kanban.Infrastructure.Security.Cryptography;

public class BCrypt : IEncrypter
{
    public string Encrypt(string value)
    {
        return BC.HashPassword(inputKey: value);
    }

    public bool Verify(string value, string hash) => BC.Verify(text: value, hash: hash);
}