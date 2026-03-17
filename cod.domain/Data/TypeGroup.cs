using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using cod.domain.Common;

namespace cod.domain.Data
{
    [Table("TypeGroups", Schema = "data")]
    public partial class TypeGroup : Entity
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public virtual ICollection<Type> Types { get; set; }
    }
}
