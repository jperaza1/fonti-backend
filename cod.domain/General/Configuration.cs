using System.ComponentModel.DataAnnotations.Schema;
using cod.domain.Common;

namespace cod.domain.General
{
    [Table("Configurations", Schema = "gen")]
    public class Configuration : EntityTenantable
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
