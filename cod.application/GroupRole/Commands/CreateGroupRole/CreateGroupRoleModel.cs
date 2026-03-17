using System.Collections.Generic;

namespace cod.application.GroupRole.Commands.CreateGroupRole
{
    public class CreateGroupRoleModel
    {
        public int? GroupHeaderId { get; set; }
        public string Name { get; set; }
        public string GroupHeader { get; set; }
        public List<CreateGroupRolePermissionModel> Permissions { get; set; }
    }
}