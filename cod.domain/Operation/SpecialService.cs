using cod.domain.Common;
using cod.domain.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static cod.domain.Features.Enums.DomainEnums;

namespace cod.domain.Operation
{
    [Table("SpecialServices", Schema = "operation")]
    public class SpecialService : Entity
    {
        public string ReasonRejection { get; set; }
        public string Reference { get; set; }
        public DateTime Date { get; set; }
        public int CafeteriaId { get; set; }
        [ForeignKey("CafeteriaId")]
        public virtual Cafeteria Cafeteria { get; set; }
        public SpecialServiceStatus Status { get; set; }
        public string ApprovedUser { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public string RejectedUser { get; set; }
        public DateTime? RejectedAt { get; set; }
        public virtual ICollection<SpecialServiceDetail> SpecialServiceDetails { get; set; }

    }
}
