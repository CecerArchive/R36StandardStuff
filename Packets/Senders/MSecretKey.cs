using IHI.Server.Network;

namespace IHI.Server.Network.Messages
{
    public class MSecretKey : OutgoingMessage
    {
        public string Key { get; set; }

        protected override void Compile()
        {
            InternalOutgoingMessage.Initialize(1)
                .AppendString(Key);
        }
    }
}