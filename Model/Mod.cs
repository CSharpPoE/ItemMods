using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace ItemMods.Model
{
    public class Mod : ICloneable
    {
        private const String JSON_PATH = "c://tmp//poe//json";
        private static readonly JsonConverter converter = new ModConverter();
        private static Dictionary<String, Dictionary<int, Mod>> dict;

        public static void Init()
        {
            dict = new Dictionary<string, Dictionary<int, Mod>>(StringComparer.InvariantCultureIgnoreCase);

            foreach (var file in Directory.GetFiles(JSON_PATH))
            {
                var innerDict = new Dictionary<int, Mod>();
                dict.Add(Path.GetFileName(file).Replace(".json", ""), innerDict);

                foreach (var mods in JsonConvert.DeserializeObject<IEnumerable<Mod>>(
                    File.ReadAllText(file), converter).ToList())
                {
                    if (innerDict.TryGetValue(mods.GetHash(), out var mod))
                    {
                        innerDict[mods.GetHash()] = mod.Combine(mods);
                    }
                    else
                    {
                        innerDict.Add(mods.GetHash(), mods);
                    }
                }
            }
        }

        public string name { get; set; }
        private int _hash;

        public int GetHash()

        {
            if (_hash == 0) _hash = name.Hash();
            return _hash;
        }

        //public int hash => name.Hash();
        public IEnumerable<Tier> tier { get; set; }

        public static Mod Parse(String parsedCategory, String modText)
        {
            if (String.IsNullOrEmpty(parsedCategory))
                throw new Exception($"Unable to parse category: {parsedCategory}");
            if (String.IsNullOrEmpty(modText)) throw new Exception($"Unable to parse mod: {modText}");

            try
            {
                if (!dict.TryGetValue(parsedCategory, out var innerDict)) return default;
                if (!innerDict.TryGetValue(modText.Hash(), out var mod)) return default;
                return mod.Copy(ParseValue(modText, mod));
            }
            catch
            {
                throw new Exception($"Can not parse mod: {modText}");
            }
        }

        public static IEnumerable<Mod> Parse(String category, IEnumerable<String> explicitMods)
        {
            var mods = new List<Mod>();
            var eMods = explicitMods as IList<string> ?? explicitMods.ToList();

            if (HasHybrid(category, eMods, out var hybrid, out var remainingMods))
            {
                mods.AddRange(remainingMods.Select(e => Parse(category, e)));
                mods.AddRange(hybrid);
            }
            else mods.AddRange(eMods.Select(e => Parse(category, e)));
            return mods;
        }

        public static bool HasHybrid(String category, ICollection<String> explicitMods, out ICollection<Mod> hybrid,
            out IEnumerable<String> remainingMods)
        {
            var resultList = new List<(Mod mod, String modText1, Value value1, String modText2, Value value2)>();
            hybrid = new List<Mod>();
            remainingMods = new List<String>();
            if (!dict.TryGetValue(category, out var innerDict)) return false;
            foreach (var mod1 in explicitMods)
            {
                foreach (var mod2 in explicitMods)
                {
                    if (mod1 == mod2) continue;
                    if (!innerDict.TryGetValue((mod1 + "/" + mod2).Hash(), out var mod)) continue;

                    var result = ParseValue(mod1 + "/" + mod2, mod, true).ToArray();
                    if (result.Length > 0)
                    {
                        resultList.Add((mod.Copy(result), mod1, result[0].values[0], mod2, result[0].values[1]));
                    }
                }
            }

            if (resultList.Any())
            {
                foreach (var (mod, modText1, value1, modText2, value2) in resultList)
                {
                    decimal.TryParse(modText1.GetValues(), out var modValue1);
                    if (value1.Min <= modValue1 && modValue1 <= value1.Max)
                    {
                        explicitMods.Remove(modText1);
                    }
                    else
                    {
                        explicitMods.Remove(modText1);
                        explicitMods.Add(modText1.SubtractValue(
                            (modValue1 - decimal.Round(decimal.Divide(value1.Min + value1.Max, 2)))
                            .ToString(CultureInfo.InvariantCulture)));
                    }


                    decimal.TryParse(modText2.GetValues(), out var modValue2);
                    if (value2.Min <= modValue2 && modValue2 <= value2.Max)
                    {
                        explicitMods.Remove(modText2);
                    }

                    else
                    {
                        explicitMods.Remove(modText2);
                        explicitMods.Add(modText2
                            .SubtractValue((modValue2 - decimal.Round(decimal.Divide(value2.Min + value2.Max, 2)))
                                .ToString(CultureInfo.InvariantCulture)));
                    }

                    hybrid.Add(mod);
                }

                remainingMods = explicitMods.ToList();
                return true;
            }

            return false;
        }

        private static IEnumerable<Tier> ParseValue(string text, Mod mod, bool hybrid = false)
        {
            var values = new List<decimal>();
            var parsedValueText = Regex.Matches(text, "[\\d.]+").Cast<Match>().Select(e => e.Value).ToList();

            foreach (var pvt in parsedValueText)
            {
                decimal.TryParse(pvt, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var value);
                values.Add(value);
            }

            var matchedTiers = new List<Tier>();
            var parsedValue = ToValue(values);

            foreach (var tier in mod.tier)
            {
                switch (tier.values.Count)
                {
                    case 1:
                        if (tier.values[0] <= parsedValue) matchedTiers.Add(tier);
                        break;
                    case 2:
                        if (tier.values[0] <= parsedValue && tier.values[1] >= parsedValue) matchedTiers.Add(tier);
                        break;
                }
            }

            // In case no tier could be deducted from the parsed text
            if (hybrid && !matchedTiers.Any())
            {
                var mod1value = new Value {Min = parsedValue.Min, Max = parsedValue.Min};
                var mod2value = new Value {Min = parsedValue.Max, Max = parsedValue.Max};

                var potential_tier = new List<(Tier, bool, bool, bool, bool)>();

                foreach (var tier in mod.tier)
                {
                    var t1_certain = false;
                    var t2_certain = false;

                    var t1_works = false;
                    var t2_works = false;

                    // If mod has 50 and tier contains min == max == 50, then its certain that tier matches that mod
                    if (mod1value.Min - tier.values[0].Min == 0 && mod1value.Max - tier.values[0].Max == 0)
                        t1_certain = true;
                    if (mod2value.Min - tier.values[1].Min == 0 && mod2value.Max - tier.values[1].Max == 0)
                        t2_certain = true;

                    // if mod has 50 and tier has min == 45 and max == 55, then 50 is within the range of that tier
                    if (mod1value.Min - tier.values[0].Min >= 0 && mod1value.Max - tier.values[0].Max >= 0)
                        t1_works = true;
                    if (mod2value.Min - tier.values[1].Min >= 0 && mod2value.Max - tier.values[1].Max >= 0)
                        t2_works = true;

                    potential_tier.Add((tier, t1_certain, t2_certain, t1_works, t2_works));
                }
                // retrieve all matches
                foreach (var (tier, certain_v1, certain_v2, works_v1, works_v2) in potential_tier.OrderBy(e => e.Item2)
                    .ThenBy(e => e.Item3))
                {
                    if (certain_v1 && certain_v2) return new List<Tier> {tier};
                    if (certain_v1 && works_v2) return new List<Tier> {tier};
                    if (certain_v2 && works_v1) return new List<Tier> {tier};
                    if (works_v2 && works_v1) return new List<Tier> {tier};
                }
            }

            return matchedTiers;
        }

        public Mod Copy() => (Mod) Clone();

        public Mod Copy(IEnumerable<Tier> t)
        {
            var clone = (Mod) Clone();
            clone.tier = t;
            return clone;
        }

        public object Clone()
        {
            return new Mod
            {
                name = name,
                tier = tier.Select(t => t.Copy())
            };
        }

        public Mod Combine(Mod other)
        {
            var tiers = new List<Tier>();
            tiers.AddRange(tier.Select(e => e.Copy()));
            tiers.AddRange(other.tier.Select(e => e.Copy()));
            return Copy(tiers);
        }

        private static Value ToValue(IReadOnlyList<decimal> values)
        {
            switch (values.Count)
            {
                case 1: return new Value {Min = values[0], Max = values[0]};
                case 2: return new Value {Min = values[0], Max = values[1]};
                default: throw new Exception($"Unsupported value count: {values.Count}");
            }
        }
    }

    public static class ModExtensions
    {
        public static int Hash(this String s) => new String(s.ToLower().Where(char.IsLetter).ToArray()).GetHashCode();

        public static String GetValues(this String s) =>
            Regex.Matches(s, "[\\d.]+").Cast<Match>().Select(e => e.Value).FirstOrDefault() ?? "";

        public static String SubtractValue(this String s, String value) => Regex.Replace(s, "[0-9]+", value);
    }
}