using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geria.Infrastructure.Infrastructure
{
    public class Disposable : IDisposable
    {
        private bool _isDisposed;
        ~Disposable()
        {
            Dispose(false);
        }
        public void Dispose()
        {
            if (!_isDisposed)
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
        }
        private void Dispose(bool disposing)
        {
            if (!_isDisposed && disposing)
            {
                DisposeCore(true);
            }
            _isDisposed = true;
        }
        protected virtual void DisposeCore(bool disposing)
        {

        }

        //  Add DisposeDictionary<T> and Dictionary Method.
        protected static void DisposeMember(object member)
        {
            if (member is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }
}
