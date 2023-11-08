using Microsoft.AspNetCore.Mvc;
using Personal.DataContext;

public class BaseController : Controller
{
    protected WalletContext DbContext => (WalletContext)HttpContext.RequestServices.GetService(typeof(WalletContext));
}