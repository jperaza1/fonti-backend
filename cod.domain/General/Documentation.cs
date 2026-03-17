using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cod.domain.Common;

namespace cod.domain.General
{
    [Table("Documentations", Schema = "gen")]
    public class Documentation : EntityTenantable
    {
        public string Entity { get; set; }
        public int EntityId { get; set; }
        public string DocumentType { get; set; }
        public string FileName { get; set; }
        public int ExternalId { get; set; }
    }
}
