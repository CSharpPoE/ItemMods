using System;

namespace ItemMods.Model.Item
{
    public sealed class HelmetType
    {
        public static String Parse(String helmet)
        {
            switch (helmet)
            {
                case "Iron Hat":
                case "Cone Helmet":
                case "Barbute Helmet":
                case "Close Helmet":
                case "Gladiator Helmet":
                case "Reaver Helmet":
                case "Siege Helmet":
                case "Samite Helmet":
                case "Ezomyte Burgonet":
                case "Royal Burgonet":
                case "Eternal Burgonet":
                    return "ArmourHelmet";
                case "Leather Cap":
                case "Tricorne":
                case "Leather Hood":
                case "Wolf Pelt":
                case "Hunter Hood":
                case "Noble Tricorne":
                case "Ursine Pelt":
                case "Silken Hood":
                case "Sinner Tricorne":
                case "Lion Pelt":
                    return "EvasionHelmet";
                case "Vine Circlet":
                case "Iron Circlet":
                case "Torture Cage":
                case "Tribal Circlet":
                case "Bone Circlet":
                case "Lunaris Circlet":
                case "Steel Circlet":
                case "Necromancer Circlet":
                case "Solaris Circlet":
                case "Mind Cage":
                case "Hubris Circlet":
                    return "EnergyShieldHelmet";
                case "Battered Helm":
                case "Sallet":
                case "Visored Sallet":
                case "Gilded Sallet":
                case "Secutor Helm":
                case "Fencer Helm":
                case "Lacquered Helmet":
                case "Fluted Bascinet":
                case "Pig-Faced Bascinet":
                case "Nightmare Bascinet":
                    return "ArmourandEvasionHelmet";
                case "Rusted Coif":
                case "Soldier Helmet":
                case "Great Helmet":
                case "Crusader Helmet":
                case "Aventail Helmet":
                case "Zealot Helmet":
                case "Great Crown":
                case "Magistrate Crown":
                case "Prophet Crown":
                case "Praetor Crown":
                case "Bone Helmet":
                    return "ArmourandEnergyShieldHelmet";
                case "Scare Mask":
                case "Plague Mask":
                case "Iron Mask":
                case "Festival Mask":
                case "Golden Mask":
                case "Raven Mask":
                case "Callous Mask":
                case "Regicide Mask":
                case "Harlequin Mask":
                case "Vaal Mask":
                case "Deicide Mask":
                    return "EvasionandEnergyShieldHelmet";
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}