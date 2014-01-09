#region Usings

using System.Globalization;
using IHI.Server.Habbos;

#endregion

namespace IHI.Server.Network.Messages
{
    public class MCreditBalance : OutgoingMessage
    {
        /// <summary>
        ///   Constructs a new instance of MCreditBalance.
        /// </summary>
        /// <param name = "habbo"></param>
        public MCreditBalance()
        {
        }
        /// <summary>
        ///   Constructs a new instance of MCreditBalance and sets the balance to that of a given Habbo.
        /// </summary>
        /// <param name = "habbo"></param>
        public MCreditBalance(Habbo habbo)
        {
            Balance = habbo.Credits;
        }

        public int Balance { get; set; }

        protected override void Compile()
        {
            InternalOutgoingMessage.Initialize(6)
                .AppendString(Balance.ToString(CultureInfo.InvariantCulture));
        }
    }
}