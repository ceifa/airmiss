using Selene.Messaging;

namespace Selene.Providers
{
    internal interface INodeConnectionManager
    {
        ConnectionContext GetConnectionContext(string connectionId);

        void SetConnectionContext(ConnectionContext connectionContext);

        void ReleaseConnection(string connectionId);
    }
}
