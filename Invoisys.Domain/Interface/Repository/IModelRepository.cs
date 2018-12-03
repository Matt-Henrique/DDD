using Invoisys.Domain.Entity;

namespace Invoisys.Domain.Interface.Repository
{
    public interface IModelRepository : IRepository<Model>
    {
        void DeleteAll();
    }
}