using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cod.domain.Authorization;
using Microsoft.EntityFrameworkCore;
using cod.application.Features.Base;
using cod.application.Interfaces;

namespace cod.application.GroupRole.Queries.GetGroupRoles
{
    public class GetGroupRolesQuery : BaseQuery, IGetGroupRolesQuery
    {
        public GetGroupRolesQuery(IRepository repository) : base(repository)
        {
        }

        public async Task<List<GetGroupRolesModel>> Execute()
        {
            return await _repository
                .QueryActive<ApplicationGroupRole>()
                .Select(group => new GetGroupRolesModel
                {
                    Id = group.Id,
                    Name = group.Name,
                    GroupHeader = group.GroupHeader.Name ?? string.Empty,
                    GroupHeaderId = group.GroupHeaderId,
                    IsActive = group.IsActive
                }).ToListAsync();
        }
    }
}