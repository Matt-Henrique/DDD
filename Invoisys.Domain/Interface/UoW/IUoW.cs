namespace Invoisys.Domain.Interface.UoW
{
    public interface IUoW
    {
        void Commit();
        void Dispose();
    }
}