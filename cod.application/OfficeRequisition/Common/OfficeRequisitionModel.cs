using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static cod.domain.Features.Enums.DomainEnums;

namespace cod.application.OfficeRequisition.Common
{
    public class OfficeRequisitionModel
    {
        public int Id { get; set; }
        public string Reference { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public string Cafeteria { get; set; }
        public int CafeteriaId { get; set; }
        public string StatusName { get; set; }
        public string ApprovedUser { get; set; }
        public string CreatorUser { get; set; }
        public DateTime CreationTime { get; set; }
        public SpecialServiceStatus Status { get; set; }
        public string ReasonRejection { get; set; }
        public List<OfficeRequisitionDetailModel> Details { get; set; }

    }

    public class OfficeRequisitionDetailModel
    {
        public int Id { get; set; }
        public int OfficeRequisitionId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public double Quantity { get; set; }
    }
}
