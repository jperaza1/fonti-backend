namespace cod.application.Configuration.Queries.GetConfigurationById
{
    public class GetConfigurationByIdModel
    {
        public int Id { get; set; }

        public bool IsActive { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }
    }
}