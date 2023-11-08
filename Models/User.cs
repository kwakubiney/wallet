using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Personal.Models{

    [Index(nameof(Username), IsUnique = true)]
    public class User : BaseEntity{
        public int Id {get; set;}
        
        [Required]
        public string Username { get; set;}

        [Required]
        public string Password { get; set; }

        [InverseProperty(nameof(Wallet.OwnerId))]
        public ICollection<Wallet> Wallets { get; }
    } 
}