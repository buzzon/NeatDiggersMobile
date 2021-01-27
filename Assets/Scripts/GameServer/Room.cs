using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NeatDiggers.GameServer.Characters;
using NeatDiggers.GameServer.Items;

namespace NeatDiggers.GameServer
{
    public class Room
    {
        public bool IsStarted { get; set; }
        public string Code { get; set; }
        public List<Player> Players { get; set; }
        public List<string> Spectators { get; set; }
        public int PlayerTurn { get; set; }
        public Vector FlagPosition { get; set; }
        public bool FlagOnTheGround { get; set; }
        public int Round { get; set; }
        public Player Winner { get; set; }
    }
}
