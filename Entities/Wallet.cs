using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Personal.Entities;

namespace Personal.Models
{

    [Index(nameof(AccountNumber), IsUnique = true)]
    public class Wallet : BaseEntity
    {

        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        //TODO: Truncate AccountNumber to first 10 on saving
        [Required]
        public string AccountNumber { get; set; }

        [ForeignKey(nameof(User))]
        [InverseProperty(nameof(User.Wallets))]
        [Required]
        public User OwnerId { get; set; }

        [Required]
        public Type Type { get; set; }

        [Required]
        public Scheme Scheme { get; set; }

        [Required]
        public string Owner { get; set; }
    }

    public enum Type
    {
        [Description("MOMO")]
        MOMO,
        [Description("CARD")]
        CARD
    }

    public enum Scheme
    {
        [Description("VISA")]
        VISA,
        [Description("MASTERCARD")]
        MASTERCARD,
        [Description("VODAFONE")]
        VODAFONE,
        [Description("MTN")]
        MTN,
        [Description("AIRTELTIGO")]
        AIRTELTIGO
    }
}