using IHI.Server.Network;

namespace IHI.Server.Plugins.R36StandardStuff.Packets
{
    public class MVolumeLevel : OutgoingMessage
    {
        public int Volume { get; set; }

        protected override void Compile()
        {
            InternalOutgoingMessage.Initialize(308)
                .AppendInt32(Volume);
        }
    }
}