using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Auth.Helpers.Interfaces;
using Microsoft.IdentityModel.Tokens;

public class JWTGenerator : IJwtGenerator{
    private IConfiguration _configuration;
    private readonly JwtHeader jwtHeader;
    private readonly IList<Claim> jwtClaims;
    private readonly DateTime jwtTime;
    private readonly int tokenLifeTimeInSeconds;

    public JWTGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            jwtHeader = new JwtHeader(credentials);
            jwtClaims = new List<Claim>();
            jwtTime = DateTime.Now;
            tokenLifeTimeInSeconds = int.Parse(_configuration["Jwt:LifeTimeInSeconds"]);
        }

    public JWTGenerator AddClaim(Claim claim){
        jwtClaims.Add(claim);
        return this;
    }

    public string GetToken(){
        var jwt = new JwtSecurityToken(
            jwtHeader,
            new JwtPayload(
                issuer: _configuration["Jwt:Issuer"],
                audience : _configuration["Jwt:Issuer"],
                claims: jwtClaims,
                notBefore: jwtTime,
                expires: jwtTime.AddSeconds(tokenLifeTimeInSeconds)
        ));
        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }
}