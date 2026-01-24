using BusinessObjects;
using HotelManagementMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace HotelManagementMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly UserManager<ApplicationUser> _userManager;


        public AccountController(IAccountService accountService, UserManager<ApplicationUser> userManager)
        {
            _accountService = accountService;
            _userManager = userManager;
        }


        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            ApplicationUser? user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login");
            }

            ProfileViewModel model = new ProfileViewModel();
            model.UserName = user.UserName ?? "";
            model.Email = user.Email ?? "";
            model.FullName = user.FullName ?? "";
            model.PhoneNumber = user.PhoneNumber;

            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(ProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            ApplicationUser? user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login");
            }

            user.FullName = model.FullName;
            user.PhoneNumber = model.PhoneNumber;

            IdentityResult result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                foreach (IdentityError e in result.Errors)
                {
                    ModelState.AddModelError("", e.Description);
                }
                return View(model);
            }

            ViewBag.Message = "Profile updated successfully!";
            return View(model);
        }





        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            // IMPORTANT: Do NOT default returnUrl here to avoid overwriting valid nulls which indicate we should decide
            // returnUrl ??= Url.Content("~/"); 

            if (ModelState.IsValid)
            {
                var result = await _accountService.LoginAsync(model.Username, model.Password);

                if (result.Succeeded)
                {
                    TempData["SuccessMessage"] = "Login successful!";

                    // 1. If ReturnUrl is present and local, use it (user was redirected from a protected page)
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl) && returnUrl != "/")
                    {
                         return Redirect(returnUrl);
                    }

                    // 2. Otherwise, Redirect based on Role
                    var user = await _userManager.FindByNameAsync(model.Username);
                    if (user != null)
                    {
                        var roles = await _userManager.GetRolesAsync(user);

                        if (roles.Contains("Admin") || roles.Contains("Manager"))
                        {
                            return RedirectToAction("Index", "Dashboard");
                        }
                        if (roles.Contains("Customer"))
                        {
                            return RedirectToAction("Search", "Rooms"); // Redirect Customers to Search Page
                        }
                    }

                    // 3. Fallback to Home
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }
            }

            return View(model);
        }

        // GET: /Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Username,
                    FullName = model.FullName,
                    Email = model.Email,
                    EmailConfirmed = true
                };

                // Check if email or username already exists
                var existingUser = await _userManager.FindByEmailAsync(model.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", "Email is already taken.");
                    return View(model);
                }

                var existingName = await _userManager.FindByNameAsync(model.Username);
                if (existingName != null)
                {
                    ModelState.AddModelError("Username", "Username is already taken.");
                    return View(model);
                }

                var result = await _accountService.RegisterAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _accountService.LoginAsync(model.Username, model.Password);

                    return RedirectToAction("Login", "Account");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        // Logout action
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _accountService.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }

        // Helper method to redirect user to the correct page after login
        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");

            }
        }
    }
}
