using System;

namespace ItemMods.Model.Item
{
    public sealed class GloveType
    {
        public static String Parse(String gloves)
        {
            switch (gloves)
            {
                case "Iron Gauntlets":
                case "Plated Gauntlets":
                case "Bronze Gauntlets":
                case "Steel Gauntlets":
                case "Antique Gauntlets":
                case "Ancient Gauntlets":
                case "Goliath Gauntlets":
                case "Vaal Gauntlets":
                case "Titan Gauntlets":
                case "Spiked Gloves":
                    return "ArmourGloves";
                case "Rawhide Gloves":
                case "Goathide Gloves":
                case "Deerskin Gloves":
                case "Nubuck Gloves":
                case "Eelskin Gloves":
                case "Sharkskin Gloves":
                case "Shagreen Gloves":
                case "Gripped Gloves":
                case "Slink Gloves":
                    return "EvasionGloves";
                case "Wool Gloves":
                case "Velvet Gloves":
                case "Silk Gloves":
                case "Embroidered Gloves":
                case "Satin Gloves":
                case "Samite Gloves":
                case "Conjurer Gloves":
                case "Arcanist Gloves":
                case "Sorcerer Gloves":
                case "Fingerless Silk Gloves":
                    return "EnergyShieldGloves";
                case "Fishscale Gauntlets":
                case "Ironscale Gauntlets":
                case "Bronzescale Gauntlets":
                case "Steelscale Gauntlets":
                case "Serpentscale Gauntlets":
                case "Wyrmscale Gauntlets":
                case "Hydrascale Gauntlets":
                case "Dragonscale Gauntlets":
                    return "ArmourandEvasionGloves";
                case "Chain Gloves":
                case "Ringmail Gloves":
                case "Mesh Gloves":
                case "Riveted Gloves":
                case "Zealot Gloves":
                case "Soldier Gloves":
                case "Legion Gloves":
                case "Crusader Gloves":
                    return "ArmourandEnergyShieldGloves";
                case "Wrapped Mitts":
                case "Strapped Mitts":
                case "Clasped Mitts":
                case "Trapper Mitts":
                case "Ambush Mitts":
                case "Carnal Mitts":
                case "Assassin's Mitts":
                case "Murder Mitts":
                    return "EvasionandEnergyShieldGloves";
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}