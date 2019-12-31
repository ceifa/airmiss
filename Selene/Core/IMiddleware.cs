using System;
using System.Threading.Tasks;

namespace Selene.Core
{
    public interface IMiddleware
    {
        Func<Task<object>, IContext> Next { get; set; }

        Task<object> InvokeAsync(IContext context);
    }
}