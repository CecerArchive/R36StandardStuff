using System.Globalization;
using IHI.Server.Network;
using IHI.Server.Rooms.Figure;

namespace IHI.Server.Plugins.R36StandardStuff.Packets
{
    public class MAccountDetails : OutgoingMessage
    {
        public int ConnectionId
        {
            get;
            set;
        }
        public string Username
        {
            get;
            set;
        }
        public string Motto
        {
            get;
            set;
        }
        public HabboFigure Figure
        {
            get;
            set;
        }
        public string UnknownA
        {
            get;
            set;
        }
        public int UnknownB
        {
            get;
            set;
        }
        public int UnknownC
        {
            get;
            set;
        }

        public int UnknownD
        {
            get; 
            set;
        }

        protected override void Compile()
        {
            InternalOutgoingMessage.Initialize(5)
                .AppendString(ConnectionId.ToString(CultureInfo.InvariantCulture))
                .AppendString(Username)
                .AppendString(Figure.ToString())
                .AppendString(Figure.GenderChar.ToString(CultureInfo.InvariantCulture))
                .AppendString(Motto)
                .AppendString(UnknownA)
                .AppendInt32(UnknownB)
                .AppendString(Figure.FormattedSwimFigure)
                .AppendInt32(UnknownC)
                .AppendInt32(UnknownD);
        }
    }
}