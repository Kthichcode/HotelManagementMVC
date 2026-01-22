using BusinessObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace HotelManagementMVC.Controllers
{
    [Authorize]
    public class WalletsController : Controller
    {
        private readonly IWalletService _walletService;
        private readonly UserManager<ApplicationUser> _userManager;

        public WalletsController(IWalletService walletService, UserManager<ApplicationUser> userManager)
        {
            _walletService = walletService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var wallet = _walletService.GetUserWallet(user.Id);
            return View(wallet);
        }
    }
}
