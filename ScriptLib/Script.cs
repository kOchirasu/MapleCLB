﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using MapleLib.Packet;
using SharedTools;

namespace ScriptLib {
    public abstract class Script<TClient> where TClient : IScriptClient {
        private readonly List<ushort> headers = new List<ushort>();

        private int refs;
        protected TClient client;
        protected bool running;

        protected Script(TClient client) {
            this.client = client;
        }

        #region Script Commands
        public bool Start() {
            return Start(Run);
        }

        public void Stop() {
            Interlocked.Decrement(ref refs);
            if (refs == 0) {
                headers.ForEach(d => client.RemoveScriptRecv(d));
                headers.Clear();
                running = false;
            }
        }

        private void Run() {
            try {
                Init();
            } catch (InvalidOperationException ex) {
                Debug.WriteLine($"Error running {GetType().Name}.  Terminated.");
                Debug.WriteLine(ex.ToString());
            }
        }
        #endregion

        protected bool Start(Action run) {
            Interlocked.Increment(ref refs);
            if (running) {
                return false;
            }
            running = true;
            Debug.WriteLine($"[SCRIPT] Started {GetType().Name}.");
            run();
            return true;
        }

        #region Scripting Functions
        protected void SendPacket(byte[] packet) {
            client.SendPacket(packet);
        }

        protected void SendPacket(PacketWriter w) {
            client.SendPacket(w);
        }

        protected void RegisterRecv(ushort header, EventHandler<PacketReader> handler) {
            Precondition.Check<InvalidOperationException>(client.AddScriptRecv(header, handler),
                $"Failed to register header {header:X4}.");
            headers.Add(header);
        }

        protected void UnregisterRecv(ushort header) {
            headers.Remove(header);
            client.RemoveScriptRecv(header);
        }

        protected abstract void Init();
        #endregion
    }
}
