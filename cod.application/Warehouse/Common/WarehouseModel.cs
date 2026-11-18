using cod.domain.Features.Enums;

namespace cod.application.Warehouse.Common
{
    public class WarehouseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public WarehouseKind Kind { get; set; }
        public string KindName { get; set; }
    }
}
