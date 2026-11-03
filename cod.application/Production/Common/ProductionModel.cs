using System;
using System.Collections.Generic;

namespace cod.application.Production.Common
{
    public class ProductionModel
    {
        public int Id { get; set; }
        public DateTime ProductionDate { get; set; }
        public int ShiftTypeId { get; set; }
        public string Shift { get; set; }
        public int ProductId { get; set; }
        public string Product { get; set; }
        public decimal Quantity { get; set; }
        public string Note { get; set; }
        public string Losses { get; set; }
        public string Status { get; set; }
        public bool IsActive { get; set; }
    }

    public class ProductionShiftModel
    {
        public int Id { get; set; }
        public DateTime ProductionDate { get; set; }
        public int ShiftTypeId { get; set; }
        public string Shift { get; set; }
        public string Products { get; set; }
        public string Status { get; set; }
        public bool IsActive { get; set; }
        public bool IsClosed { get; set; }
        public List<ProductionLineModel> Lines { get; set; }
    }

    public class ProductionLineModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Product { get; set; }
        public decimal Quantity { get; set; }
        public string Note { get; set; }
        public string Losses { get; set; }
        public string Status { get; set; }
        public bool IsActive { get; set; }
    }

    public class ProductionFilterModel
    {
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }

    public class CreateProductionModel
    {
        public int ProductionShiftId { get; set; }
        public DateTime ProductionDate { get; set; }
        public int ShiftTypeId { get; set; }
        public List<CreateProductionLineModel> Lines { get; set; }
    }

    public class CreateProductionLineModel
    {
        public int ProductId { get; set; }
        public decimal Quantity { get; set; }
        public string Note { get; set; }
        public List<ProductionLossModel> Losses { get; set; }
    }

    public class ProductionLossModel
    {
        public int ProductId { get; set; }
        public decimal Quantity { get; set; }
        public string Note { get; set; }
    }

    public class ProductionProductModel
    {
        public int ProductId { get; set; }
        public string Product { get; set; }
    }

    public class ProductionConsumptionRequest
    {
        public int ProductId { get; set; }
        public decimal Quantity { get; set; }
    }

    public class ProductionConsumptionModel
    {
        public int ProductId { get; set; }
        public string Product { get; set; }
        public string UnitOfMeasure { get; set; }
        public decimal Quantity { get; set; }
    }
}
