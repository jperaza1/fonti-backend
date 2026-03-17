using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.domain.Authorization;

namespace cod.application.Users.Queries.GetUsers
{
    public class GetUsersQuery : BaseQuery, IGetUsersQuery
    {
        public GetUsersQuery(IRepository repository) : base(repository)
        {
        }

        public Task<List<GetUsersModel>> Execute()
        {
            return _repository.Query<ApplicationUserProfile>().Select(user => new GetUsersModel
            {
                Id = user.Id,
                Email = user.Email,
                IsActive = user.IsActive,
                LastName = user.LastName,
                FirstName = user.FirstName,
                UserTypeText = user.UserType.ToString(),
                Carrier = string.Empty,
                UserId = user.UserId,
                Cafeteria = user.CafeteriaId.HasValue ? user.Cafeteria.Name : string.Empty
            }).ToListAsync();
        }
    }
}