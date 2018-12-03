using Invoisys.Domain.Entity;

namespace Invoisys.AppService.Interface
{
    public interface IModelAppService : IAppService<Model>
    {
        void DeleteById(int id);
        void DeleteAll();
    }
}