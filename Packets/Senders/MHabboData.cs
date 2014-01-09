#region Usings

using System.Globalization;
using IHI.Server.Habbos;
using IHI.Server.Network;
using IHI.Server.Rooms.Figure;

#endregion

namespace IHI.Server.Network.Messages
{
    public class MHabboData : OutgoingMessage
    {
        public HabboFigure Figure { get; set; }
        public int HabboId { get; set; }
        public string Motto { get; set; }
        public string UnknownA { get; set; }
        public string Username { get; set; }

        protected override void Compile()
        {
            InternalOutgoingMessage.Initialize(5)
                .AppendString(HabboId.ToString(CultureInfo.InvariantCulture))
                .AppendString(Username) // TODO: Should this be display name?
                .AppendString(Figure.ToString())
                .AppendString(Figure.GenderChar.ToString(CultureInfo.InvariantCulture))
                .AppendString(Motto)
                .AppendString(UnknownA)
                .AppendInt32(12) // TODO: Find out what this does.
                .AppendString(Figure.FormattedSwimFigure)
                .AppendBoolean(false)
                .AppendBoolean(true);
        }
    }
}