namespace cod.application.Type.Queries.GetTypesByGroupCode
{
    public class GetTypesByGroupCodeModel
    {
        public int Id { get; set; }
        public int TypeGroupId { get; set; }
        public string TypeGroup { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
    }
}