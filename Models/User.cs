using System.ComponentModel.DataAnnotations.Schema;

namespace Personal.Models{
    public class User : BaseEntity{
        public string Username { get; set;}
        public string Password { get; set; }

        [InverseProperty(nameof(Wallet.User))]
        public ICollection<Wallet> Wallets { get; }
    } 
}