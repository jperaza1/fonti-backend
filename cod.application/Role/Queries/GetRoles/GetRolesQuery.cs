using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cod.domain.Authorization;
using Microsoft.EntityFrameworkCore;
using cod.application.Features.Base;
using cod.application.Interfaces;

namespace cod.application.Role.Queries.GetRoles
{
    public class GetRolesQuery : BaseQuery, IGetRolesQuery
    {
        public GetRolesQuery(IRepository repository) : base(repository)
        {
        }

        public async Task<List<GetRolesModel>> Execute()
        {
            return await
                _repository
                    .Query<ApplicationRol>()
                    .Select(role => new GetRolesModel
                    {
                        Id = role.Id,
                        Name = role.Name,
                        Description = role.Description,
                        IsActive = role.IsActive
                    }).ToListAsync();
        }
    }
}