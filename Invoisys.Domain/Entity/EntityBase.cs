using System;

namespace Invoisys.Domain.Entity
{
    /// <summary>
    /// Base class to be inherited by others entities.
    /// </summary>
    public abstract class EntityBase
    {
        public int Id { get; protected set; }
        public DateTime CreateDate { get; protected set; }
        public DateTime? ModifyDate { get; protected set; }
        public void SetCreated()
        {
            CreateDate = DateTime.Now;
        }
        public void SetModified()
        {
            ModifyDate = DateTime.Now;
        }
    }
}