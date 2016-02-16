﻿using System.Diagnostics;
using MapleCLB.MapleLib;
using MapleCLB.Packets.Send;

namespace MapleCLB.MapleClient.Handlers {
    internal class Handshake : Handler<ServerInfo> {
        internal Handshake(Client client) : base(client) { }

        internal override void Handle(object session, ServerInfo info) {
            Debug.WriteLine("HANDSHAKEEEEE");
            switch (Client.Mode) {
                case ClientMode.CONNECTED:
                    Debug.WriteLine("Validating login for MapleStory v" + info.Version + "." + info.Subversion);
                    SendPacket(Login.Validate(info.Locale, info.Version, short.Parse(info.Subversion)));
                    string authCode = Auth.GetAuth(Client.User, Client.Pass);
                    Client.Mode = ClientMode.LOGIN;
                    Debug.WriteLine(authCode);
                    System.Threading.Thread.Sleep(1000);
                    SendPacket(Login.ClientLogin(Client.Pass, authCode, Client.Hwid1, Client.Hwid2));
                    break;

                case ClientMode.LOGIN:
                    Debug.WriteLine("Logged in!");
                    Client.ccst.Enabled = true;
                    SendPacket(Login.EnterServer(Client.World, Client.UserId, Client.SessionId, Client.Hwid1, Client.Hwid2));
                    Client.Mode = ClientMode.GAME;
                    break;

                case ClientMode.GAME:
                    SendPacket(Login.EnterServer(Client.World, Client.UserId, Client.SessionId, Client.Hwid1, Client.Hwid2));
                    break;

                case ClientMode.CASHSHOP:
                    System.Threading.Thread.Sleep(2000);
                    SendPacket(Login.EnterServer(Client.World, Client.UserId, Client.SessionId, Client.Hwid1, Client.Hwid2));
                    System.Threading.Thread.Sleep(2000);
                    SendPacket(General.ExitCS());
                    Client.Mode = ClientMode.GAME;
                    Client.WriteLog.Report("Left CS!");
                    break;
            }
        }
    }
}
