using Microsoft.EntityFrameworkCore;
using cod.domain.Data;

namespace cod.persistence.Implementations
{
    public partial class Repository
    {
        public DbSet<Type> Type { get; set; }
        public DbSet<TypeGroup> TypeGroup { get; set; }
        public DbSet<Vendor> Vendor { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<ProductVendor> ProductVendors { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<Cafeteria> Cafeteria { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<CustomerCafeteria> CustomerCafeteria { get; set; }
        public DbSet<VacationPolicy> VacationPolicies { get; set; }
        public DbSet<ProductRoutePrice> ProductRoutePrices { get; set; }
        public DbSet<Distributor> Distributors { get; set; }
        public DbSet<ProductDistributorPrice> ProductDistributorPrices { get; set; }
        public DbSet<Correlative> Correlatives { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<Truck> Trucks { get; set; }
        public DbSet<Bank> Banks { get; set; }
        public DbSet<UnitOfMeasure> UnitOfMeasures { get; set; }
        public DbSet<ProductRecipe> ProductRecipes { get; set; }
        public DbSet<ProductRecipeDetail> ProductRecipeDetails { get; set; }

    }
}
