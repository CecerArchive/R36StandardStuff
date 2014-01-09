using IHI.Server.Network;

namespace IHI.Server.Network.Messages
{
    public class MSetupEncryption : OutgoingMessage
    {
        public bool UnknownA { get; set; }

        protected override void Compile()
        {
            InternalOutgoingMessage.Initialize(277)
                .AppendBoolean(UnknownA);
        }
    }
}