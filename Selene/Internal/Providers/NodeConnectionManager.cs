using Selene.Messaging;
using System;
using System.Collections.Generic;

namespace Selene.Providers
{
    internal class NodeConnectionManager : INodeConnectionManager
    {
        private readonly IDictionary<string, ConnectionContext> _connectedNodes;

        public NodeConnectionManager()
        {
            _connectedNodes = new Dictionary<string, ConnectionContext>();
        }

        public ConnectionContext GetConnectionContext(string connectionId)
        {
            if (string.IsNullOrWhiteSpace(connectionId))
                throw new ArgumentException("Connection id cannot be null or white space", nameof(connectionId));

            return _connectedNodes.TryGetValue(connectionId, out var connectionContext) ? connectionContext : new ConnectionContext(connectionId);
        }

        public void SetConnectionContext(ConnectionContext connectionContext)
        {
            if (string.IsNullOrWhiteSpace(connectionContext?.ConnectionId))
                throw new ArgumentException("Connection id cannot be null or white space", nameof(connectionContext));

            _connectedNodes[connectionContext.ConnectionId] = connectionContext;
        }

        public void ReleaseConnection(string connectionId)
        {
            _connectedNodes.Remove(connectionId);
        }
    }
}
