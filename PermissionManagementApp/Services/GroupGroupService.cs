using PermissionManagementApp.Data.Models;
using PermissionManagementApp.Data;
using Microsoft.EntityFrameworkCore;

namespace PermissionManagementApp.Services
{
    public class GroupGroupService
    {
        private readonly AppDbContext dbContext;

        public GroupGroupService(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<GroupGroup>> GetAllGroupRelationsAsync()
        {
            return await dbContext.GroupGroups
                .Include(gg => gg.ParentGroup)
                .Include(gg => gg.ChildGroup)
                .ToListAsync();
        }

        public async Task<List<Group>> GetChildGroupsAsync(int parentGroupId)
        {
            return await dbContext.GroupGroups
                .Where(gg => gg.ParentGroupId == parentGroupId)
                .Select(gg => gg.ChildGroup)
                .ToListAsync();
        }

        public async Task<List<Group>> GetParentGroupsAsync(int childGroupId)
        {
            return await dbContext.GroupGroups
                .Where(gg => gg.ChildGroupId == childGroupId)
                .Select(gg => gg.ParentGroup)
                .ToListAsync();
        }

        public async Task AddGroupRelationAsync(int parentGroupId, int childGroupId)
        {
            if (parentGroupId == childGroupId) return;

            var exists = await dbContext.GroupGroups
                .AnyAsync(gg => gg.ParentGroupId == parentGroupId && gg.ChildGroupId == childGroupId);

            if (!exists)
            {
                dbContext.GroupGroups.Add(new GroupGroup
                {
                    ParentGroupId = parentGroupId,
                    ChildGroupId = childGroupId
                });
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task RemoveGroupRelationAsync(int parentGroupId, int childGroupId)
        {
            var relation = await dbContext.GroupGroups
                .FirstOrDefaultAsync(gg => gg.ParentGroupId == parentGroupId && gg.ChildGroupId == childGroupId);

            if (relation != null)
            {
                dbContext.GroupGroups.Remove(relation);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
