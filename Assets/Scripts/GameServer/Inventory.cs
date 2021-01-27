using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NeatDiggers.GameServer.Items;

namespace NeatDiggers.GameServer
{
    public class Inventory
    {
        public Item LeftWeapon { get; set; }
        public Item RightWeapon { get; set; }
        public Item Armor { get; set; }
        public List<Item> Items { get; set; }
        public int Drop { get; set; }
    }
}
