using cod.application.Correlative.Common;
using System.Threading.Tasks;

namespace cod.application.Correlative.Queries.GetCorrelative
{
    public interface IGetCorrelativeQuery
    {
        Task<CorrelativeModel> Execute(int id);
    }
}
