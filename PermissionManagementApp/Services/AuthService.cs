using Microsoft.EntityFrameworkCore;
using PermissionManagementApp.Data;
using PermissionManagementApp.Data.Models;

namespace PermissionManagementApp.Services
{
	public class AuthService
	{
		private readonly AppDbContext dbContext;
		private User? currentUser;

		public AuthService(AppDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		public async Task<bool> LoginAsync(string userName)
		{
			currentUser = await dbContext.Users.FirstOrDefaultAsync(u => u.Name == userName);
			return currentUser != null;
		}

		public User GetCurrentUser() => currentUser;
		
		public void Logout() { 
		currentUser = null;
		}

		public int? GetCurrentUserId() => currentUser?.Id;
	}
}

