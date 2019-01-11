using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using ItemMods.Model;
using ItemMods.Model.Item;
using PathOfExile;
using PathOfExile.Model;

namespace ItemMods
{
    public class Program
    {
        static void Main(string[] args)
        {
            Mod.Init();

            //var res = Mod.Parse("belt",
            //    new List<String>
            //    {
            //        "35% increased Elemental Damage with Attack Skills"
            //    });

            PublicStashAPI.Run(new List<Action<PublicStash>> {GatherMods});
        }

        public static void GatherMods(PublicStash publicStash)
        {
            const int NORMAL = 0;
            const int UNIQUE = 3;
            var parsed = 0;
            var mods = new List<Mod>();
            foreach (var stash in publicStash.stashes)
            {
                foreach (var item in stash.items)
                {
                    if (!item.identified || item.frameType == NORMAL || item.frameType == UNIQUE)
                    {
                        continue; //unique
                    }
                    switch (item)
                    {
                        case Armour arm:
                            mods.AddRange(Mod.Parse(ParseCategory(arm.category.armour.FirstOrDefault(), arm.typeLine),
                                arm.explicitMods));
                            //mods.AddRange(Mod.Parse(ParseCategory(arm.category.armour.FirstOrDefault()),
                            //    arm.explicitMods));
                            break;
                        case Weapon wep:
                            mods.AddRange(Mod.Parse(wep.category.weapons.FirstOrDefault(), wep.explicitMods));
                            break;
                        case Accessory acc:
                            mods.AddRange(Mod.Parse(acc.category.accessories.FirstOrDefault(), acc.explicitMods));
                            break;
                    }
                    // Send all the mods to the mods parser, it can decide whether somnething is hybrid or not
                    if (mods.Any(e => e == null || e.tier?.Count() == 0))
                    {
                    }
                    mods.Clear();
                    parsed = parsed + 1;
                }
            }
        }

        private static String ParseCategory(String category, String typeline)
        {
            switch (category)
            {
                case "chest": return ChestType.Parse(typeline);
                case "gloves": return GloveType.Parse(typeline);
                case "helmet": return HelmetType.Parse(typeline);
                case "boots": return BootsType.Parse(typeline);
                case "shield": return ShieldType.Parse(typeline);
                default: return category;
            }
        }

        private static void ToJson()
        {
            var directory = "C:\\tmp\\poe\\poeaffixwithmods\\";
            var subgroup = new List<List<String>>();
            var group = new List<String>();
            foreach (var name in Directory.GetFiles(directory))
            {
                var lines = File.ReadAllLines(name);
                var count = lines.Length;
                for (var i = 0; i < count; i++)
                {
                    if (i + 1 < count && lines[i + 1][0] != 'i')
                    {
                        group.Add(lines[i]);
                        subgroup.Add(group.ToList());
                        group = new List<String>();
                    }
                    else
                    {
                        group.Add(lines[i]);
                    }
                }

                subgroup.Add(group);
                group = new List<String>();
                ConvertToJson(subgroup, name.Replace(directory, "").Replace(".txt", ""));
                subgroup = new List<List<String>>();
            }
        }

        public static void ConvertToJson(List<List<String>> groups, String filename)
        {
            var mods = new List<JsonMod>();
            foreach (var grp in groups)
            {
                var mod = new JsonMod();
                for (var i = 0; i < grp.Count; i++)
                {
                    if (i == 0)
                    {
                        mod.name = grp[i];
                        mod.hash = new String(mod.name.ToLower().Where(Char.IsLetter).ToArray()).GetHashCode();
                        continue;
                    }

                    var s = grp[i];

                    if (mod.name.Contains("/"))
                    {
                        if (EssenceMod(s)) mod.tier.Add(ToEssence(s));
                        else if (MasterMod(s)) mod.tier.Add(ToMaster(s));
                        else mod.tier.Add(ToHybrid(s));
                    }
                    else
                    {
                        if (EssenceMod(s)) mod.tier.Add(ToEssence(s));
                        else if (MasterMod(s)) mod.tier.Add(ToMaster(s));
                        else mod.tier.Add(ToMod(s));
                    }
                }
                mods.Add(mod);
                //mods.Add(JsonConvert.SerializeObject(mod, Formatting.Indented));
                //var t = JsonConvert.SerializeObject(mod, Formatting.Indented);
                //----------------
                /* TODO Next step:
                    1. Make a hierarchy of mods. BaseMod <- { Regular, Master, Essence, (Corruption) }
                    2. Check the json format, if you want to change, for example: Mastermod should not be a separat object. move its properties to the tier level.
                    3. Write everything to files. 
                    4. Write the parser which either returns a mod if not found return not_found_mod or something
                    5. Let the program run while you find discrepancies in the mods (adjust and fix)
                    6. Celebrate!
                */
                //----------------
            }

            //if ( String.IsNullOrEmpty(filename) )
            //{
            //}
            //else
            //{
            //    File.WriteAllText($"C:\\tmp\\poe\\json\\{filename}.json",
            //        JsonConvert.SerializeObject(mods, Formatting.Indented));
            //}
        }

        private static Tier ToHybrid(String s)
        {
            //iLvl 1: 3 - 7 / 3 - 7(Supple)
            //iLvl 8: 5 / 10 - 20(of Shining)
            //ilvl 68: 18 / 15(of Shaping)

            int.TryParse(Regex.Replace(Regex.Match(s, "[iIlLvVlL]+[0-9\\s]+").Value, "ilvl ", "",
                RegexOptions.IgnoreCase), out var ilvl);


            var name = Regex.Match(s, "[\\(][\\w\\d\\s',]+\\)").Value.Replace("(", "").Replace(")", "");

            var values1 = new List<decimal>();
            var values2 = new List<decimal>();


            foreach (var str in Regex.Matches(s, $":[\\w\\d\\s-.,#%'/]+[/]").Cast<Match>()
                .Select(e => e.Value))
            {
                var f = Regex.Matches(str, "[\\d.]+").Cast<Match>().Select(e => e.Value).ToList();

                foreach (var g in f)
                {
                    decimal.TryParse(g, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture,
                        out var value);
                    values1.Add(value);
                }
            }


            foreach (var str in Regex.Matches(s,
                    $"/ [\\w\\d\\s-.,#%']+[\\(]").Cast<Match>()
                .Select(e => e.Value))
            {
                var f = Regex.Matches(str, "[\\d.]+").Cast<Match>().Select(e => e.Value).ToList();

                foreach (var g in f)
                {
                    decimal.TryParse(g, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture,
                        out var value);
                    values2.Add(value);
                }
            }

            //var t = Regex.Matches(s,
            //        "([iIlLvVlL]+[0-9\\s]+:([\\w\\d\\s-.,#%'/]+)?[\\(][\\w\\d\\s',]+\\))").Cast<Match>()
            //    .Select(e => e.Value).ToList();


            var modvalues = ToValue(values1);
            modvalues.AddRange(ToValue(values2));
            return new Hybrid
            {
                name = name,
                ilvl = ilvl,
                values = ToValue(values1).AddValueRange(ToValue(values2))
            };
        }

        private static Tier ToMod(String s)
        {
            int.TryParse(Regex.Replace(Regex.Match(s, "[iIlLvVlL]+[0-9\\s]+").Value, "ilvl ", "",
                RegexOptions.IgnoreCase), out var ilvl);

            var name = Regex.Match(s, "[\\(][\\w\\d\\s',]+\\)").Value.Replace("(", "").Replace(")", "");

            var values = new List<decimal>();
            foreach (var str in Regex.Matches(s,
                    $":[\\w\\d\\s-.,#%'/]+[\\(]").Cast<Match>()
                .Select(e => e.Value))
            {
                var f = Regex.Matches(str, "[\\d.]+").Cast<Match>().Select(e => e.Value).ToList();

                foreach (var g in f)
                {
                    decimal.TryParse(g, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var value);
                    values.Add(value);
                }
            }

            //var t = Regex.Matches(s,
            //        "([iIlLvVlL]+[0-9\\s]+:([\\w\\d\\s-.,#%'/]+)?[\\(][\\w\\d\\s',]+\\))").Cast<Match>()
            //    .Select(e => e.Value).ToList();

            return new Base
            {
                name = name,
                ilvl = ilvl,
                values = ToValue(values)
            };
        }

        private static Tier ToEssence(String s)
        {
            var name = Regex.Match(s, "[\\(][\\w\\d\\s',]+\\)").Value.Replace("(", "").Replace(")", "");

            var values = new List<decimal>();
            foreach (var str in Regex.Matches(s,
                    $":[\\w\\d\\s-.,#%'/]+[\\(]").Cast<Match>()
                .Select(e => e.Value))
            {
                var f = Regex.Matches(str, "[\\d.]+").Cast<Match>().Select(e => e.Value).ToList();

                foreach (var g in f)
                {
                    decimal.TryParse(g, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var value);
                    values.Add(value);
                }
            }

            //var t = Regex.Matches(s,
            //        "([iIlLvVlL]+[0-9\\s]+:([\\w\\d\\s-.,#%'/]+)?[\\(][\\w\\d\\s',]+\\))").Cast<Match>()
            //    .Select(e => e.Value).ToList();

            return new Essence
            {
                name = name,
                values = ToValue(values),
            };
        }

        private static Tier ToMaster(String s)
        {
            int.TryParse(Regex.Replace(Regex.Match(s, "[iIlLvVlL]+[0-9\\s]+").Value, "ilvl ", "",
                RegexOptions.IgnoreCase), out var ilvl);

            //var name = Regex.Match(s, "[\\(][\\w\\d\\s',]+\\)").Value.Replace("(", "").Replace(")", "").Replace(" lvl ", " ");

            var mastermod = Regex.Match(s, "[\\(][\\w\\d\\s',]+\\)").Value.Replace("(", "").Replace(")", "");
            var master = Masters(mastermod);
            int.TryParse(Regex.Match(mastermod, "[\\d]+").Value, out var lvl);


            var values = new List<decimal>();
            foreach (var str in Regex.Matches(s,
                    $":[\\w\\d\\s-.,#%'/]+[\\(]").Cast<Match>()
                .Select(e => e.Value))
            {
                var f = Regex.Matches(str, "[\\d.]+").Cast<Match>().Select(e => e.Value).ToList();

                foreach (var g in f)
                {
                    decimal.TryParse(g, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var value);
                    values.Add(value);
                }
            }

            //var t = Regex.Matches(s,
            //        "([iIlLvVlL]+[0-9\\s]+:([\\w\\d\\s-.,#%'/]+)?[\\(][\\w\\d\\s',]+\\))").Cast<Match>()
            //    .Select(e => e.Value).ToList();

            return new Master
            {
                master = master,
                ilvl = ilvl,
                lvl = lvl,
                values = ToValue(values),
            };
        }

        private static List<Value> ToValue(List<decimal> values)
        {
            switch (values.Count)
            {
                case 1:
                    return new List<Value>
                    {
                        new Value {min = values[0], max = values[0]}
                    };
                case 2:
                    return new List<Value>
                    {
                        new Value {min = values[0], max = values[1]}
                    };
                case 3:
                    return new List<Value>
                    {
                        new Value {min = values[0], max = values[0]},
                        new Value {min = values[1], max = values[2]}
                    };
                case 4:
                    return new List<Value>
                    {
                        new Value {min = values[0], max = values[1]},
                        new Value {min = values[2], max = values[3]}
                    };
                default: return new List<Value>();
            }
        }

        private static bool MasterMod(String s)
        {
            if (Regex.IsMatch(s, @"\bHaku\b", RegexOptions.IgnoreCase)) return true;
            if (Regex.IsMatch(s, @"\bElreon\b", RegexOptions.IgnoreCase)) return true;
            if (Regex.IsMatch(s, @"\bCatarina\b", RegexOptions.IgnoreCase)) return true;
            if (Regex.IsMatch(s, @"\bTora\b", RegexOptions.IgnoreCase)) return true;
            if (Regex.IsMatch(s, @"\bVorici\b", RegexOptions.IgnoreCase)) return true;
            if (Regex.IsMatch(s, @"\bLeo\b", RegexOptions.IgnoreCase)) return true;
            if (Regex.IsMatch(s, @"\bVagan\b", RegexOptions.IgnoreCase)) return true;
            if (Regex.IsMatch(s, @"\bZana\b", RegexOptions.IgnoreCase)) return true;

            return false;
        }

        private static String Masters(String s)
        {
            if (Regex.IsMatch(s, @"\bHaku\b", RegexOptions.IgnoreCase)) return "Haku";
            if (Regex.IsMatch(s, @"\bElreon\b", RegexOptions.IgnoreCase)) return "Elreon";
            if (Regex.IsMatch(s, @"\bCatarina\b", RegexOptions.IgnoreCase)) return "Catarina";
            if (Regex.IsMatch(s, @"\bTora\b", RegexOptions.IgnoreCase)) return "Tora";
            if (Regex.IsMatch(s, @"\bVorici\b", RegexOptions.IgnoreCase)) return "Vorici";
            if (Regex.IsMatch(s, @"\bLeo\b", RegexOptions.IgnoreCase)) return "Leo";
            if (Regex.IsMatch(s, @"\bVagan\b", RegexOptions.IgnoreCase)) return "Vagan";
            if (Regex.IsMatch(s, @"\bZana\b", RegexOptions.IgnoreCase)) return "Zana";

            return null;
        }

        private static bool EssenceMod(String s)
        {
            return new[]
                {
                    "whispering", "muttering", "weeping", "wailing", "screaming", "shrieking", "deafening", "Corrupted"
                }
                .Any(
                    ess => s.IndexOf(ess, StringComparison.OrdinalIgnoreCase) >= 0);
        }


        public class Value
        {
            public decimal min { get; set; }
            public decimal max { get; set; }
        }

        public class Mastermod
        {
            public string name { get; set; }
            public int lvl { get; set; }
        }

        public class Tier
        {
            public List<Value> values { get; set; }
        }

        public class Master : Tier
        {
            public String type => "Master";
            public String master { get; set; }
            public int ilvl { get; set; }
            public int lvl { get; set; }
        }

        public class Essence : Tier
        {
            public String type => "Essence";
            public string name { get; set; }
            public int ilvl => 1;
        }

        public class Base : Tier
        {
            public String type => "Base";
            public string name { get; set; }
            public int ilvl { get; set; }
        }

        public class Hybrid : Tier
        {
            public String type => "Hybrid";
            public string name { get; set; }
            public int ilvl { get; set; }
        }

        public class JsonMod
        {
            public JsonMod()
            {
                tier = new List<Tier>();
            }

            public string name { get; set; }
            public int hash { get; set; }
            public List<Tier> tier { get; }
        }

        public static void Run()
        {
            var publicStash = PublicStashAPI.GetAsync().Result;
            var mods = new List<Mod>();

            foreach (var stash in publicStash.stashes)
            {
                foreach (var item in stash.items)
                {
                    if (!item.identified || item.frameType == 0 || item.frameType == 3)
                    {
                        continue; //unique
                    }
                    switch (item)
                    {
                        case Armour arm:
                            mods.AddRange(CreateAffixes(arm.category.armour.FirstOrDefault(), arm.explicitMods));
                            break;
                        case Weapon wep:
                            mods.AddRange(CreateAffixes(wep.category.weapons.FirstOrDefault(), wep.explicitMods));
                            break;
                        case Accessory acc:
                            mods.AddRange(CreateAffixes(acc.category.accessories.FirstOrDefault(), acc.explicitMods));
                            break;
                    }

                    if (mods.Any())
                    {
                    }
                }
            }
        }

        private static IEnumerable<Mod> CreateAffixes(String category, IEnumerable<String> explicitMods) =>
            Mod.Parse(category, explicitMods);


        // Simple regex which will download all the mods from poeaffix and store them to a file
        private static void RegexPoeAffix()
        {
            using (WebClient client = new WebClient()) // WebClient class inherits IDisposable
            {
                var htmlCode = client.DownloadString("http://poeaffix.net/1h-claw.html");

                //var test = "<a href=\"1h-axe.html\"><li>Axe</li></a>";

                var matches = Regex.Matches(htmlCode,
                        @"<a href=""[\w\d-]+.html").Cast<Match>()
                    .Select(m => m.Value).ToArray();

                var sites = new List<String>();

                foreach (var str in matches)
                {
                    if (str.Contains("index")) continue;
                    sites.Add(str.Replace("<a href=", "").Replace("\"", "").Replace(@"\", ""));
                }

                foreach (var site in sites)
                {
                    htmlCode = client.DownloadString($"http://poeaffix.net/{site}");

                    //var matches1 = Regex.Matches(htmlCode,
                    //        @"<a href=""#/"" type=""changecolor"">[+#%'/a-zA-Z0-9\s]+((<br>)[+#%'\d\w\s]+)?")
                    //    .Cast<Match>()
                    //    .Select(m => m.Value).ToArray();

                    //var matches2 = Regex.Matches(htmlCode,
                    //        @"<div class=""expander"">[+#%'/a-zA-Z0-9\s]+((<br>)[+#%'\d\w\s]+)?")
                    //    .Cast<Match>()
                    //    .Select(m => m.Value).ToArray();

                    //var mods = new List<String>();

                    //foreach (var str in matches1)
                    //{
                    //    mods.Add(str.Replace("<a href=\"#/\" type=\"changecolor\">", "").Replace("<br>", ""));
                    //}

                    //foreach (var str in matches2)
                    //{
                    //    mods.Add(str.Replace("<div class=\"expander\">", "").Replace("<br>", ""));
                    //}
                    ////< title > One Hand Axe</ title >

                    var matches3 = Regex.Matches(htmlCode, @"<title>[\w\s]+")
                        .Cast<Match>()
                        .Select(m => m.Value).ToArray();

                    var name = matches3.FirstOrDefault()?.Replace("<title>", "").Replace(" ", "");
                    if (name == "HelmetEnchantment") continue;

                    var mods = Reeggggex(site, name);

                    if (String.IsNullOrEmpty(name))
                    {
                    }
                    else
                    {
                        File.WriteAllLines($"C:\\tmp\\poe\\poeaffixwithmods\\{name}.txt", mods);
                    }
                }
            }
        }

        public static List<String> Reeggggex(String endpoint, String filename)
        {
            string html;
            // obtain some arbitrary html....
            using (var client = new WebClient())
            {
                html = client.DownloadString($"http://poeaffix.net/{endpoint}");
            }

            var test = File.ReadAllText($"C:\\tmp\\poe\\poeaffix\\{filename}.txt");
            var t2 = "(" + test.Replace("\r\n", "|").Replace("+", "\\+");
            var optionalMatches = t2.Remove(t2.Count() - 1, 1) + ")+";

            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            var sb = new StringBuilder();

            var list = new List<String>();

            foreach (var htmlNode in doc.DocumentNode
                .Descendants("div").Where(div => div.HasClass("affix")))
            {
                var innerText = htmlNode.InnerText;
                var removedTabAndNewLine = innerText.Replace("\n", "").Replace("\t", "").Replace("Â", "");
                var t = Regex.Matches(removedTabAndNewLine,
                        $"({optionalMatches}|[iIlLvVlL]+[0-9\\s]+:([\\w\\d\\s-.,#%'/]+)?[\\(][\\w\\d\\s',]+\\))")
                    .Cast<Match>().Select(e => e.Value).ToList();


                var skipnext = false;
                for (var i = 0; i < t.Count; i++)
                {
                    if (skipnext)
                    {
                        skipnext = false;
                        continue;
                    }

                    if (i + 1 < t.Count())
                    {
                        var first = t[i][0];
                        var second = t[i + 1][0];
                        if (first != 'i' && first != 'I' && second != 'i' && second != 'I')
                        {
                            list.Add($"{t[i]} / {t[i + 1]}");
                            skipnext = true;
                        }
                        else
                        {
                            list.Add(t[i]);
                        }
                    }
                    else
                    {
                        list.Add(t[i]);
                    }
                }
            }

            return list;
        }
    }

    public static class ProgramExtensions
    {
        public static List<Program.Value> AddValueRange(this List<Program.Value> self, List<Program.Value> other)
        {
            self.AddRange(other);
            return self;
        }
    }
}