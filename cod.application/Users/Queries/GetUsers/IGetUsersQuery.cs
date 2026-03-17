using System.Collections.Generic;
using System.Threading.Tasks;

namespace cod.application.Users.Queries.GetUsers
{
    public interface IGetUsersQuery
    {
        Task<List<GetUsersModel>> Execute();
    }
}
