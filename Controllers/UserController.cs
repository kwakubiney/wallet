using System.ComponentModel;
using Auth.Helpers.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Personal.Models;

namespace bank_service.Controllers;

[Route("api/v1/user")]
public class UserController: BaseController{

     private readonly IPasswordHasher _hasher;

    public UserController(IPasswordHasher hasher)
        {
            _hasher = hasher;
        }

    [HttpPost]
    public IActionResult CreateUser([FromBody] CreateUserPayload payload){
         if (!ModelState.IsValid)
            {
                return BadRequest();
            }
        var userToBeSaved = new User{
            Username = payload.Username,
            Password = _hasher.Hash(payload.Password)
            };
        DbContext.Users.Add(userToBeSaved);
        DbContext.SaveChanges();
        var response = new CreateUserResponseDto{
            Username = payload.Username,
            Id = userToBeSaved.Id
            };
        string uri = $"https://localhost/api/v1/user/{userToBeSaved.Id}";
        return Created(uri, response);
    }
}