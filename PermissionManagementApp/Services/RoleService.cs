using Azure;
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
			var dbCon = dbContext;
			try
			{
				var userPermission = await dbCon.UserPermissions.AsNoTracking()
					.Where(up => up.UserId == userId)
					.Select(up => up.Permission)
					.ToListAsync();

				await Task.Delay(1000);

				var groupPermission = await dbCon.UserGroups
					.AsNoTracking ()
					.Where(ug => ug.UserId == userId)
					.SelectMany(ug => dbContext.GroupPermissions.Where(gp => gp.GroupId == ug.GroupId)
					.Select(gp => gp.Permission)).AsNoTracking ()
					.ToListAsync();

				return userPermission.Concat(groupPermission).Distinct().ToList();
			}
			catch (Exception ex)
			{
				return new List<Permission>();
			}

		}

		public async Task<bool> HasPermissionAsync(int userId, string permissionName)
		{
			var permissions = await GetUserPermissionsAsync(userId);
			return permissions.Any(p => p.Name == permissionName);
		}
	}
}
