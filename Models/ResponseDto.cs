namespace Personal.Models
{
    public class ResponseDTO<T>
    {
        public T data { get; set; }
        public string message { get; set; }
    }
}