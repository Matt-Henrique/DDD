using Invoisys.Domain.Entity;

namespace Invoisys.Domain.Interface.Service
{
    public interface IModelService : IService<Model>
    {
        void DeleteAll();
    }
}