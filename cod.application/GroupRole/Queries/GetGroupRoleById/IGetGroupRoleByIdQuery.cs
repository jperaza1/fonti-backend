using System.Threading.Tasks;

namespace cod.application.GroupRole.Queries.GetGroupRoleById
{
    public interface IGetGroupRoleByIdQuery
    {
        Task<GetGroupRoleByIdModel> Execute(int id);
    }
}
