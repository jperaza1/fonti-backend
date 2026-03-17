using cod.domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.domain.General
{
    [Table("DataChangeLogs", Schema = "gen")]
    public class DataChangeLog : EntityTenantable
    {
        public int EntityId { get; set; }
        public string Entity { get; set; }
        public DateTime Date { get; set; }
        public string Log { get; set; }
    }
}
