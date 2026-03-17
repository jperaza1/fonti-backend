namespace cod.application.Alerts.Commands.CreateAlertConfiguration
{
    public class AlertConfigurationLanguageModel
    {
        public int Id { get; set; }
        public int AlertConfigurationId { get; set; }
        public string LanguageType { get; set; }
        public string BodyTemplate { get; set; }
        public string[] BodyFields { get; set; }
    }
}
