using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PermissionManagementApp.Data.Models
{
	public class GroupPermission
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public int GroupId { get; set; }
		[ForeignKey("GroupId")]
		public Group Group { get; set; }

		[Required]
		public int PermissionId { get; set; }
		[ForeignKey("PermissionId")]
		public Permission Permission { get; set; }
	}
}
