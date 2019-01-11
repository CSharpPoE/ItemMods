using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ItemMods.Model
{
    class ModConverter : JsonConverter
    {
        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            dynamic obj = JObject.Load(reader);
            var tiers = new List<Tier>();
            foreach (var tier in obj.tier)
            {
                switch ((String) tier.type)
                {
                    case "Base":
                        tiers.Add(new Base
                        {
                            name = tier.name,
                            type = tier.type,
                            ilvl = tier.ilvl,
                            values = tier.values.ToObject<IEnumerable<Value>>()
                        });
                        break;
                    case "Hybrid":
                        tiers.Add(new Base
                        {
                            name = tier.name,
                            type = tier.type,
                            ilvl = tier.ilvl,
                            values = tier.values.ToObject<IEnumerable<Value>>()
                        });
                        break;
                    case "Essence":
                        tiers.Add(new Essence
                        {
                            name = tier.name,
                            type = tier.type,
                            ilvl = tier.ilvl,
                            values = tier.values.ToObject<IEnumerable<Value>>()
                        });
                        break;
                    case "Master":
                        tiers.Add(new Master
                        {
                            type = tier.type,
                            master = tier.master,
                            ilvl = tier.ilvl,
                            lvl = tier.lvl,
                            values = tier.values.ToObject<IEnumerable<Value>>()
                        });
                        break;
                    default:
                        tiers.Add(new Unspecified());
                        break;
                }
            }

            return new Mod
            {
                name = obj.name,
                //hash = obj.hash,
                tier = tiers
            };
        }

        public override bool CanConvert(Type objectType) => objectType == typeof(Mod);
    }
}