using Microsoft.EntityFrameworkCore;
using cod.application.Common.Extensions;
using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.application.Users.Queries.GetUsers;
using cod.domain.Authorization;
using cod.domain.Features.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Users.Queries.GetActiveUsers
{
    public class GetActiveUsersQuery : BaseQuery, IGetActiveUsersQuery
    {
        public GetActiveUsersQuery(IRepository repository) : base(repository)
        {
        }

        public async Task<List<GetUsersModel>> Execute()
        {
            List<GetUsersModel> users = await _repository.Query<ApplicationUserProfile>()
                .Where(x => x.IsActive)
                .OrderBy(x => x.FirstName)
                .Select(user => new GetUsersModel
                {
                    Id = user.Id,
                    IsActive = user.IsActive,
                    LastName = user.LastName,
                    FirstName = user.FirstName,
                    UserTypeText = user.UserType.ToString(),
                    Carrier = string.Empty,
                    UserId = user.UserId,
            }).ToListAsync();

            foreach (GetUsersModel user in users)
            {
                if (Enum.TryParse(user.UserTypeText, out DomainEnums.UserType userType))
                    user.UserTypeText = userType.GetEnumDescription();
            }

            return users;
        }
    }
}
