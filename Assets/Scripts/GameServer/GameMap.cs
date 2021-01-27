using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NeatDiggers.GameServer
{
    public enum GameMapType
    {
        Standart,
        Diagonal,
        Large,
        Custom
    }

    public enum Cell
    {
        None,
        Empty,
        Wall,
        Digging,
    }

    public class GameMap
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public List<Vector> SpawnPoints { get; set; }
        public Vector FlagSpawnPoint { get; set; }
        public Cell[,] Map { get; set; }
    }
}
