using cod.domain.Operation;
using cod.domain.Rrhh;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.persistence.Implementations
{
    public partial class Repository
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<WorkExperience> WorkExperience { get; set; }
        public DbSet<EmployeePersonalReference> EmployeePersonalReference { get; set; }
        public DbSet<EmployeeContractBeneficiary> EmployeeContractBeneficiary { get; set; }
        public DbSet<EmployeeChildren> EmployeeChildren { get; set; }
        public DbSet<VacationPeriod> VacationPeriods { get; set; }
        public DbSet<VacationRequested> VacationRequested { get; set; }
    }
}
