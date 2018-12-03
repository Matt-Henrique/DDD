namespace Invoisys.Domain.Entity
{
    public class Model : EntityBase
    {
        public string Name { get; protected set; }
        protected Model() { }
        public Model(string name)
        {
            Name = name;
        }
    }
}