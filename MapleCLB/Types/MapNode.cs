﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace MapleCLB.Types {
    public sealed class Portal {
        public short X { get; set; }
        public short Y { get; set; }
        public string Name { get; set; }
    }

    public class MapNode {
        public ReadOnlyDictionary<int, Portal> Portals;
        public ReadOnlyDictionary<int, int> Choice { get; set; }
        public int Id { get; }

        public MapNode(int id, ReadOnlyDictionary<int, Portal> portals,
                       ReadOnlyDictionary<int, int> choice) {
            Id = id;
            Portals = portals;
            Choice = choice;
        }

        public MapNode(BinaryReader reader) {
            Id = reader.ReadInt32();

            int count = reader.ReadInt32();
            Dictionary<int, Portal> portals = new Dictionary<int, Portal>(count);
            for (int i = 0; i < count; i++) {
                int key = reader.ReadInt32();
                portals[key] = new Portal {
                    X = reader.ReadInt16(),
                    Y = reader.ReadInt16(),
                    Name = reader.ReadString()
                };
            }

            count = reader.ReadInt32();
            Dictionary<int, int> choice = new Dictionary<int, int>(count);
            for (int i = 0; i < count; i++) {
                int key = reader.ReadInt32();
                int value = reader.ReadInt32();
                choice[key] = value;
            }

            // Assign Dictionaries
            Portals = new ReadOnlyDictionary<int, Portal>(portals);
            Choice = new ReadOnlyDictionary<int, int>(choice);
        }

        public override bool Equals(object o) {
            var map = o as MapNode;
            return Id == map?.Id;
        }

        public override int GetHashCode() {
            return Id.GetHashCode();
        }
    }
}
