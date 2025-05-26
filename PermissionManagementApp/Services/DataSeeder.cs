using PermissionManagementApp.Data.Models;
using PermissionManagementApp.Data;
using Microsoft.EntityFrameworkCore;

namespace PermissionManagementApp.Services
{
    public class DataSeeder
    {
        private readonly AppDbContext dbContext;

        public DataSeeder(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task SeedAsync()
        {
            // 1. User ADMIN
            var adminUser = await dbContext.Users.FirstOrDefaultAsync(u => u.Name == "ADMIN");
            if (adminUser == null)
            {
                adminUser = new User { Name = "ADMIN" };
                dbContext.Users.Add(adminUser);
                await dbContext.SaveChangesAsync();
            }

            // 2. Wichtige Permissions anlegen
            string[] defaultPermissions = new[]
            {
            "Admin", "UserManagement", "GroupManagement", "PermissionManagement", "Guest"
        };

            foreach (var permName in defaultPermissions)
            {
                if (!await dbContext.Permissions.AnyAsync(p => p.Name == permName))
                {
                    dbContext.Permissions.Add(new Permission { Name = permName });
                }
            }

            await dbContext.SaveChangesAsync();

            // 3. Permissions zuweisen
            var allPerms = await dbContext.Permissions.ToListAsync();
            foreach (var perm in allPerms)
            {
                bool alreadyAssigned = await dbContext.UserPermissions.AnyAsync(up => up.UserId == adminUser.Id && up.PermissionId == perm.Id);
                if (!alreadyAssigned)
                {
                    dbContext.UserPermissions.Add(new UserPermission
                    {
                        UserId = adminUser.Id,
                        PermissionId = perm.Id
                    });
                }
            }

            await dbContext.SaveChangesAsync();
        }
    }

}
