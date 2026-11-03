using static cod.domain.Features.Enums.DomainEnums;

namespace cod.application.Truck.Common
{
    public class TruckModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Plate { get; set; }
        public TruckStatus TruckStatus { get; set; }
        public string TruckStatusName { get; set; }
        public int? RouteId { get; set; }
        public string RouteName { get; set; }
        public bool IsActive { get; set; }
    }
}
