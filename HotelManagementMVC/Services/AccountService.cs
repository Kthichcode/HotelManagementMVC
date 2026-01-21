using BusinessObjects;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Services
{
    public class AccountService : IAccountService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountService(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<SignInResult> LoginAsync(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, password, false, false);
                return result;
            }
            return SignInResult.Failed;
        }

        public async Task<IdentityResult> RegisterAsync(ApplicationUser user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            return result;
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
