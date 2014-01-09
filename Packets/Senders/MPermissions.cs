#region Usings

using System.Collections.Generic;
using IHI.Server.Network;

#endregion

namespace IHI.Server.Network.Messages
{
    public class MPermissions : OutgoingMessage
    {
        public IEnumerable<string> FuseRights { get; set; }

        protected override void Compile()
        {
            InternalOutgoingMessage.Initialize(2);
            foreach (string fuseRight in FuseRights)
            {
                InternalOutgoingMessage.AppendString(fuseRight);
            }
        }
    }
}