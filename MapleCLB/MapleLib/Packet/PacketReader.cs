﻿using System;
using System.Text;

namespace MapleCLB.MapleLib.Packet {
    public class PacketReader {
        public byte[] Buffer { get; private set; }
        public int Position { get; private set; }

        public int Available {
            get { return Buffer.Length - Position; }
        }

        public PacketReader(byte[] packet, int skip = 0) {
            Buffer = packet;
            Position = skip;
        }

        private void CheckLength(int length) {
            if (Position + length > Buffer.Length || length < 0) {
                throw new IndexOutOfRangeException("Not enough space in packet: " + ToString() + 
                    "\n" + Position + length + " > " + Buffer.Length + " OR " + length + " < 0");
            }
        }

        public byte ReadByte() {
            CheckLength(1);
            return Buffer[Position++];
        }

        public bool ReadBool() {
            return ReadByte() == 1;
        }

        public byte[] ReadBytes(int count) {
            CheckLength(count);
            byte[] bytes = new byte[count];
            System.Buffer.BlockCopy(Buffer, Position, bytes, 0, count);
            Position += count;
            return bytes;
        }

        public unsafe short ReadShort() {
            CheckLength(2);
            fixed (byte* ptr = Buffer) {
                short value = *(short*)(ptr + Position);
                Position += 2;
                return value;
            }
        }

        public unsafe short ReadShortBigEndian() {
            CheckLength(2);
            fixed (byte* ptr = Buffer) {
                short value = (short)(*(ptr + Position) << 8 | *(ptr + Position + 1));
                Position += 2;
                return value;
            }
        }

        public unsafe int ReadInt() {
            CheckLength(4);
            fixed (byte* ptr = Buffer) {
                int value = *(int*)(ptr + Position);
                Position += 4;
                return value;
            }
        }

        public unsafe uint ReadUInt() {
            CheckLength(4);
            fixed (byte* ptr = Buffer) {
                uint value = *(uint*)(ptr + Position);
                Position += 4;
                return value;
            }
        }

        public unsafe int ReadIntBigEndian() {
            CheckLength(4);
            fixed (byte* ptr = Buffer) {
                int value = *(ptr + Position) << 24 | *(ptr + Position + 1) << 16 | *(ptr + Position + 2) << 8 | *(ptr + Position + 3);
                Position += 4;
                return value;
            }
        }

        public unsafe long ReadLong() {
            CheckLength(8);
            fixed (byte* ptr = Buffer) {
                long value = *(long*)(ptr + Position);
                Position += 8;
                return value;
            }
        }

        public string ReadString(int count) {
            byte[] bytes = ReadBytes(count);
            return Encoding.UTF8.GetString(bytes);
        }

        public string ReadUnicodeString() {
            short count = ReadShort();
            byte[] bytes = ReadBytes(count * 2);
            return Encoding.Unicode.GetString(bytes);
        }

        public string ReadMapleString() {
            short count = ReadShort();
            return ReadString(count);
        }

        public string ReadHexString(int count) {
            return HexEncoding.ToHexString(ReadBytes(count));
        }

        public void Skip(int count) {
            CheckLength(count);
            Position += count;
        }

        public void Next(byte b) {
            int pos = Array.IndexOf(Buffer, b, Position);
            Skip(pos - Position + 1);
        }

        public byte[] ToArray() {
            byte[] copy = new byte[Buffer.Length];
            System.Buffer.BlockCopy(Buffer, 0, copy, 0, Buffer.Length);
            return copy;
        }

        public override string ToString() {
            return HexEncoding.ToHexString(Buffer);
        }
    }
}
