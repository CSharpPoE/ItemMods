using System;

namespace ItemMods.Model.Item
{
    public sealed class ShieldType
    {
        public static String Parse(String shield)
        {
            switch (shield)
            {
                case "Splintered Tower Shield":
                case "Corroded Tower Shield":
                case "Rawhide Tower Shield":
                case "Cedar Tower Shield":
                case "Copper Tower Shield":
                case "Reinforced Tower Shield":
                case "Painted Tower Shield":
                case "Buckskin Tower Shield":
                case "Mahogany Tower Shield":
                case "Bronze Tower Shield":
                case "Girded Tower Shield":
                case "Crested Tower Shield":
                case "Shagreen Tower Shield":
                case "Ebony Tower Shield":
                case "Ezomyte Tower Shield":
                case "Colossal Tower Shield":
                case "Pinnacle Tower Shield":
                    return "ArmourShield";
                case "Goathide Buckler":
                case "Pine Buckler":
                case "Painted Buckler":
                case "Hammered Buckler":
                case "War Buckler":
                case "Gilded Buckler":
                case "Oak Buckler":
                case "Enameled Buckler":
                case "Corrugated Buckler":
                case "Battle Buckler":
                case "Golden Buckler":
                case "Ironwood Buckler":
                case "Lacquered Buckler":
                case "Vaal Buckler":
                case "Crusader Buckler":
                case "Imperial Buckler":
                    return "EvasionShield";
                case "Twig Spirit Shield":
                case "Yew Spirit Shield":
                case "Bone Spirit Shield":
                case "Tarnished Spirit Shield":
                case "Jingling Spirit Shield":
                case "Brass Spirit Shield":
                case "Walnut Spirit Shield":
                case "Ivory Spirit Shield":
                case "Ancient Spirit Shield":
                case "Chiming Spirit Shield":
                case "Thorium Spirit Shield":
                case "Lacewood Spirit Shield":
                case "Fossilised Spirit Shield":
                case "Vaal Spirit Shield":
                case "Harmonic Spirit Shield":
                case "Titanium Spirit Shield":
                    return "EnergyShieldShield";
                case "Rotted Round Shield":
                case "Fir Round Shield":
                case "Studded Round Shield":
                case "Scarlet Round Shield":
                case "Splendid Round Shield":
                case "Maple Round Shield":
                case "Spiked Round Shield":
                case "Crimson Round Shield":
                case "Baroque Round Shield":
                case "Teak Round Shield":
                case "Spiny Round Shield":
                case "Cardinal Round Shield":
                case "Elegant Round Shield":
                    return "ArmourandEvasionShield";
                case "Plank Kite Shield":
                case "Linden Kite Shield":
                case "Reinforced Kite Shield":
                case "Layered Kite Shield":
                case "Ceremonial Kite Shield":
                case "Etched Kite Shield":
                case "Steel Kite Shield":
                case "Laminated Kite Shield":
                case "Angelic Kite Shield":
                case "Branded Kite Shield":
                case "Champion Kite Shield":
                case "Mosaic Kite Shield":
                case "Archon Kite Shield":
                    return "ArmourandEnergyShieldShield";
                case "Spiked Bundle":
                case "Driftwood Spiked Shield":
                case "Alloyed Spiked Shield":
                case "Burnished Spiked Shield":
                case "Redwood Spiked Shield":
                case "Compound Spiked Shield":
                case "Polished Spiked Shield":
                case "Sovereign Spiked Shield":
                case "Alder Spiked Shield":
                case "Ezomyte Spiked Shield":
                case "Mirrored Spiked Shield":
                case "Supreme Spiked Shield":
                    return "EvasionandEnergyShieldShield";
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}