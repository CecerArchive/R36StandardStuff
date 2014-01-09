using System.Collections.Generic;
using IHI.Server.Network;

namespace IHI.Server.Plugins.R36StandardStuff.Packets.Senders
{
    public class MFuseRights : OutgoingMessage
    {
        public ICollection<string> FuseRights
        {
            get;
            set;
        }

        protected override void Compile()
        {
            InternalOutgoingMessage.Initialize(2);

            InternalOutgoingMessage.AppendInt32(FuseRights.Count);
            foreach (string fuseRight in FuseRights)
            {
                InternalOutgoingMessage.AppendString(fuseRight);
            }
        }
    }
}
