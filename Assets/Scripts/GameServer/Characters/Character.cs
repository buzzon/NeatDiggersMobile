using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NeatDiggers.GameServer.Abilities;

namespace NeatDiggers.GameServer.Characters
{
    public enum CharacterName
    {
        Empty,
        Pandora,
        Kirill,
        Jupiter,
        Sirius
    }

    public class Character
    {
        public CharacterName Name { get; set; }
        public string Title { get; set; }
        public int MaxHealth { get; set; }
        public WeaponType WeaponType { get; set; }
        public List<Ability> Abilities { get; set; }
    }
}
