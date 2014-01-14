using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IHI.Server.Habbos.Messenger;
using IHI.Server.Network;

using IHI.Server.Plugins.LibMessenger;

namespace IHI.Server.Plugins.R36StandardStuff.Packets.Senders
{
    public class MMessengerInit : OutgoingMessage
    {
        public IEnumerable<Category> Categories { get; set; }
        public int UnknownA { get; set; }
        public int UnknownB { get; set; }
        public int UnknownC { get; set; }
        public bool UnknownD { get; set; }
        public int MaximumFriends { get; set; }
        public ISet<Befriendable> BefriendablesToExclude { get; set; }

        protected override void Compile()
        {
            Dictionary<Befriendable, ISet<Category>> _friendships = new Dictionary<Befriendable, ISet<Category>>();

            InternalOutgoingMessage.Initialize(12)
                .AppendInt32(UnknownA)
                .AppendInt32(UnknownB)
                .AppendInt32(UnknownC)
                .AppendInt32(Categories.Count() - 1); // -1 because the default category doesn't count

            foreach (Category category in Categories.Where(category => category.Id != 0))
            {
                InternalOutgoingMessage
                    .AppendInt32(category.Id)
                    .AppendString(category.Name);

                foreach (Befriendable friend in category.GetFriends().Except(BefriendablesToExclude))
                {
                    if (!_friendships.ContainsKey(friend))
                        _friendships[friend] = new HashSet<Category>();
                    _friendships[friend].Add(category);
                }
            }

            InternalOutgoingMessage
                .AppendInt32(0); // Friend Count

            foreach (KeyValuePair<Befriendable, ISet<Category>> friend in _friendships)
            {
                foreach (Category category in friend.Value)
                {
                    InternalOutgoingMessage
                        .AppendInt32(friend.Key.Id)
                        .AppendString(friend.Key.DisplayName)
                        .AppendBoolean(true) // TODO: Find out what this does.
                        .AppendBoolean(friend.Key.LoggedIn)
                        .AppendBoolean(friend.Key.LoggedIn)
                        .AppendBoolean(friend.Key.Stalkable)
                        .AppendString(friend.Key.Figure.ToString())
                        .AppendInt32(category.Id)
                        .AppendString(friend.Key.Motto)
                        .AppendString(friend.Key.LastAccess.ToString());
                }
            }

            InternalOutgoingMessage
                .AppendInt32(MaximumFriends)
                .AppendBoolean(UnknownD);
        }
    }
}
