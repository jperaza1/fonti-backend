namespace cod.application.GroupRole.Queries.GetGroupRolePermissions
{
    public class GetGroupRolePermissionsModel
    {
        public string PermissionId { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public int GroupRoleId { get; set; }
        public string GroupRole { get; set; }
        public int? GroupHeaderId { get; set; }
        public string GroupHeader { get; set; }
    }
}