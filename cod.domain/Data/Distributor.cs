using cod.domain.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace cod.domain.Data
{
    [Table("Distributors", Schema = "data")]
    public class Distributor : Entity
    {
        public string Name { get; set; }
        public virtual ICollection<ProductDistributorPrice> ProductDistributorPrices { get; set; }
    }
}
