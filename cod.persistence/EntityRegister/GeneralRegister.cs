using Microsoft.EntityFrameworkCore;
using cod.domain.General;

namespace cod.persistence.Implementations
{
    public partial class Repository
    {
        public DbSet<Configuration> Configuration { get; set; }
        public DbSet<Documentation> Documentation { get; set; }
        public DbSet<DataChangeLog> DataChangeLog { get; set; }
    }
}

