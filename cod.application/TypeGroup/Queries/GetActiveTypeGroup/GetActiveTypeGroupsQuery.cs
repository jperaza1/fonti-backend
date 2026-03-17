using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using cod.application.Features.Base;
using cod.application.Interfaces;

namespace cod.application.TypeGroup.Queries.GetActiveTypeGroup
{
    public class GetActiveTypeGroupsQuery : BaseQuery, IGetActiveTypeGroupsQuery
    {
        public GetActiveTypeGroupsQuery(IRepository repository) : base(repository)
        {
        }
        public async Task<List<GetActiveTypeGroupModel>> Execute()
        {
            return await _repository.AllActive<domain.Data.TypeGroup>().Select(type => new GetActiveTypeGroupModel
            {
                Id = type.Id,
                Name = type.Name,
                Code = type.Code,         
            }).ToListAsync();
        }
    }
}
