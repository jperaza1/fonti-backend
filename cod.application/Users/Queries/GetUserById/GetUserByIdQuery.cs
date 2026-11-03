using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.domain.Authorization;
using cod.domain.Features.Enums;

namespace cod.application.Users.Queries.GetUserById
{
    public class GetUserByIdQuery : BaseQuery, IGetUserByIdQuery
    {
        public GetUserByIdQuery(IRepository repository) : base(repository)
        {
        }

        public async Task<GetUserByIdModel> Execute(int id)
        {
            ApplicationUserProfile profile = await _repository.FirstOrDefaultAsync<ApplicationUserProfile>(user => user.Id == id);

            List<int> userRoles = await _repository
                .Query<ApplicationUserRole>(rol => rol.UserId == profile.UserId)
                .Select(rol => rol.RolId)
                .ToListAsync();

            List<DomainEnums.UserAlertOption> userAlerts = await _repository.QueryActive<ApplicationUserAlert>()
             .Where(userAlert => userAlert.UserId == profile.UserId)
             .Select(userAlert => userAlert.UserAlertOption)
             .ToListAsync();

            return new GetUserByIdModel
            {
                Id = profile.Id,
                LastName = profile.LastName,
                FirstName = profile.FirstName,
                UserType = profile.UserType,
                IsActive = profile.IsActive,
                UserAlerts = userAlerts,
                RoleIds = userRoles,
                Routes = UserRoutes.Parse(profile.Routes),
            };
        }
    }
}