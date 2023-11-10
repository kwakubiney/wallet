using System.ComponentModel;
using System.Security.Claims;
using Auth.Helpers.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Personal.Models;

namespace wallet.Controllers;

[Route("api/v1/user")]
public class UserController : BaseController
{

    private readonly IPasswordHasher _hasher;

    private readonly IJwtGenerator _jWTGenerator;

    public UserController(IPasswordHasher hasher, IJwtGenerator jWTGenerator)
    {
        _hasher = hasher;
        _jWTGenerator = jWTGenerator;
    }

    [HttpPost]
    public IActionResult CreateUser([FromBody] CreateUserPayload payload)
    {
        if (!ModelState.IsValid)
        {
            return ModelState.As<BadRequestObjectResult>(); ;
        }
        var userToBeSaved = new User
        {
            Username = payload.Username,
            Password = _hasher.Hash(payload.Password),
            PhoneNumber = payload.PhoneNumber
        };
        DbContext.Users.Add(userToBeSaved);
        DbContext.SaveChanges();
        var response = new CreateUserResponseDto
        {
            Username = payload.Username,
            Id = userToBeSaved.Id,
            PhoneNumber = payload.PhoneNumber
        };

        string uri = $"https://localhost/api/v1/user/{userToBeSaved.Id}";
        return Created(uri,
        new ResponseDTO<CreateUserResponseDto>()
        {
            data = response,
            message = "User has successfully been created"
        });
    }

    [HttpGet("login")]
    public IActionResult LoginUser([FromBody] LoginUserPayload payload)
    {
        if (!ModelState.IsValid)
        {
            return ModelState.As<BadRequestObjectResult>(); ;
        }
        var user = DbContext.Users.FirstOrDefault(u => u.Username == payload.Username);
        if (user == null)
        {
            return NotFound(string.Format("No user with username {0} exists", payload.Username));
        }

        if (!_hasher.ValidateHash(payload.Password, user.Password))
        {
            return BadRequest("Password entered is incorrect");
        }
        JWTGenerator token = _jWTGenerator.AddClaim(
            new Claim("username", user.Username)
        ).AddClaim(
            new Claim("id", user.Id.ToString())
        );
        return Ok(new ResponseDTO<LoginUserResponseDto>()
        {
            message = "User has successfully logged in",
            data = new LoginUserResponseDto()
            {
                Token = token.GetToken()
            }
        });
    }
}