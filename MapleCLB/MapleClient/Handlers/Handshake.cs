﻿using System.Diagnostics;
using System.Threading;
using MapleCLB.Packets.Send;
using MapleLib;

namespace MapleCLB.MapleClient.Handlers {
    internal class Handshake : Handler<ServerInfo> {
        internal Handshake(Client client) : base(client) { }

        internal override void Handle(object session, ServerInfo info) {
            Debug.WriteLine("HANDSHAKEEEEE");
            switch (Client.State) {
                case ClientState.DISCONNECTED:
                    Debug.WriteLine("Validating login for MapleStory v" + info.Version + "." + info.Subversion);
                    SendPacket(Login.Validate(info.Locale, info.Version, short.Parse(info.Subversion)));
                    string authCode = Auth.GetAuth(Client.Account);
                    Debug.WriteLine(authCode);
                    Thread.Sleep(1000);
                    SendPacket(Login.ClientLogin(Client.Account, authCode));
                    Client.State = ClientState.LOGIN;
                    break;

                case ClientState.LOGIN:
                    Client.Log.Report("Logged in!");
                    Client.dcst.Enabled = true;
                    SendPacket(Login.EnterServer(Client.Account, Client.UserId, Client.SessionId));
                    Client.State = ClientState.GAME;
                    break;

                case ClientState.GAME:
                    SendPacket(Login.EnterServer(Client.Account, Client.UserId, Client.SessionId));
                    break;

                case ClientState.CASHSHOP:
                    Thread.Sleep(2000);
                    SendPacket(Login.EnterServer(Client.Account, Client.UserId, Client.SessionId));
                    Thread.Sleep(2000);
                    SendPacket(General.ExitCS());
                    Debug.WriteLine("Left Cash Shop!");
                    Client.State = ClientState.GAME;
                    break;
            }
        }
    }
}
