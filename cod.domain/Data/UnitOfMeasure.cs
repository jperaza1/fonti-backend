using cod.domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace cod.domain.Data
{
    [Table("UnitOfMeasures", Schema = "data")]
    public class UnitOfMeasure : Entity
    {
        public string Name { get; set; }
    }
}
