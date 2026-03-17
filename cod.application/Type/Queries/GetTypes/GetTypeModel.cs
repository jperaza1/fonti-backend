namespace cod.application.Type.Queries.GetTypes
{
    public class GetTypeModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int TypeGroupId { get; set; }
        public string TypeGroupName { get; set; }
        public bool IsActive { get; set; }
    }
}
