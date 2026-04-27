using cod.application.Common.Extensions;
using cod.application.Employee.Common;
using cod.application.Features.Base;
using cod.application.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Employee.Queries.GetEmployees
{
    public class GetEmployeesQuery : BaseQuery, IGetEmployeesQuery
    {
        public GetEmployeesQuery(IRepository repository) : base(repository)
        {
        }

        public async Task<List<EmployeeModel>> Execute()
        {
            var baseQuery = _repository.Query<domain.Rrhh.Employee>();

            var result = await baseQuery.Select(f => new EmployeeModel
            {
                Id = f.Id,
                FirstName = f.FirstName,
                LastName = f.LastName,
                Identification = f.Identification,
                GenderName = f.Gender.GetEnumDescription(),
                EmploymentTypeName = f.EmploymentType.GetEnumDescription(),
                EmploymentStatusName = f.EmploymentStatus.GetEnumDescription(),
                CafeteriaId = f.CafeteriaId,
                Cafeteria = f.Cafeteria.Name,
                Position = f.Position.Name
            }).ToListAsync();

            return result;
        }
    }
}
