using System;
using System.Threading;
using System.Threading.Tasks;

namespace Airmiss.Core
{
    public interface IMiddleware
    {
        Task<object?> InvokeAsync(IContext context, Func<Task<object?>> next, CancellationToken cancellationToken);
    }
}