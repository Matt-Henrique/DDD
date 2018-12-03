using Invoisys.AppService.Interface;
using Invoisys.Domain.Entity;
using Invoisys.Domain.Interface.Service;
using Invoisys.Domain.Interface.UoW;

namespace Invoisys.AppService
{
    public class ModelAppService : AppService<Model>, IModelAppService
    {
        private readonly IModelService _service;
        private readonly IUoW _uow;
        public ModelAppService(IModelService service, IUoW uow) : base(service, uow)
        {
            _service = service;
            _uow = uow;
        }
        public void DeleteById(int id)
        {
            var model = _service.GetById(id);
            _service.Delete(model);
            _uow.Commit();
        }
        public void DeleteAll()
        {
            _service.DeleteAll();
        }
    }
}