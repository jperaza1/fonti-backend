using System.Threading.Tasks;

namespace cod.application.Configuration.Queries.GetConfigurationById
{
    public interface IGetConfigurationByIdQuery
    {
        Task<GetConfigurationByIdModel> Execute(int id);
    }
}