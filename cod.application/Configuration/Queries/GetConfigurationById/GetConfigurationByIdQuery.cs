using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using cod.application.Features.Base;
using cod.application.Interfaces;

namespace cod.application.Configuration.Queries.GetConfigurationById
{
    public class GetConfigurationByIdQuery : BaseQuery, IGetConfigurationByIdQuery
    {
        public GetConfigurationByIdQuery(IRepository repository) : base(repository)
        {
        }

        public Task<GetConfigurationByIdModel> Execute(int id)
        {
            return _repository.Query<domain.General.Configuration>()
                .Select(conf => new GetConfigurationByIdModel
                {
                    Id = conf.Id,
                    IsActive = conf.IsActive,
                    Key = conf.Key,
                    Value = conf.Value
                })
                .FirstOrDefaultAsync(conf => conf.Id == id);
        }
    }
}
