using cod.application.DispatchControl.Commom;
using System.Threading.Tasks;

namespace cod.application.DispatchControl.Queries.GetAllDispatchControl
{
    public interface IGetAllDispatchControlQuery
    {
        Task<DispatchControlListModel> Execute(DispatchControlFilterModel filterModel);
    }
}
