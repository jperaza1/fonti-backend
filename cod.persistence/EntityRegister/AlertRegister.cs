using Microsoft.EntityFrameworkCore;
using cod.domain.Alerts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.persistence.Implementations
{
    public partial class Repository
    {
        public DbSet<AlertConfiguration> AlertConfiguration { get; set; }
        public DbSet<AlertConfigurationLanguage> AlertConfigurationLanguage { get; set; }
    }
}
