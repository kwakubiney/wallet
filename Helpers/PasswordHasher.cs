using Auth.Helpers.Interfaces;

namespace Auth.Helpers.Implementations
{
    public class BcryptPasswordHasher : IPasswordHasher
    {
        public string Hash(string value)
        {
            return BCrypt.Net.BCrypt.HashPassword(value, GetRandomSalt());
        }

        public bool ValidateHash(string raw, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(raw, hash);
        }

        private static string GetRandomSalt()
        {
            return BCrypt.Net.BCrypt.GenerateSalt(12);
        }
    }
}