using Microsoft.EntityFrameworkCore;
using PermissionManagementApp.Data;
using PermissionManagementApp.Data.Models;

namespace PermissionManagementApp.Services
{
	public class GroupService
	{
		private readonly AppDbContext dbContext;

		public GroupService(AppDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		public async Task<List<Group>> GetAllGroupsAsync()
		{
			return await dbContext.Groups.Include(g => g.UserGroups).ToListAsync();
		}

		public async Task<Group?> GetGroupByIdAsync(int groupId)
		{
			return await dbContext.Groups.Include(g => g.UserGroups).FirstOrDefaultAsync(g => g.Id == groupId);
		}

		public async Task AddGroupAsync(Group group)
		{
			await dbContext.Groups.AddAsync(group);
			await dbContext.SaveChangesAsync();
		}

		public async Task UpdateGroupAsync(Group group)
		{
			dbContext.Groups.Update(group);
			await dbContext.SaveChangesAsync();
		}

		public async Task DeleteGroupAsync(int id)
		{
			var group = await GetGroupByIdAsync(id);
			if (group != null)
			{
				dbContext.Groups.Remove(group);
				await dbContext.SaveChangesAsync();
			}

		}
	}
}
