using System.Collections.Generic;

namespace cod.application.GroupRole.Queries.GetGroupRolePermissions
{
    public class GetGroupPermissionModel
    {
        public string PermissionId { get; set; }
        public string NormalizedName { get; set; }
        public string DisplayName { get; set; }
        public string Parent { get; set; }
        public bool IsChild { get; set; }
        public List<GetGroupPermissionModel> Children { get; set; }
        public GetGroupPermissionModel()
        {
            this.Children = new List<GetGroupPermissionModel>();
        }
    }
}