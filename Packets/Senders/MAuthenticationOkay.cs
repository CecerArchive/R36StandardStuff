using IHI.Server.Network;

namespace IHI.Server.Network.Messages
{
    public class MAuthenticationOkay : OutgoingMessage
    {
        protected override void Compile()
        {
            InternalOutgoingMessage.Initialize(3);
        }
    }
}