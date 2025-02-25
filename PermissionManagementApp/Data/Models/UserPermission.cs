using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PermissionManagementApp.Data.Models
{
	public class UserPermission
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public int UserId { get; set; }
		[ForeignKey("UserId")]
		public User User { get; set; }

		[Required]
		public int PermissionId { get; set; }
		[ForeignKey("PermissionId")]
		public Permission Permission { get; set; }
	}
}
