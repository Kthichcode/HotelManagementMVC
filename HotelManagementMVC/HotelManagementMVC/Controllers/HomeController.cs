using Microsoft.AspNetCore.Mvc;

namespace HotelManagementMVC.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            return View();
        }
    }
}
