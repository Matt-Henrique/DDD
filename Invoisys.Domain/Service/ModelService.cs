using Invoisys.Domain.Entity;
using Invoisys.Domain.Interface.Repository;
using Invoisys.Domain.Interface.Service;

namespace Invoisys.Domain.Service
{
    public class ModelService : Service<Model>, IModelService
    {
        private readonly IModelRepository _repository;
        public ModelService(IModelRepository repository) : base(repository)
        {
            _repository = repository;
        }
        public void DeleteAll()
        {
            _repository.DeleteAll();
        }
    }
}