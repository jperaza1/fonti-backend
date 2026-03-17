using System.Threading.Tasks;

namespace cod.application.Role.Queries.GetRoleById
{
    public interface IGetRoleByIdQuery
    {
        Task<GetRoleByIdModel> Execute(int id);
    }
}
