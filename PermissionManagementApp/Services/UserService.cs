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

		public async Task AddOrEditUserAsync(User user)
		{
			if(user.Id == 0)
			{

			await dbContext.Users.AddAsync(user);
			}
			else
			{
				var userToEdit = await GetUserByIdAsync(user.Id);
				if(userToEdit != null)
				{
					userToEdit.Name = user.Name;
				}
			}
			await dbContext.SaveChangesAsync();
		}

		public async Task<User?> GetUserByNameAsync(string userName)
		{
			return await dbContext.Users.FirstOrDefaultAsync(u=> u.Name == userName);
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
