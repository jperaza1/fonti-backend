using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cod.application.Type.Queries.GetTypesByGroupCode;
using Microsoft.EntityFrameworkCore;
using cod.application.Interfaces;

namespace cod.application.Type.Queries.GetTypesByGroupCodeList
{
    public class GetTypesByGroupCodeListQuery : IGetTypesByGroupCodeListQuery
    {
        private readonly IRepository _repository;

        public GetTypesByGroupCodeListQuery(IRepository repository)
        {
            _repository = repository;
        }
        public async Task<Dictionary<string, List<GetTypesByGroupCodeModel>>> Execute(List<GetTypesByGroupCodeListModel> codes)
        {
            List<string> codeList = codes.Select(code => code.Code).ToList();
            List<domain.Data.TypeGroup> typeGroups = await _repository
                .WhereAsync<domain.Data.TypeGroup>(typeGroup => codeList.Contains(typeGroup.Code)).ToListAsync();

            return typeGroups.ToDictionary(typeGroup => typeGroup.Code, typeGroup => typeGroup.Types.Where(tp => tp.IsActive).Select(itemType => new GetTypesByGroupCodeModel
            {
                Name = itemType.Name,
                Code = itemType.Code,
                Id = itemType.Id,
                TypeGroup = itemType.TypeGroup.Name,
                TypeGroupId = itemType.TypeGroupId
            }).ToList());
        }
    }
}