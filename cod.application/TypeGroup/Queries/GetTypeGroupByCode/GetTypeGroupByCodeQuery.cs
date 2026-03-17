using System.Threading.Tasks;
using cod.application.Features.Base;
using cod.application.Interfaces;

namespace cod.application.TypeGroup.Queries.GetTypeGroupByCode
{
    public class GetTypeGroupByCodeQuery : BaseQuery, IGetTypeGroupByCodeQuery
    {
        public GetTypeGroupByCodeQuery(IRepository repository) : base(repository)
        {
        }

        public async Task<GetTypeGroupByCodeModel> Execute(string code)
        {
            domain.Data.TypeGroup typeGroup = await
                _repository.FirstOrDefaultAsync<domain.Data.TypeGroup>(group => group.Code == code);
            return new GetTypeGroupByCodeModel
            {
                Id = typeGroup.Id,
                Name = typeGroup.Name
            };
        }
    }
}