using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using cod.application.Features.Base;
using cod.application.Interfaces;

namespace cod.application.Type.Queries.GetActivesTypes
{
    public class GetActivesTypesQuery : BaseQuery, IGetActivesTypesQuery
    {
        public GetActivesTypesQuery(IRepository repository) : base(repository)
        {
        }
        public async Task<List<GetActivesTypesModel>> Execute()
        {
            return await _repository.QueryActive<domain.Data.Type>().Select(type => new GetActivesTypesModel
            {
                Id = type.Id,
                TypeGroupName = type.TypeGroup.Name,
                Name = type.Name,
                Code = type.Code,
                TypeGroupId = type.TypeGroupId
            }).ToListAsync();
        }
    }
}
