using Microsoft.EntityFrameworkCore;
using PermissionManagementApp.Data;
using PermissionManagementApp.Data.Models;

namespace PermissionManagementApp.Services
{
	public class UserPermissionService
	{
		private readonly AppDbContext dbContext;

		public UserPermissionService(AppDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		public async Task<List<Permission>> GetUserPermissionsAsync(int userId)
		{
			return await dbContext.UserPermissions
				.Where(up  => up.UserId == userId)
				.Select(up => up.Permission)
				.ToListAsync();
		}

		public async Task AssignPermissionToUserAsync(int userId, int permissionId)
		{
			var exists = await dbContext.UserPermissions.AnyAsync(up => up.UserId == userId && up.PermissionId == permissionId);

			if (!exists)
			{
				dbContext.UserPermissions.Add(new UserPermission { UserId = userId, PermissionId = permissionId });
				await dbContext.SaveChangesAsync();
			}
		}

		public async Task RemovePermissionFromUserAsync(int userId, int permissionId)
		{
			var entry = await dbContext.UserPermissions.FirstOrDefaultAsync(up => up.UserId == userId && up.PermissionId == permissionId);

			if (entry != null)
			{
				dbContext.UserPermissions.Remove(entry);
				await dbContext.SaveChangesAsync();
			}
		}
	}
}
