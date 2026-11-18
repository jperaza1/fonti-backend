using cod.domain.Common;
using cod.domain.Features.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace cod.domain.Operation
{
    [Table("Warehouses", Schema = "operation")]
    public class Warehouse : Entity
    {
        public string Name { get; set; }
        public WarehouseKind Kind { get; set; }
    }
}
