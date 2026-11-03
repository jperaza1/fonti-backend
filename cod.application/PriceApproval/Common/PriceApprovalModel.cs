using cod.domain.Features.Enums;
using System;
using System.Collections.Generic;

namespace cod.application.PriceApproval.Common
{
    public class PriceApprovalModel
    {
        public int Id { get; set; }
        public int PurchaseOrderId { get; set; }
        public string Token { get; set; }
        public string Reference { get; set; }
        public string Vendor { get; set; }
        public PriceApprovalStatus Status { get; set; }
        public string StatusName { get; set; }
        public string Notes { get; set; }
        public string RejectedUser { get; set; }
        public DateTime? RejectedAt { get; set; }
        public string ApprovedUser { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public List<PriceApprovalDetailModel> Details { get; set; }
    }

    public class PriceApprovalDetailModel
    {
        public string Product { get; set; }
        public decimal OldCost { get; set; }
        public decimal NewCost { get; set; }
    }

    public class ApproveRequestModel
    {
        public int Id { get; set; }
        public PriceApprovalStatus Status { get; set; }
        public string Note { get; set; }
    }
}
