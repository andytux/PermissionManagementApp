using System.ComponentModel.DataAnnotations;

namespace PermissionManagementApp.Data.Models
{
	public class Group
	{
		[Key]
		public int Id { get; set; }

		[Required]
		[StringLength(100)]
		public string Name { get; set; }

		public ICollection<UserGroup> UserGroups { get; set; }
		public ICollection<GroupPermission> GroupPermissions { get; set; }
	}
}
