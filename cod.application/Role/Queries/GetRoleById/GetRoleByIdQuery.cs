using System.Linq;
using System.Threading.Tasks;
using cod.domain.Authorization;
using cod.application.Features.Base;
using cod.application.Interfaces;

namespace cod.application.Role.Queries.GetRoleById
{
    public class GetRoleByIdQuery : BaseQuery, IGetRoleByIdQuery
    {
        public GetRoleByIdQuery(IRepository repository) : base(repository)
        {
        }

        public async Task<GetRoleByIdModel> Execute(int id)
        {

            ApplicationRol rol = await
                _repository.FirstOrDefaultAsync<ApplicationRol>(role => role.Id == id);

            return new GetRoleByIdModel
            {
                Id = rol.Id,
                Name = rol.Name,
                Description = rol.Description,
                Permissions = rol.ApplicationRolePermissions.Select(role => role.PermissionId).ToList()
            };

        }
    }
}