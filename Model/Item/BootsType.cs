using System;

namespace ItemMods.Model.Item
{
    public sealed class BootsType
    {
        public static String Parse(String boots)
        {
            switch (boots)
            {
                case "Iron Greaves":
                case "Steel Greaves":
                case "Plated Greaves":
                case "Reinforced Greaves":
                case "Antique Greaves":
                case "Ancient Greaves":
                case "Goliath Greaves":
                case "Vaal Greaves":
                case "Titan Greaves":
                    return "ArmourBoots";
                case "Rawhide Boots":
                case "Goathide Boots":
                case "Deerskin Boots":
                case "Nubuck Boots":
                case "Eelskin Boots":
                case "Sharkskin Boots":
                case "Shagreen Boots":
                case "Stealth Boots":
                case "Slink Boots":
                    return "EvasionBoots";
                case "Wool Shoes":
                case "Velvet Slippers":
                case "Silk Slippers":
                case "Scholar Boots":
                case "Satin Slippers":
                case "Samite Slippers":
                case "Conjurer Boots":
                case "Arcanist Slippers":
                case "Sorcerer Boots":
                    return "EnergyShieldBoots";
                case "Leatherscale Boots":
                case "Ironscale Boots":
                case "Bronzescale Boots":
                case "Steelscale Boots":
                case "Serpentscale Boots":
                case "Wyrmscale Boots":
                case "Hydrascale Boots":
                case "Dragonscale Boots":
                    return "ArmourandEvasionBoots";
                case "Chain Boots":
                case "Ringmail Boots":
                case "Mesh Boots":
                case "Riveted Boots":
                case "Zealot Boots":
                case "Soldier Boots":
                case "Legion Boots":
                case "Crusader Boots":
                    return "ArmourandEnergyShieldBoots";
                case "Wrapped Boots":
                case "Strapped Boots":
                case "Clasped Boots":
                case "Shackled Boots":
                case "Trapper Boots":
                case "Ambush Boots":
                case "Carnal Boots":
                case "Assassin's Boots":
                case "Murder Boots":
                    return "EvasionandEnergyShieldBoots";
                default: throw new ArgumentOutOfRangeException();

                //TODO HANDLE Two-Stoned boots, atm it has the same name, but differenciate on the implicit. 
            }
        }
    }
}