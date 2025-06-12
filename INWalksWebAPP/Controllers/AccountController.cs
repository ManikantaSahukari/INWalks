using Microsoft.AspNetCore.Mvc;

namespace INWalksWebAPP.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
    }
}
