using System;
using System.Collections.Generic;

namespace ItemMods.Model
{
    public abstract class Tier : ICloneable
    {
        public String type { get; set; }
        public List<Value> values { get; set; }

        public abstract object Clone();
        public abstract Tier Copy();
    }

    public class Master : Tier
    {
        public String master { get; set; }
        public int ilvl { get; set; }
        public int lvl { get; set; }

        public override object Clone()
        {
            return new Master
            {
                type = type,
                master = master,
                ilvl = ilvl,
                lvl = lvl,
                values = values
            };
        }

        public override Tier Copy() => (Master) Clone();
    }

    public class Essence : Tier
    {
        public string name { get; set; }
        public int ilvl { get; set; }

        public override object Clone()
        {
            return new Essence
            {
                name = name,
                type = type,
                ilvl = ilvl,
                values = values
            };
        }

        public override Tier Copy() => (Essence) Clone();
    }

    public class Base : Tier
    {
        public string name { get; set; }
        public int ilvl { get; set; }

        public override object Clone()
        {
            return new Base
            {
                name = name,
                type = type,
                ilvl = ilvl,
                values = values
            };
        }

        public override Tier Copy() => (Base) Clone();
    }

    public class Unspecified : Tier
    {
        public override object Clone()
        {
            return new Unspecified();
        }

        public override Tier Copy() => (Unspecified) Clone();
    }
}