using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PermissionManagementApp.Data.Models
{
	public class UserGroup
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public int UserId { get; set; }
		[ForeignKey("UserId")]
		public User User { get; set; }

		[Required]
		public int GroupId { get; set; }
		[ForeignKey("GroupId")]
		public Group Group { get; set; }
	}
}
