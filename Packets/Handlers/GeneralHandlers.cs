using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using IHI.Server.Events;
using IHI.Server.Habbos;
using IHI.Server.Habbos.Messenger;
using IHI.Server.Network;
using IHI.Server.Network.Messages;
using IHI.Server.Plugins.LibMessenger;
using IHI.Server.Plugins.R36StandardStuff.Packets;
using IHI.Server.Plugins.R36StandardStuff.Packets.Senders;
using IHI.Server.Useful;

namespace IHI.Server.Plugins.R36StandardStuff
{
    public static partial class Handlers
    {
        internal static void ProcessPong(Habbo sender, IncomingMessage message)
        {
            HabboEventArgs eventArgs = new HabboEventArgs(sender);
            R36StandardStuff.EventFirer.Fire("habbo_pong:before", eventArgs);
            if (!eventArgs.IsCancelled)
                R36StandardStuff.EventFirer.Fire("habbo_pong:after", eventArgs);
        }

        internal static void ProcessEncryptionRequest(Habbo sender, IncomingMessage message)
        {
            new MSetupEncryption
                {
                    UnknownA = false
                }.Send(sender);
        }

        internal static void ProcessSessionRequest(Habbo sender, IncomingMessage message)
        {
            new MSessionParams
                {
                    A = 9,
                    B = 0,
                    C = 0,
                    D = 1,
                    E = 1,
                    F = 3,
                    G = 0,
                    H = 2,
                    I = 1,
                    J = 4,
                    K = 0,
                    L = 5,
                    DateFormat = "dd-MM-yyyy",
                    M = "",
                    N = 7,
                    O = false,
                    P = 8,
                    URL = "http://null",
                    Q = "",
                    R = 9,
                    S = false
                }.Send(sender);
        }

        internal static void ProcessSSOTicket(Habbo sender, IncomingMessage message)
        {
            ClassicIncomingMessage classicMessage = (ClassicIncomingMessage) message;

            Habbo loggedInHabbo = CoreManager.ServerCore.HabboDistributor.GetHabboFromSSOTicket(classicMessage.PopPrefixedString());

            if (loggedInHabbo == null)
            {
                new MConnectionClosed
                    {
                        Reason = ConnectionClosedReason.InvalidSSOTicket
                    }.Send(sender);

                // TODO: Is delay needed?

                sender.Socket.Disconnect("Invalid SSO Ticket!"); // Invalid SSO Ticket
            }
            else
            {
                HabboEventArgs eventArgs = new HabboEventArgs(loggedInHabbo);
                R36StandardStuff.EventFirer.Fire("habbo_login:before", eventArgs);
                
                if (eventArgs.IsCancelled)
                {
                    if (sender.Socket != null)
                        sender.Socket.Disconnect(eventArgs.CancelReason);
                    return;
                }

                // If this Habbo is already connected...
                if (loggedInHabbo.LoggedIn)
                {
                    // Disconnect them.
                    new MConnectionClosed
                        {
                            Reason = ConnectionClosedReason.ConcurrentLogin
                        }.Send(loggedInHabbo);
                    loggedInHabbo.Socket.Disconnect("Concurrent Login!");
                }

                sender.Socket.LinkHabbo(loggedInHabbo);
                loggedInHabbo.LoggedIn = true;
                loggedInHabbo.LastAccess = DateTime.Now;

                R36StandardStuff.EventFirer.Fire("habbo_login:after", eventArgs);
            }
        }

        internal static void ProcessCurrentDateRequest(Habbo sender, IncomingMessage message)
        {
            new MCurrentDate
            {
                Date = DateTime.Today
            }.Send(sender);
        }

        internal static void ProcessBalanceRequest(Habbo sender, IncomingMessage message)
        {
            new MCreditBalance(sender).Send(sender);
        }

        internal static void ProcessGroupStatusInit(Habbo sender, IncomingMessage message)
        {
            // Load group stuff
        }

        internal static void ProcessBadgeStatusRequest(Habbo sender, IncomingMessage message)
        {
            // Load badge stuff
        }

        internal static void ProcessAccountDetailsRequest(Habbo sender, IncomingMessage message)
        {
            new MAccountDetails
            {
                ConnectionId = 0,
                Username = sender.Username, // Should this be DisplayName
                Motto = sender.Motto,
                Figure = sender.Figure,
                UnknownA = "",
                UnknownB = 12,
                UnknownC = 0,
                UnknownD = 1
            }.Send(sender);
        }

        internal static void ProcessMessengerInit(Habbo sender, IncomingMessage message)
        {
            MessengerManager manager = CoreManager.ServerCore.GetMessengerManager();
            new MMessengerInit
            {
                BefriendablesToExclude = new HashSet<Befriendable>
                {
                    manager.GetBefriendable(sender)
                },
                Categories = manager.GetCategories(sender),
                MaximumFriends = 200, // TODO: Make this controllable by plugins.
                UnknownA = 10,
                UnknownB = 20,
                UnknownC = 30,
                UnknownD = false
            }.Send(sender);
        }
    }
}
