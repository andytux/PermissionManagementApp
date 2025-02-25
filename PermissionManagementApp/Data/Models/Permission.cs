using System.ComponentModel.DataAnnotations;

namespace PermissionManagementApp.Data.Models
{
	public class Permission
	{
		[Key]
		public int Id { get; set; }

		[Required]
		[StringLength(100)]
		public string Name { get; set; }
	}
}
