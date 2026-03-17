using cod.application.Interfaces;

namespace cod.application.Features.Base
{
    public class BaseQuery
    {
        protected readonly IRepository _repository;

        public BaseQuery(IRepository repository)
        {
            _repository = repository;
        }
    }
}
