using cod.domain.Features.Enums;
using System;

namespace cod.application.Kardex.Common
{
    public class KardexModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Product { get; set; }
        public string UnitOfMeasure { get; set; }
        public DateTime MovementDate { get; set; }
        public KardexMovementType MovementType { get; set; }
        public string MovementTypeName { get; set; }
        public decimal Quantity { get; set; }
        public decimal Balance { get; set; }
        public decimal? UnitCost { get; set; }
        public int? DispatchControlId { get; set; }
        public int? SourceProductId { get; set; }
        public string SourceProduct { get; set; }
        public string Note { get; set; }
    }

    public class KardexFilterModel
    {
        public int? ProductId { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }

    public class KardexEntryModel
    {
        public int ProductId { get; set; }
        public decimal Quantity { get; set; }
        public DateTime MovementDate { get; set; }
        public string Note { get; set; }
        public bool IsExit { get; set; }
    }

    public class ProductStockModel
    {
        public int ProductId { get; set; }
        public string Product { get; set; }
        public ProductType ProductType { get; set; }
        public string ProductTypeName { get; set; }
        public string UnitOfMeasure { get; set; }
        public decimal? Cost { get; set; }
        public decimal Quantity { get; set; }
    }
}
