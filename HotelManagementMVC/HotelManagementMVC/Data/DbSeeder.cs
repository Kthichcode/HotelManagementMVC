using BusinessObjects;
using Microsoft.AspNetCore.Identity;

namespace HotelManagementMVC.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();

            var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // 1) Seed roles
            string[] roles = { "Admin", "Staff", "Manager", "Customer" };

            foreach (var role in roles)
            {
                if (!await roleMgr.RoleExistsAsync(role))
                {
                    var roleResult = await roleMgr.CreateAsync(new IdentityRole(role));
                    if (!roleResult.Succeeded)
                    {
                        throw new Exception($"Create role '{role}' failed: " +
                            string.Join("; ", roleResult.Errors.Select(e => e.Description)));
                    }
                }
            }

            // 2) Seed admin account (login by username)
            var adminUsername = "Admin";
            var admin = await userMgr.FindByNameAsync(adminUsername);

            if (admin == null)
            {
                admin = new ApplicationUser
                {
                    UserName = adminUsername,
                    Email = "admin@hotel.com", // email hợp lệ để tránh fail validate
                    FullName = "System Admin"
                };

                var createUser = await userMgr.CreateAsync(admin, "Admin@123");
                if (!createUser.Succeeded)
                {
                    throw new Exception("Create admin failed: " +
                        string.Join("; ", createUser.Errors.Select(e => e.Description)));
                }

                var addRole = await userMgr.AddToRoleAsync(admin, "Admin");
                if (!addRole.Succeeded)
                {
                    throw new Exception("Add admin role failed: " +
                        string.Join("; ", addRole.Errors.Select(e => e.Description)));
                }
            }
            else
            {
                // Nếu user đã tồn tại nhưng chưa có role Admin thì add vào cho chắc
                if (!await userMgr.IsInRoleAsync(admin, "Admin"))
                {
                    var addRole = await userMgr.AddToRoleAsync(admin, "Admin");
                    if (!addRole.Succeeded)
                    {
                        throw new Exception("Add admin role failed: " +
                            string.Join("; ", addRole.Errors.Select(e => e.Description)));
                    }
                }
            }
        }
    }
}
