using System;
using System.Net;
using IHI.Server.Events;
using IHI.Server.Install;
using IHI.Server.Network;
using IHI.Server.Network.GameSockets;
using IHI.Server.Network.Messages;
using IHI.Server.Plugins.R36StandardStuff.Packets;
using IHI.Server.Plugins.R36StandardStuff.Packets.Senders;

namespace IHI.Server.Plugins.R36StandardStuff
{
    public class R36StandardStuff : Plugin
    {
        public override string Id
        {
            get { return "cecer:r36standardstuff"; }
        }

        public override string Name
        {
            get
            {
                return "R36StandardStuff";
            }
        }

        internal static EventFirer EventFirer
        {
            get;
            private set;
        }

        public R36StandardStuff()
        {
            CoreManager.ServerCore.Installer
                    .AddStepIfMissing(new Step("r36:host", typeof(IPAddress), "R36 Bind IP",
                        "The IP address to listen for R36 connections on", "127.0.0.1")
                        .AddExample("192.168.1.70")
                        .AddExample("123.123.123.123")
                        .AddExample("127.0.0.1"))
                    .AddStepIfMissing(new Step("r36:port", typeof(ushort), "R36 Bind Port",
                        "The port to listen for R36 connections on.", "14478")
                        .AddExample("14478")
                        .AddExample("30000"));
        }

        public GameSocketManager GameSocketManager
        {
            get;
            private set;
        }

        /// <summary>
        ///   Called when the plugin is started.
        /// </summary>
        public override void Start(EventFirer eventFirer)
        {
            EventFirer = eventFirer;

            IPAddress ipAddress = CoreManager.ServerCore.Config.GetValue("r36:bindip", IPAddress.Any).Value;
            ushort port = CoreManager.ServerCore.Config.GetValue<ushort>("r36:bindport", 14478).Value;

            GameSocketProtocol r36Protocol = new GameSocketProtocol(new ClassicGameSocketReader(), 36);

            GameSocketManager = CoreManager.ServerCore.NewGameSocketManager("R36", ipAddress, port, r36Protocol);
            GameSocketManager.Start();


            CoreManager.ServerCore.EventManager.StrongBind<GameSocketEventArgs>("incoming_game_connection:after", AcceptConnection);
            CoreManager.ServerCore.EventManager.WeakBind<GameSocketEventArgs>("incoming_game_connection:after", RegisterLoginHandlers);
            CoreManager.ServerCore.EventManager.WeakBind<HabboEventArgs>("habbo_login:after", ConfirmLogin);
            CoreManager.ServerCore.EventManager.WeakBind<HabboEventArgs>("habbo_login:after", SendFuserights);
        }

        private void AcceptConnection(GameSocketEventArgs eventArgs)
        {
            if (eventArgs.Socket.GameSocketManager != GameSocketManager)
                return;

            new MAcceptConnection().Send(eventArgs.Socket);
        }

        private void ConfirmLogin(HabboEventArgs eventArgs)
        {
            new MAuthenticationOkay().Send(eventArgs.Habbo);
        }

        private void SendFuserights(HabboEventArgs eventArgs)
        {
            FuserightEventArgs fuserightEventArgsInstance = new FuserightEventArgs(eventArgs.Habbo);
            EventFirer.Fire("fuseright_request:before", eventArgs);
            EventFirer.Fire("fuseright_request:after", eventArgs);

            new MFuseRights
            {
                FuseRights = fuserightEventArgsInstance.GetFuserights()
            }.Send(eventArgs.Habbo);
        }

        private void RegisterLoginHandlers(GameSocketEventArgs eventArgs)
        {
            if (eventArgs.Socket.GameSocketManager != GameSocketManager)
                return;

            eventArgs.Socket.PacketHandlers[196, GameSocketMessageHandlerPriority.DefaultAction] += Handlers.ProcessPong;
            eventArgs.Socket.PacketHandlers[206, GameSocketMessageHandlerPriority.DefaultAction] += Handlers.ProcessEncryptionRequest;
            eventArgs.Socket.PacketHandlers[204, GameSocketMessageHandlerPriority.DefaultAction] += Handlers.ProcessSSOTicket;
            eventArgs.Socket.PacketHandlers[2002, GameSocketMessageHandlerPriority.DefaultAction] += Handlers.ProcessSessionRequest;

            eventArgs.Socket.PacketHandlers[7, GameSocketMessageHandlerPriority.DefaultAction] += Handlers.ProcessAccountDetailsRequest;
            eventArgs.Socket.PacketHandlers[8, GameSocketMessageHandlerPriority.DefaultAction] += Handlers.ProcessBalanceRequest;
            eventArgs.Socket.PacketHandlers[49, GameSocketMessageHandlerPriority.DefaultAction] += Handlers.ProcessCurrentDateRequest;
            eventArgs.Socket.PacketHandlers[157, GameSocketMessageHandlerPriority.DefaultAction] += Handlers.ProcessBadgeStatusRequest;
            eventArgs.Socket.PacketHandlers[228, GameSocketMessageHandlerPriority.DefaultAction] += Handlers.ProcessGroupStatusInit;
        }
    }
}