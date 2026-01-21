using BusinessObjects;
using DataAccessObjects;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using System.Threading.Tasks;

namespace Repositories
{
    public class UserRepository : IUserRepository
    {      
        private readonly AccountDAO _accountDao;

        public UserRepository(AccountDAO accountDao)
        {
            _accountDao = accountDao;
        }

        public async Task<ApplicationUser> GetUserByUsernameAsync(string username)
        {
            return await _accountDao.GetUserByUsernameAsync(username);
        }
    }
}
