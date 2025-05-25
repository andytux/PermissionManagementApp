using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PermissionManagementApp.Data.Models
{
    public class GroupGroup
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ParentGroupId { get; set; }
        [ForeignKey("ParentGroupId")]
        public Group ParentGroup { get; set; }

        [Required]
        public int ChildGroupId { get; set; }
        [ForeignKey("ChildGroupId")]
        public Group ChildGroup { get; set; }
    }
}
