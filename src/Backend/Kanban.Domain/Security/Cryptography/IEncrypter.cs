namespace Kanban.Domain.Security.Cryptography;

public interface IEncrypter
{
    string Encrypt(string value);
    bool Verify(string value, string hash);
}