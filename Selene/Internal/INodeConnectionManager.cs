using Selene.Messaging;

namespace Selene.Internal.Providers
{
    internal interface INodeConnectionManager
    {
        ConnectionContext GetConnectionContext(string connectionId);

        void SetConnectionContext(ConnectionContext connectionContext);

        void ReleaseConnection(string connectionId);
    }
}