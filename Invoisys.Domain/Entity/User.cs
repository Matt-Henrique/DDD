using System;

namespace Invoisys.Domain.Entity
{
    /// <summary>
    /// This class is used like a DTO, populating Identity User table.
    /// </summary>
    public class User
    {
        public string Id { get; protected set; }
        public virtual string Email { get; protected set; }
        public virtual bool EmailConfirmed { get; protected set; }
        public virtual string PasswordHash { get; protected set; }
        public virtual string SecurityStamp { get; protected set; }
        public virtual string PhoneNumber { get; protected set; }
        public virtual bool PhoneNumberConfirmed { get; protected set; }
        public virtual bool TwoFactorEnabled { get; protected set; }
        public virtual DateTime? LockoutEndDateUtc { get; protected set; }
        public virtual bool LockoutEnabled { get; protected set; }
        public virtual int AccessFailedCount { get; protected set; }
        public virtual string UserName { get; protected set; }
        public User()
        {
            Id = Guid.NewGuid().ToString();
        }
        public void DisableLockout()
        {
            LockoutEnabled = false;
        }
    }
}