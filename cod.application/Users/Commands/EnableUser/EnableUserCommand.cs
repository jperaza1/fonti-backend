using System.Threading.Tasks;
using cod.domain.Authorization;
using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;

namespace cod.application.Users.Commands.EnableUser
{
    public class EnableUserCommand : BaseCommand, IEnableUserCommand
    {
        public EnableUserCommand(IRepository repository, IExceptionFactory exceptionFactory) : base(repository, exceptionFactory)
        {
        }

        public async Task Execute(int id)
        {
            await Validate(id);
            await _repository.UpdateAsync<ApplicationUserProfile>(profile => profile.Id == id,
                newValues => new ApplicationUserProfile
                {
                    IsActive = true
                });
            await _repository.CommitAsync();
        }

        private async Task Validate(int id)
        {
            bool existUser = await _repository
                .AnyAsync<ApplicationUserProfile>(user => user.Id == id);

            if (!existUser)
                throw new ApplicationException("USER NOT FOUND", "USER");
        }
    }
}