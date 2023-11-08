namespace Auth.Helpers.Interfaces
{
    public interface IPasswordHasher
    {
        string Hash(string value);
        bool ValidateHash(string raw, string hash);
    }
}