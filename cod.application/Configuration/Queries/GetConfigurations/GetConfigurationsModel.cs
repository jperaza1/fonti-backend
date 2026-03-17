namespace cod.application.Configuration.Queries.GetConfigurations
{
    public class GetConfigurationsModel
    {
        public int Id { get; set; }

        public bool IsActive { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }
    }
}