using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Personal.Models;

namespace wallet.Controllers;

[Route("api/v1/")]
public class WalletController : BaseController
{

    [Authorize]
    [HttpPost("wallet")]
    public IActionResult CreateWallet([FromBody] CreateWalletPayload payload)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var loggedInUserId = User.FindFirst("id")?.Value;
        if (payload.User.ToString() != loggedInUserId)
        {
            return Unauthorized("User is not authorized to create this wallet");
        }
        List<string> listOfVendors = new List<string>() { "vodafone", "mtn", "airteltigo" };
        List<string> listOfSchemes = new List<string>() { "visa", "mastercard" };
        if (payload.Type == Personal.Models.Type.MOMO && !listOfVendors.Contains(payload.Scheme.ToString().ToLower())
         || (payload.Type == Personal.Models.Type.CARD && !listOfSchemes.Contains(payload.Scheme.ToString().ToLower()))
         )
        {
            return BadRequest(new ResponseDTO<string>()
            {
                data = null,
                message = "Improper wallet scheme and type selection"
            });
        }

        string accountNumber = payload.Type == Personal.Models.Type.CARD
                                   ? payload.AccountNumber.Length > 10 ? payload.AccountNumber.Substring(0, 10) : payload.AccountNumber
                                   : payload.AccountNumber;

        var wallet = DbContext.Wallets.FirstOrDefault(w => w.AccountNumber == accountNumber);
        if (wallet != null)
        {
            return BadRequest(
                new ResponseDTO<string>()
                {
                    data = null,
                    message = "Wallet with account number specified already exists"
                });
        }

        var user = DbContext.Users.Include(u => u.Wallets).FirstOrDefault(u => u.Id.ToString() == payload.User);
        if (user == null)
        {
            return NotFound(
                new ResponseDTO<string>()
                {
                    data = null,
                    message = string.Format("No user with id {0} exists", payload.Id)
                }
                );
        }

        if (user.Wallets.Count() == 5)
        {
            return BadRequest(
                new ResponseDTO<string>()
                {
                    data = null,
                    message = "User cannot create more than 5 wallets"
                });
        }

        var walletToBeSaved = new Wallet
        {
            Name = payload.Name,
            Scheme = payload.Scheme,
            Type = payload.Type,
            AccountNumber = accountNumber,
            OwnerId = user,
            Owner = payload.Owner
        };

        DbContext.Wallets.Add(walletToBeSaved);
        DbContext.SaveChanges();
        var response = new WalletResponseDto
        {
            Id = walletToBeSaved.Id,
            Name = payload.Name,
            Scheme = payload.Scheme,
            Type = payload.Type,
            AccountNumber = accountNumber,
            Owner = payload.Owner,
            CreatedAt = walletToBeSaved.CreatedAt,
            UpdatedAt = walletToBeSaved.UpdatedAt
        };
        string uri = $"https://localhost/api/v1/wallet/{walletToBeSaved.Id}";
        return Created(uri,
        new ResponseDTO<WalletResponseDto>()
        {
            data = response,
            message = "Wallet has successfully been created"
        });
    }

    [Authorize]
    [HttpGet("wallet/{id}")]
    public IActionResult GetWallet(int id)
    {
        var wallet = DbContext.Wallets.Include(w => w.OwnerId).FirstOrDefault(w => w.Id == id);
        if (wallet == null)
        {
            return NotFound(
                new ResponseDTO<WalletResponseDto>()
                {
                    data = null,
                    message = string.Format("Wallet with id {0} not found", id)
                }
            );
        }
        var loggedInUserId = User.FindFirst("id")?.Value;
        if (wallet.OwnerId.Id.ToString() != loggedInUserId)
        {
            return Unauthorized("User is not authorized to view this wallet");
        }
        return Ok(
            new ResponseDTO<WalletResponseDto>()
            {
                data = new WalletResponseDto()
                {
                    Name = wallet.Name,
                    Id = wallet.Id,
                    Scheme = wallet.Scheme,
                    Owner = wallet.Owner,
                    AccountNumber = wallet.AccountNumber,
                    Type = wallet.Type,
                    CreatedAt = wallet.CreatedAt,
                    UpdatedAt = wallet.UpdatedAt
                },
                message = "Wallet successfully retrieved"
            }
        );
    }

    [Authorize]
    [HttpGet("user/{id}/wallets/")]
    public IActionResult GetWallets(int id)
    {
        var loggedInUserId = User.FindFirst("id")?.Value;
        if (id.ToString() != loggedInUserId)
        {
            return Unauthorized("User is not authorized to view this wallet");
        }
        var user = DbContext.Users.Include(u => u.Wallets).FirstOrDefault(u => u.Id.ToString() == id.ToString());
        if (user == null)
        {
            return NotFound(
                    new ResponseDTO<string>()
                    {
                        data = null,
                        message = string.Format("No user with id {0} exists", id)
                    }
                    );
        }

        var walletsForUser = user.Wallets;
        WalletResponseDto[] walletResponseDto = new WalletResponseDto[walletsForUser.Count()];

        int index = 0;
        foreach (Wallet wallet in walletsForUser)
        {
            walletResponseDto[index] = new WalletResponseDto
            {
                Name = wallet.Name,
                Id = wallet.Id,
                Scheme = wallet.Scheme,
                Owner = wallet.Owner,
                AccountNumber = wallet.AccountNumber,
                Type = wallet.Type,
                CreatedAt = wallet.CreatedAt,
                UpdatedAt = wallet.UpdatedAt
            };
            index++;
        }
        return Ok(
            new ResponseDTO<WalletResponseDto[]>()
            {
                data = walletResponseDto,
                message = "Wallets successfully retrieved"
            }
        );
    }

    [Authorize]
    [HttpDelete("wallet/{id}")]
    public IActionResult DeleteWallet(int id)
    {
        var wallet = DbContext.Wallets.Include(w => w.OwnerId).FirstOrDefault(w => w.Id == id);
        if (wallet == null)
        {
            return NotFound(
                new ResponseDTO<WalletResponseDto>()
                {
                    data = null,
                    message = string.Format("Wallet with id {0} not found", id)
                }
            );
        }
        var loggedInUserId = User.FindFirst("id")?.Value;
        if (wallet.OwnerId.Id.ToString() != loggedInUserId)
        {
            return Unauthorized("User is not authorized to delete this wallet");
        }
        DbContext.Wallets.Where(w => w.Id == id).ExecuteDeleteAsync();
        return Ok(
            new ResponseDTO<string>()
            {
                data = null,
                message = "Wallet successfully deleted"
            }
        );
    }
}

