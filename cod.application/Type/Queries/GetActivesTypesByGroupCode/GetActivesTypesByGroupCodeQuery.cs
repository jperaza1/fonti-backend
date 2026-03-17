using Microsoft.EntityFrameworkCore;
using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.application.Type.Queries.GetTypesByGroupCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Type.Queries.GetActivesTypesByGroupCode
{
    public class GetActivesTypesByGroupCodeQuery : BaseQuery, IGetActivesTypesByGroupCodeQuery
    {
        public GetActivesTypesByGroupCodeQuery(IRepository repository) : base(repository)
        {
        }

        public async Task<List<GetTypesByGroupCodeModel>> Execute(string groupCode)
        {
            domain.Data.TypeGroup typeGroup = await _repository
                .FirstOrDefaultAsync<domain.Data.TypeGroup>(group => group.Code == groupCode);

            int typeGroupId = typeGroup?.Id ?? 0;

            return await _repository
                .QueryActive<domain.Data.Type>(type => type.TypeGroupId == typeGroupId && type.IsActive)
                .OrderBy(g => g.Name)
                .Select(type => new GetTypesByGroupCodeModel
                {
                    TypeGroupId = type.TypeGroupId,
                    Id = type.Id,
                    Name = type.Name,
                    Code = type.Code
                }).ToListAsync();
        }
    }
}
