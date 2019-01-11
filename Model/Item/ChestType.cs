using System;

namespace ItemMods.Model.Item
{
    public sealed class ChestType
    {
        public static String Parse(String chest)
        {
            switch (chest)
            {
                case "Plate Vest":
                case "Chestplate":
                case "Copper Plate":
                case "War Plate":
                case "Full Plate":
                case "Arena Plate":
                case "Lordly Plate":
                case "Bronze Plate":
                case "Battle Plate":
                case "Sun Plate":
                case "Colosseum Plate":
                case "Majestic Plate":
                case "Golden Plate":
                case "Crusader Plate":
                case "Astral Plate":
                case "Gladiator Plate":
                case "Glorious Plate":
                    return "ArmourChest";
                case "Shabby Jerkin":
                case "Strapped Leather":
                case "Buckskin Tunic":
                case "Wild Leather":
                case "Full Leather":
                case "Sun Leather":
                case "Thief's Garb":
                case "Eelskin Tunic":
                case "Frontier Leather":
                case "Glorious Leather":
                case "Coronal Leather":
                case "Cutthroat's Garb":
                case "Sharkskin Tunic":
                case "Destiny Leather":
                case "Exquisite Leather":
                case "Zodiac Leather":
                case "Assassin's Garb":
                    return "EvasionChest";
                case "Simple Robe":
                case "Silken Vest":
                case "Scholar's Robe":
                case "Silken Garb":
                case "Mage's Vestment":
                case "Silk Robe":
                case "Cabalist Regalia":
                case "Sage's Robe":
                case "Silken Wrap":
                case "Conjurer's Vestment":
                case "Spidersilk Robe":
                case "Destroyer Regalia":
                case "Savant's Robe":
                case "Necromancer Silks":
                case "Occultist's Vestment":
                case "Widowsilk Robe":
                case "Vaal Regalia":
                    return "EnergyShieldChest";
                case "Scale Vest":
                case "Light Brigandine":
                case "Scale Doublet":
                case "Infantry Brigandine":
                case "Full Scale Armour":
                case "Soldier's Brigandine":
                case "Field Lamellar":
                case "Wyrmscale Doublet":
                case "Hussar Brigandine":
                case "Full Wyrmscale":
                case "Commander's Brigandine":
                case "Battle Lamellar":
                case "Desert Brigandine":
                case "Full Dragonscale":
                case "General's Brigandine":
                case "Triumphant Lamellar":
                    return "ArmourandEvasionChest";
                case "Chainmail Vest":
                case "Chainmail Tunic":
                case "Ringmail Coat":
                case "Chainmail Doublet":
                case "Full Ringmail":
                case "Full Chainmail":
                case "Holy Chainmail":
                case "Latticed Ringmail":
                case "Crusader Chainmail":
                case "Ornate Ringmail":
                case "Chain Hauberk":
                case "Devout Chainmail":
                case "Loricated Ringmail":
                case "Conquest Chainmail":
                case "Elegant Ringmail":
                case "Saint's Hauberk":
                case "Saintly Chainmail":
                    return "ArmourandEnergyShieldChest";
                case "Padded Vest":
                case "Oiled Vest":
                case "Padded Jacket":
                case "Oiled Coat":
                case "Scarlet Raiment":
                case "Waxed Garb":
                case "Bone Armour":
                case "Quilted Jacket":
                case "Sleek Coat":
                case "Crimson Raiment":
                case "Lacquered Garb":
                case "Crypt Armour":
                case "Sentinel Jacket":
                case "Varnished Coat":
                case "Blood Raiment":
                case "Sadist Garb":
                case "Carnal Armour":
                    return "EvasionandEnergyShieldChest";
                case "Sacrificial Garb":
                    return "SacrificialGarb";
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}