using Microsoft.EntityFrameworkCore;
using PermissionManagementApp.Data;
using PermissionManagementApp.Data.Models;

namespace PermissionManagementApp.Services
{
	public class RoleService
	{
		private readonly AppDbContext dbContext;

		public RoleService(AppDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		public async Task<List<Permission>> GetUserPermissionsAsync(int userId)
		{
			var userPermission = await dbContext.UserPermissions
				.Where(up => up.UserId == userId)
				.Select(up => up.Permission)
				.ToListAsync();

			var groupPermission = await dbContext.UserGroups
				.Where(ug => ug.UserId == userId)
				.SelectMany(ug => dbContext.GroupPermissions.Where(gp => gp.GroupId == ug.GroupId).Select(gp => gp.Permission))
				.ToListAsync();
		}

		public async Task<bool> HasPermissionAsync(int userId, string permissionName)
		{
			var permissions = await GetUserPermissionsAsync(userId);
			return permissions.Any(p=> p.Name == permissionName);
		}
	}
}
