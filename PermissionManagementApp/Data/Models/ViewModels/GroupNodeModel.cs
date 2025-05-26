namespace PermissionManagementApp.Data.Models.ViewModels
{
    public class GroupNodeModel
    {
        public Group Group { get; set; }
        public List<GroupNodeModel> Children { get; set; } = new();
    }
}
