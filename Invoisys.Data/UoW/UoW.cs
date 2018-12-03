using Invoisys.Domain.Interface.UoW;
using Invoisys.Infrastructure.Data.Context;
using System;
using System.Transactions;

namespace Invoisys.Infrastructure.Data.UoW
{
    public class UoW : IDisposable, IUoW
    {
        private bool _disposed;
        private readonly InvoisysContext _context;
        public UoW(InvoisysContext context)
        {
            _context = context;
        }
        public void Commit()
        {
            using (var scope = new TransactionScope())
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
        private void Dispose(bool disposing)
        {
            if (!_disposed)
                if (disposing) _context.Dispose();
            _disposed = true;
        }
    }
}