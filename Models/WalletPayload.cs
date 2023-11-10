using System.ComponentModel.DataAnnotations;

namespace Personal.Models
{
    public class CreateWalletPayload
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Wallet name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Account number is required.")]
        [MinLength(10)]
        public string AccountNumber { get; set; }

        [Required(ErrorMessage = "User ID is required")]
        public string User { get; set; }

        [Required(ErrorMessage = "Account type is required")]
        public Type Type { get; set; }

        [Required(ErrorMessage = "Account scheme is required")]
        public Scheme Scheme { get; set; }
    }
}