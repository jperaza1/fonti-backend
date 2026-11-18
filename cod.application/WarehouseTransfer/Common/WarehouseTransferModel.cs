using cod.domain.Features.Enums;
using System;
using System.Collections.Generic;

namespace cod.application.WarehouseTransfer.Common
{
    public class WarehouseTransferModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int FromWarehouseId { get; set; }
        public string FromWarehouse { get; set; }
        public int ToWarehouseId { get; set; }
        public string ToWarehouse { get; set; }
        public string Note { get; set; }
        public WarehouseTransferStatus Status { get; set; }
        public string StatusName { get; set; }
        public string SentUser { get; set; }
        public string Reason { get; set; }
        public List<WarehouseTransferDetailModel> Details { get; set; }
    }

    public class WarehouseTransferDetailModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Product { get; set; }
        public decimal Quantity { get; set; }
    }

    public class RejectWarehouseTransferModel
    {
        public int Id { get; set; }
        public string Reason { get; set; }
    }
}
