using IHI.Server.Network;

namespace IHI.Server.Network.Messages
{
    public class MConnectionClosed : OutgoingMessage
    {
        public ConnectionClosedReason Reason { get; set; }

        protected override void Compile()
        {
            InternalOutgoingMessage.Initialize(287)
                .AppendInt32((int)Reason);
        }
    }
}