using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NeatDiggers.GameServer.Items
{
    public enum ItemName
    {
        Empty,
        Rain,
        StrangeTeleport,
        SoulsExchange,
        SuperJump,
        CatchUp,
        ItemSteal,
        Tutorial,
        FirstAidKit,
        Katana,
        Invul,
        DoubleDamage,
        Bandage,
        BigFirstAidKit,
        Grenade,
        HeavySword,
        PowerShieldItem,
        ArmorBuffItem,
        Hook,
        Laser,
        Bow,
        MachineGun,
        Shield,
        Boots,
        AutomaticRifle,
        SniperRifle,
        Blade,
        Pistol,
        Revolver,
        Sharpen,
        Scope,
        DoubleScope,
        Armor,
        Vest,
        Jacket,
        Sword,
        Knife,
        SuperBoots,
        length
    }

    public enum ItemType
    {
        Passive,
        Active,
        Weapon,
        Armor
    }

    public enum Rarity
    {
        None,
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary
    }

    public class Item
    {
        public ItemName Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ItemType Type { get; set; }
        public Target Target { get; set; }
        public WeaponHanded WeaponHanded { get; set; }
        public WeaponType WeaponType { get; set; }
        public int WeaponDamage { get; set; }
        public int WeaponConsumption { get; set; }
        public int WeaponDistance { get; set; }
        public  int ArmorStrength { get; set; }
        public int ArmorDurability { get; set; }
        public Rarity Rarity { get; set; }
    }
}
