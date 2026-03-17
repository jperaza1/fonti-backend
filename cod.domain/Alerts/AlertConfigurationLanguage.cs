using cod.domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace cod.domain.Alerts
{
    [Table("AlertConfigurationLanguages", Schema = "alerts")]
    public class AlertConfigurationLanguage : Entity
    {
        public int AlertConfigurationId { get; set; }
        [ForeignKey("AlertConfigurationId")]
        public virtual AlertConfiguration AlertConfiguration { get; set; }
        public string LanguageType { get; set; }
        public string BodyTemplate { get; set; }
        public string[] BodyFields { get; set; }
    }
}
