using Microsoft.EntityFrameworkCore;
using cod.domain.Data;

namespace cod.persistence.Implementations
{
    public partial class Repository
    {
        public DbSet<Type> Type { get; set; }
        public DbSet<TypeGroup> TypeGroup { get; set; }
        public DbSet<Vendor> Vendor { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Correlative> Correlatives { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
            
    }
}
