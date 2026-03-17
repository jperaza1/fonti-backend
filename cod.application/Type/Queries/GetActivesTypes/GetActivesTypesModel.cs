namespace cod.application.Type.Queries.GetActivesTypes
{
    public class GetActivesTypesModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int TypeGroupId { get; set; }
        public string TypeGroupName { get; set; }
        public bool IsActive { get; set; }
    }
}
