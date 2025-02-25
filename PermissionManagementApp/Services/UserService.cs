using Microsoft.EntityFrameworkCore;
using PermissionManagementApp.Data;
using PermissionManagementApp.Data.Models;

namespace PermissionManagementApp.Services
{
	public class UserService
	{
		private readonly AppDbContext dbContext;

		public UserService(AppDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		public async Task<List<User>> GetAllUsersAsync()
		{
			return await dbContext.Users.Include(u=> u.UserGroups).ToListAsync();
		}

		public async Task<User?> GetUserByIdAsync(int id)
		{
			return await dbContext.Users.Include(u=> u.UserGroups).FirstOrDefaultAsync(u=> u.Id == id);
		}

		public async Task AddUserAsync(User user)
		{
			await dbContext.Users.AddAsync(user);
			await dbContext.SaveChangesAsync();
		}

		public async Task DeleteUserAsync(int userId)
		{
			var user = await GetUserByIdAsync(userId);

			if(user != null)
			{
				dbContext.Users.Remove(user);
				await dbContext.SaveChangesAsync();
			}
		}
	}
}
