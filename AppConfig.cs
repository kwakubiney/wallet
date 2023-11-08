public class AppConfig{
    static public string Issuer {get;set;}
    static public string Key {get;set;}
    public AppConfig(IConfiguration config)
    {
        Issuer = config.GetValue<string>("Jwt:Issuer");
        Key = config.GetValue<string>("Jwt:Key");
    }
}