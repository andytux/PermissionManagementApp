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

        public async Task<List<Permission>> GetUserPermissionsRecursiveAsync(int userId)
        {
            var visitedGroupIds = new HashSet<int>();
            var permissions = new List<Permission>();

            // 1. Direkte User-Permissions
            var userPermissions = await dbContext.UserPermissions
                .Where(up => up.UserId == userId)
                .Select(up => up.Permission)
                .ToListAsync();

            permissions.AddRange(userPermissions);

            // 2. Starte mit den direkten Gruppen des Users
            var directGroupIds = await dbContext.UserGroups
                .Where(ug => ug.UserId == userId)
                .Select(ug => ug.GroupId)
                .ToListAsync();

            // 3. Rekursiv alle Gruppen und deren Permissions aufbauen
            var allGroupIds = await GetAllNestedGroupIdsAsync(directGroupIds, visitedGroupIds);

            if (allGroupIds.Count > 0)
            {
                var groupPermissions = await dbContext.GroupPermissions
                    .Where(gp => allGroupIds.Contains(gp.GroupId))
                    .Select(gp => gp.Permission)
                    .ToListAsync();

                permissions.AddRange(groupPermissions);
            }

            // 4. Duplikate entfernen (Permissions mit gleicher ID)
            return permissions
                .GroupBy(p => p.Id)
                .Select(g => g.First())
                .ToList();
        }

        private async Task<List<int>> GetAllNestedGroupIdsAsync(List<int> currentGroupIds, HashSet<int> visited)
        {
            var allGroups = new List<int>();

            foreach (var groupId in currentGroupIds)
            {
                if (!visited.Add(groupId)) continue;

                allGroups.Add(groupId);

                var childGroupIds = await dbContext.Set<GroupGroup>()
                    .Where(gg => gg.ParentGroupId == groupId)
                    .Select(gg => gg.ChildGroupId)
                    .ToListAsync();

                var nested = await GetAllNestedGroupIdsAsync(childGroupIds, visited);
                allGroups.AddRange(nested);
            }

            return allGroups.Distinct().ToList();
        }

    }
}
