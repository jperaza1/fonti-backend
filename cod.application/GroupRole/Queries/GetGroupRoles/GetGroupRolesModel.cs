namespace cod.application.GroupRole.Queries.GetGroupRoles
{
    public class GetGroupRolesModel
    {
        public int Id { get; set; }
        public int? GroupHeaderId { get; set; }
        public string Name { get; set; }
        public string GroupHeader { get; set; }
        public bool IsActive{ get; set; }
    }
}