using System.Collections.Generic;

namespace cod.application.Role.Commands.CreateRole
{
    public class CreateRoleModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> Permissions { get; set; }
    }
}