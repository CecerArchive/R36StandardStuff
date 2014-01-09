#region Usings

using System;

#endregion

namespace IHI.Server.Network.Messages
{
    public class MCurrentDate : OutgoingMessage
    {
        /// <summary>
        ///   Constructs a new instance of MCreditBalance.
        /// </summary>
        public MCurrentDate()
        {
            Date = DateTime.Today;
        }

        public DateTime Date { get; set; }

        protected override void Compile()
        {
            InternalOutgoingMessage.Initialize(163)
                .AppendString(Date.ToShortDateString());
        }
    }
}