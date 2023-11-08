using System.Reflection.Emit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Personal.Models;

namespace wallet.Controllers;

[Route("api/v1/wallet")]
public class WalletController : BaseController{

[Authorize]
[HttpPost]
public IActionResult CreateWalletPayload([FromBody] CreateWalletPayload payload){
     if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
    var loggedInUserId = User.FindFirst("id")?.Value;
    if (payload.User.ToString() != loggedInUserId){
        return Unauthorized("User is not authorized to create this wallet");
    }
    List<string> listOfVendors = new List<string>(){"vodafone", "mtn", "airteltigo"};
    List<string> listOfSchemes = new List<string>(){"visa", "mastercard"};
    if (payload.Type == Personal.Models.Type.MOMO && !listOfVendors.Contains(payload.Scheme.ToString().ToLower())
     || (payload.Type == Personal.Models.Type.CARD && !listOfSchemes.Contains(payload.Scheme.ToString().ToLower()))
     )
    {
        return BadRequest(new ResponseDTO<string>(){
            data = null,
            message = "Improper wallet scheme and type selection"
        });
    }

    string accountNumber = payload.Type == Personal.Models.Type.CARD 
                               ? payload.AccountNumber.Length > 10 ? payload.AccountNumber.Substring(0, 10) : payload.AccountNumber
                               : payload.AccountNumber;

    var wallet = DbContext.Wallets.FirstOrDefault(w => w.AccountNumber == accountNumber);
        if(wallet != null) {
            return BadRequest(
                new ResponseDTO<string>(){
                data = null,
                message = "Wallet with account number specified already exists"
        });
        }

    var user = DbContext.Users.Include(u => u.Wallets).FirstOrDefault(u => u.Id.ToString() == payload.User);
        if(user == null) {
            return NotFound(
                new ResponseDTO<string>(){
                data = null,
                message = string.Format("No user with id {0} exists", payload.Id)}
                );
        }
    
    if (user.Wallets.Count() == 5){
        return BadRequest(
            new ResponseDTO<string>(){
            data = null,
            message = "User cannot create more than 5 wallets"
        });
    }
    
    var walletToBeSaved = new Wallet{
            Name = payload.Name,
            Scheme = payload.Scheme,
            Type = payload.Type,
            AccountNumber = accountNumber,
            OwnerId = user,
            Owner = payload.Owner
            };
    user.Password = "";
    DbContext.Wallets.Add(walletToBeSaved);
        DbContext.SaveChanges();
        var response = new WalletResponseDto{
            Id = walletToBeSaved.Id,
            Name = payload.Name,
            Scheme = payload.Scheme,
            Type = payload.Type,
            AccountNumber = accountNumber,
            Owner = payload.Owner
            };
  string uri = $"https://localhost/api/v1/wallet/{walletToBeSaved.Id}";
        return Created(uri,
        new ResponseDTO<WalletResponseDto>(){
            data = response,
            message = "Wallet has successfully been created"
        });
}
}