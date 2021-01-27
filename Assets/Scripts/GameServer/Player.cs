using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NeatDiggers.GameServer.Characters;

namespace NeatDiggers.GameServer
{
    public class Player
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Devices { get; set; }
        public bool IsReady { get; set; }
        public bool IsTurn { get; set; }
        public bool WithFlag { get; set; }
        public Vector SpawnPoint { get; set; }
        public Vector Position { get; set; }
        public Character Character { get; set; }
        public Inventory Inventory { get; set; }
        public int Level { get; set; }
        public int Speed { get; set; }
        public int Health { get; set; }
        public int MeleeDamage { get; set; }
        public int RangedDamage { get; set; }
        public double MultiplyDamage { get; set; }
        public int MeleeDistance { get; set; }
        public int RangedDistance { get; set; }
        public int DigPower { get; set; }
        public int Hands { get; set; }
        public int Armor { get; set; }
        public int Score { get; set; }
        public List<Effect> Effects { get; set; }
    }
}
