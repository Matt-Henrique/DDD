using Invoisys.Domain.Interface.UoW;
using Invoisys.Infrastructure.Data.Context;
using System;
using System.Transactions;

namespace Invoisys.Infrastructure.Data.UoW
{
    public class UoW : IDisposable, IUoW
    {
        private bool _disposed;
        private InvoisysContext _context;
        public UoW(InvoisysContext context)
        {
            _context = context;
        }
        public void Commit()
        {
            using (TransactionScope scope = new TransactionScope())
            {
                _context.SaveChanges();
                scope.Complete();
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }
    }
}