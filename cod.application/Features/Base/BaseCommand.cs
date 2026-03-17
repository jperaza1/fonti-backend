using cod.application.Features.Exception;
using cod.application.Interfaces;

namespace cod.application.Features.Base
{
    public class BaseCommand
    {
        protected readonly IRepository _repository;
        protected readonly IExceptionFactory _exceptionFactory;

        public BaseCommand(IRepository repository, IExceptionFactory exceptionFactory)
        {
            _repository = repository;
            _exceptionFactory = exceptionFactory;
        }
    }
}
