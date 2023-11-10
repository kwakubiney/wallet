using System.ComponentModel.DataAnnotations;

namespace Personal.Models
{
    public class LoginUserPayload
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }

    }
}