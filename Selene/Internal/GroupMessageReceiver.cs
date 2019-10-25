using Selene.Messaging;
using System.Collections;
using System.Collections.Generic;

namespace Selene.Internal
{
    internal class GroupMessageReceiver : MessageReceiver, IEnumerable<MessageReceiver>
    {
        private IList<MessageReceiver> Receivers { get; }

        public GroupMessageReceiver(string identity) : base(identity)
        {
            Receivers = new List<MessageReceiver>();
        }

        public void Add(MessageReceiver receiver) => Receivers.Add(receiver);

        public void Remove(MessageReceiver receiver) => Receivers.Remove(receiver);

        #region IEnumerable members
        public IEnumerator<MessageReceiver> GetEnumerator()
        {
            return Receivers.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion
    }
}
