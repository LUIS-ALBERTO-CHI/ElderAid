using System;
using System.Threading;

namespace FwaEu.Fwamework.DependencyInjection
{

    //Note: taken from https://github.com/dotnet/aspnetcore/blob/main/src/Http/Http/src/HttpContextAccessor.cs
    public class AsyncLocalScopedServiceProviderAccessor : IScopedServiceProviderAccessor
    {
        public IServiceProvider GetScopeServiceProvider()
        {
            return ScopedServiceProvider;
        }

        private static readonly AsyncLocal<ServiceProviderHolder> _scopedServiceProviderCurrent = new AsyncLocal<ServiceProviderHolder>();

        private IServiceProvider ScopedServiceProvider
        {
            get
            {
                return _scopedServiceProviderCurrent.Value?.ServiceProvider;
            }

            set
            {
                var holder = _scopedServiceProviderCurrent.Value;
                if (holder != null)
                {
                    // Clear current ScopedServiceProvider trapped in the AsyncLocals, as its done.
                    holder.ServiceProvider = null;
                }

                if (value != null)
                {
                    // Use an object indirection to hold the ScopedServiceProvider in the AsyncLocal,
                    // so it can be cleared in all ExecutionContexts when its cleared.
                    _scopedServiceProviderCurrent.Value = new ServiceProviderHolder { ServiceProvider = value };
                }
            }
        }

        public IDisposable BeginScope(IServiceProvider scopedServiceProvider)
        {
            return new ScopedServiceProviderDisposer(this, scopedServiceProvider);
        }

        private class ServiceProviderHolder
        {
            public IServiceProvider ServiceProvider;
        }

        private class ScopedServiceProviderDisposer : IDisposable
        {
            private readonly AsyncLocalScopedServiceProviderAccessor _asyncLocalScopedServiceProviderAccessor;

            public ScopedServiceProviderDisposer(AsyncLocalScopedServiceProviderAccessor asyncLocalScopedServiceProviderAccessor, IServiceProvider scopedServiceProvider)
            {
                this._asyncLocalScopedServiceProviderAccessor = asyncLocalScopedServiceProviderAccessor;
                this._asyncLocalScopedServiceProviderAccessor.ScopedServiceProvider = scopedServiceProvider;
            }

            public void Dispose()
            {
                this._asyncLocalScopedServiceProviderAccessor.ScopedServiceProvider = null;
            }
        }
    }
}
