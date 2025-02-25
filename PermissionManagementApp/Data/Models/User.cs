using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PermissionManagementApp.Data.Models
{
	public class User
	{
		[Key]
		public int Id { get; set; }

		[Required]
		[StringLength(100)]
		public string Name { get; set; }

		public ICollection<UserGroup> UserGroups { get; set; }
		public ICollection<UserPermission> UserPermissions { get; set; }
	}
}
