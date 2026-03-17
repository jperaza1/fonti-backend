using System.Collections.Generic;

namespace cod.application.GroupRole.Queries.GetPermissionsByCurrentTenant
{
    public class GroupRoleDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? GroupRoleId { get; set; }
        public string GroupRole { get; set; }
        public IEnumerable<GroupRolePermissionDto> Permissions { get; set; }
    }
}