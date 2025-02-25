using Microsoft.EntityFrameworkCore;
using PermissionManagementApp.Data.Models;

namespace PermissionManagementApp.Data
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

		public DbSet<User> Users { get; set; }
		public DbSet<Group> Groups { get; set; }
		public DbSet<Permission> Permissions { get; set; }
		public DbSet<UserGroup> UserGroups { get; set; }
		public DbSet<GroupPermission> GroupPermissions { get; set; }
		public DbSet<UserPermission> UserPermissions { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<UserGroup>()
				.HasIndex(ug => new { ug.UserId, ug.GroupId }).IsUnique();

			modelBuilder.Entity<GroupPermission>()
				.HasIndex(gp=> new {gp.GroupId, gp.PermissionId}).IsUnique();

			modelBuilder.Entity<UserPermission>()
				.HasIndex(up=> new {up.UserId, up.PermissionId}).IsUnique();
		}
	}
}
