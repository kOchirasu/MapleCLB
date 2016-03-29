﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using MapleCLB.Resources;
using MapleCLB.Types;

namespace MapleCLB.MapleClient.Functions {
    internal class MapRusher {
        internal static List<Portal> Pathfind(int src, int dst) {
            List<Portal> directions = new List<Portal>();

            // Already on destination map
            if (src == dst) {
                return directions;
            }

            // Can move to map with 1 portal
            ReadOnlyDictionary<int, Portal> curPortals = MapData.Nodes[src].Portals;
            if (curPortals.ContainsKey(dst)) {
                directions.Add(curPortals[dst]);
                return directions;
            }

            // Cannot reach destination
            if (!MapData.Nodes[src].Choice.ContainsKey(dst)) {
                return null;
            }

            // Find path to destination
            int curr = src;
            while (curr != dst) {
                int next = MapData.Nodes[curr].Choice[dst];
                directions.Add(MapData.Nodes[curr].Portals[next]);
                curr = next;
            }

            return directions;
        }

        internal static List<int> Reachable(int src) {
            if (!MapData.Nodes.ContainsKey(src)) { // No dst in map
                return new List<int>();
            }
            return new List<int>(MapData.Nodes[src].Choice.Keys);
        }
    }
}
