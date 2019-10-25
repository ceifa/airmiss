using Selene.Messaging;

namespace Selene.Internal.Providers
{
    internal interface IReceiverContextManager
    {
        ReceiverContext GetContext(MessageReceiver receiver);

        void SetContext(ReceiverContext connectionContext);

        void ReleaseContext(MessageReceiver receiver);
    }
}