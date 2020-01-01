using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Selene.Core;

namespace Selene.Internal
{
    internal class AggregateMiddleware : IMiddleware
    {
        private readonly IEnumerable<Type> _middlewareTypes;
        private readonly ITypeActivator _typeActivator;

        public AggregateMiddleware(IEnumerable<Type> middlewareTypes, ITypeActivator typeActivator)
        {
            _middlewareTypes = middlewareTypes;
            _typeActivator = typeActivator;
        }

        public Task<object> InvokeAsync(IContext context, Func<Task<object>> next, CancellationToken cancellationToken)
        {
            var compiledMiddleware = CompileMiddleware(context, next, cancellationToken);
            return compiledMiddleware.Invoke();
        }

        private Func<Task<object>> CompileMiddleware(IContext context, Func<Task<object>> next, CancellationToken cancellationToken)
        {
            return _middlewareTypes.Reverse().Aggregate(next, (previous, current) =>
            {
                return () =>
                {
                    var middleware = (IMiddleware)_typeActivator.GetInstance(current);
                    try
                    {
                        return middleware.InvokeAsync(context, previous, cancellationToken);
                    }
                    finally
                    {
                        _typeActivator.Release(middleware);
                    }
                };
            });
        }
    }
}
