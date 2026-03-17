namespace cod.application.Alerts.Queries.GetAlertConfigurations
{
    public class GetAlertConfigurationsModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AlertBy { get; set; }
        public string BodyType { get; set; }
        public bool IsActive { get; set; }
    }
}
