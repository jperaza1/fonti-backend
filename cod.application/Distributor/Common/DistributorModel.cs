using System.Collections.Generic;

namespace cod.application.Distributor.Common
{
    public class DistributorModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public List<ProductDistributorPricesModel> ProductDistributorPrices { get; set; }
    }

    public class ProductDistributorPricesModel
    {
        public int Id { get; set; }
        public int DistributorId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public double Price { get; set; }
    }
}
