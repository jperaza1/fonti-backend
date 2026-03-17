using System.Threading.Tasks;

namespace cod.application.Users.Queries.GetUserById
{
    public interface IGetUserByIdQuery
    {
        Task<GetUserByIdModel> Execute(int id);
    }
}
