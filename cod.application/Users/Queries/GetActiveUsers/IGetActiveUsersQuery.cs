using cod.application.Users.Queries.GetUsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Users.Queries.GetActiveUsers
{
    public interface IGetActiveUsersQuery
    {
        Task<List<GetUsersModel>> Execute();
    }
}
