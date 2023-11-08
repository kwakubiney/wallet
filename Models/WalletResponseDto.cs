namespace Personal.Models
{
    public class WalletResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string AccountNumber {get; set;}

        public Scheme Scheme {get; set;}
        public Type Type {get; set;}
        public string Owner {get; set;}
    }
}