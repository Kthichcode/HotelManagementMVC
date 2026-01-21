using BusinessObjects;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IUserRepository
    {
        Task<ApplicationUser> GetUserByUsernameAsync(string username);
    }
}
