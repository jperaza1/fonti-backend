using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using cod.application.Interfaces;

namespace cod.application.TypeGroup.Queries.GetTypeGroups
{
    public class GetTypeGroupsQuery : IGetTypeGroupsQuery
    {
        private readonly IRepository _repository;

        public GetTypeGroupsQuery(IRepository repository)
        {
            _repository = repository;
        }

        public Task<List<GetTypeGroupsModel>> Execute()
        {
            return _repository.Query<domain.Data.TypeGroup>().OrderBy(typegroup => typegroup.Name).Select(typegroup => new GetTypeGroupsModel
            {
                Id = typegroup.Id,
                Name = typegroup.Name,
                Code = typegroup.Code,
                IsActive = typegroup.IsActive
            }).ToListAsync();
        }
    }
}
