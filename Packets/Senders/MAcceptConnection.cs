using IHI.Server.Network;

namespace IHI.Server.Plugins.R36StandardStuff.Packets
{
    public class MAcceptConnection : OutgoingMessage
    {
        protected override void Compile()
        {
            InternalOutgoingMessage.Initialize(0);
        }
    }
}