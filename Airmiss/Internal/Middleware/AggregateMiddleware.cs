using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Airmiss.Core;
using Airmiss.Internal.TypeActivator;

namespace Airmiss.Internal.Middleware
{
    internal class AggregateMiddleware : IMiddleware
    {
        private readonly IEnumerable<MiddlewareDescriptor> _middlewaresDescriptors;
        private readonly ITypeActivator _typeActivator;

        public AggregateMiddleware(IEnumerable<MiddlewareDescriptor> middlewaresDescriptors,
            ITypeActivator typeActivator)
        {
            _middlewaresDescriptors = middlewaresDescriptors;
            _typeActivator = typeActivator;
        }

        public Task<object?> InvokeAsync(IContext context, Func<Task<object?>> next, CancellationToken cancellationToken)
        {
            var compiledMiddleware = CompileMiddleware(context, next, cancellationToken);
            return compiledMiddleware.Invoke();
        }

        private Func<Task<object?>> CompileMiddleware(IContext context, Func<Task<object?>> next, CancellationToken cancellationToken)
        {
            return _middlewaresDescriptors
                .Where(m => m.ShouldRun(context.Processor))
                .Select(m => m.MiddlewareType)
                .Reverse()
                .Aggregate(next, (previous, current) =>
                {
                    return () =>
                    {
                        if (_typeActivator.GetInstance(current) is not IMiddleware middleware)
                        {
                            throw new TypeLoadException($"Could not get instance for type '{current.FullName}'");
                        }

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