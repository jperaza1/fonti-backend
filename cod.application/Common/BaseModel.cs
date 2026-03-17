using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Common
{
    public class BaseModel
    {
        public string CreatorUser { get; set; }
        public string LastModifiedUser { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public DateTime? DisableDateTime { get; set; }
        public string DisableUser { get; set; }
        public DateTime? EnableDateTime { get; set; }
        public string EnableUser { get; set; }
    }
}
