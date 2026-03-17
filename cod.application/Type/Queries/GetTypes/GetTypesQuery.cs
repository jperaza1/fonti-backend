using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using cod.application.Features.Base;
using cod.application.Interfaces;

namespace cod.application.Type.Queries.GetTypes
{
    public class GetTypesQuery : BaseQuery, IGetTypesQuery
    {
        public GetTypesQuery(IRepository repository) : base(repository)
        {
        }
        public async Task<List<GetTypeModel>> Execute()
        {
            return await _repository.Query<domain.Data.Type>().Select(type => new GetTypeModel
            {
                Id = type.Id,
                TypeGroupName = type.TypeGroup.Name,
                Name = type.Name,
                Code = type.Code,
                TypeGroupId = type.TypeGroupId,
                IsActive = type.IsActive
            }).ToListAsync();
        }
    }
}
