using System.Threading;
using System.Threading.Tasks;

namespace Airmiss.Core
{
    public interface ISendableClient : IClient
    {
        Task SendAsync<T>(string correlationId, T content, CancellationToken cancellationToken);
    }
}