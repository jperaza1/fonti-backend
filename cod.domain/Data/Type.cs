using System.ComponentModel.DataAnnotations.Schema;
using cod.domain.Common;

namespace cod.domain.Data
{
    [Table("Types", Schema = "data")]
    public partial class Type : EntityTenantable
    {
        public int TypeGroupId { get; set; }
        [ForeignKey("TypeGroupId")]
        public virtual TypeGroup TypeGroup { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }
}