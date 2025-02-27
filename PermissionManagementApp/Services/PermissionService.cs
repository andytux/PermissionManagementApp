using Microsoft.EntityFrameworkCore;
using PermissionManagementApp.Data;
using PermissionManagementApp.Data.Models;

namespace PermissionManagementApp.Services
{
	public class PermissionService
	{
		private readonly AppDbContext dbContext;

		public PermissionService(AppDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		public async Task<List<Permission>> GetAllPermissionsAsync()
		{
			return await dbContext.Permissions.ToListAsync();
		}

		public async Task<Permission?> GetPermissionByIdAsync(int id)
		{
			return await dbContext.Permissions.FirstOrDefaultAsync(x => x.Id == id);
		}

		public async Task AddPermissionAsync(Permission permission)
		{
			await dbContext.Permissions.AddAsync(permission);
			await dbContext.SaveChangesAsync();
		}

		public async Task UpdatePermissionAsync(Permission permission)
		{
			dbContext.Permissions.Update(permission);
			await dbContext.SaveChangesAsync();
		}

		public async Task DeletePermissionAsync(int id)
		{
			var permission = await GetPermissionByIdAsync(id);

			if (permission != null)
			{
				dbContext.Permissions.Remove(permission);
				await dbContext.SaveChangesAsync();
			}
		}
	}
}
