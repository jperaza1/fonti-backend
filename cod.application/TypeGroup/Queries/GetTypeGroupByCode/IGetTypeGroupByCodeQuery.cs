using System.Threading.Tasks;

namespace cod.application.TypeGroup.Queries.GetTypeGroupByCode
{
    public interface IGetTypeGroupByCodeQuery
    {
        Task<GetTypeGroupByCodeModel> Execute(string code);
    }
}
