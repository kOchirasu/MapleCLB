﻿using System;
using System.Net;
using System.Windows.Forms;
using MapleCLB.Forms;
using MapleCLB.MapleLib.Crypto;

namespace MapleCLB {
    internal static class Program {
        public static readonly IPAddress LoginIp = IPAddress.Parse("8.31.99.143");
        public static readonly short LoginPort = 8484;

        private static readonly byte[] UserKey = //171.1
        {
            0xF1, 0x00, 0x00, 0x00,
            0x02, 0x00, 0x00, 0x00,
            0x15, 0x00, 0x00, 0x00,
            0xE2, 0x00, 0x00, 0x00,
            0x68, 0x00, 0x00, 0x00,
            0xA6, 0x00, 0x00, 0x00,
            0xF2, 0x00, 0x00, 0x00,
            0xDE, 0x00, 0x00, 0x00
        };

        public static readonly AesCipher AesCipher = new AesCipher(UserKey);
        
        private static MainForm gui;

        [STAThread]
        private static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            gui = new MainForm();
            Application.Run(gui);
        }
    }
}
