using Microsoft.EntityFrameworkCore;
using PermissionManagementApp.Data;
using PermissionManagementApp.Data.Models;

namespace PermissionManagementApp.Services
{
	public class GroupPermissionService
	{
		private readonly AppDbContext dbContext;

		public GroupPermissionService(AppDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		public async Task<List<Permission>> GetGroupPermissionsAsync(int groupId)
		{
			return await dbContext.GroupPermissions
				.Where(gp => gp.GroupId == groupId)
				.Select(gp => gp.Permission)
				.ToListAsync();
		}

		public async Task AssignPermissionToGroupAsync(int groupId, int permissionId)
		{
			var exists = await dbContext.GroupPermissions.AnyAsync(gp => gp.GroupId == groupId && gp.PermissionId == permissionId);

			if (!exists)
			{
				dbContext.GroupPermissions.Add(new GroupPermission { GroupId = groupId, PermissionId = permissionId });
				await dbContext.SaveChangesAsync();
			}
		}

		public async Task RemovePermissionFromGroupAsync(int groupId, int permissionId)
		{
			var entry = await dbContext.GroupPermissions.FirstOrDefaultAsync(gp => gp.GroupId == groupId && gp.Permission.Id == permissionId);
			if (entry != null)
			{
				dbContext.GroupPermissions.Remove(entry);
				await dbContext.SaveChangesAsync();
			}
		}
	}
}
