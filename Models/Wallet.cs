using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Personal.Models{
    public class Wallet: BaseEntity{
        public string Name { get; set;}

        //TODO: Truncate AccountNumber to first 10 on saving
        public string AccountNumber { get; set; }

        [ForeignKey(nameof(User))]
        [InverseProperty(nameof(User.Wallets))]
        public User User {get; set;}
        public Type Type {get; set;}

        public string Owner {get; set;}
    }

        public enum Type
    {
        [Description("MOMO")]
        MOMO,
         [Description("CARD")]
        CARD
    }
}