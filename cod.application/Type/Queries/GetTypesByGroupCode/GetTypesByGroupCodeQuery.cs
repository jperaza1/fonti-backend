using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using cod.application.Interfaces;

namespace cod.application.Type.Queries.GetTypesByGroupCode
{
    public class GetTypesByGroupCodeQuery : IGetTypesByGroupCodeQuery
    {
        private readonly IRepository _repository;

        public GetTypesByGroupCodeQuery(IRepository repository)
        {
            _repository = repository;
        }
        public async Task<List<GetTypesByGroupCodeModel>> Execute(string groupCode)
        {
            domain.Data.TypeGroup typeGroup = await _repository
                .FirstOrDefaultAsync<domain.Data.TypeGroup>(group => group.Code == groupCode);

            int typeGroupId = typeGroup?.Id ?? 0;

            return await _repository
                .QueryActive<domain.Data.Type>(type => type.TypeGroupId == typeGroupId)
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