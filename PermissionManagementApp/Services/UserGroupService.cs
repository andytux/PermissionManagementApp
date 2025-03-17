using Microsoft.EntityFrameworkCore;
using PermissionManagementApp.Data;
using PermissionManagementApp.Data.Models;

namespace PermissionManagementApp.Services
{
	public class UserGroupService
	{
		private readonly AppDbContext dbContext;

		public UserGroupService(AppDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		public async Task<List<Group>> GetUserGroupsAsync(int userId)
		{
			return await dbContext.UserGroups
				.Where(ug => ug.UserId == userId)
				.Select(ug => ug.Group)
				.ToListAsync();
		}

		public async Task AssignUserToGroupAsync (int userId, int groupId)
		{
			var exists = await dbContext.UserGroups.AnyAsync(ug => ug.UserId == userId && ug.GroupId == groupId);

			if (!exists)
			{
				dbContext.UserGroups.Add(new Data.Models.UserGroup { UserId = userId, GroupId = groupId });
				await dbContext.SaveChangesAsync();
			}
		}

		public async Task RemoveUserFromGroupAsync(int userId, int groupId)
		{
			var entry = await dbContext.UserGroups.FirstOrDefaultAsync(ug=> ug.UserId == userId &&ug.GroupId == groupId);

			if (entry != null)
			{
				dbContext.UserGroups.Remove(entry);
				await dbContext.SaveChangesAsync();
			}
		}
	}
}
