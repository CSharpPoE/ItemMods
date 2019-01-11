namespace ItemMods.Model
{
    public struct Value
    {
        public decimal Min { get; set; }
        public decimal Max { get; set; }

        public static bool operator <=(Value minValue, Value other)
        {
            if (minValue.Min == minValue.Max) return minValue.Min <= other.Min;
            return minValue.Min <= other.Min && other.Min <= minValue.Max; 
        }

        public static bool operator >=(Value maxValue, Value other)
        {
            if ( maxValue.Min == maxValue.Max ) return maxValue.Max >= other.Max;
            return maxValue.Min <= other.Max && other.Max <= maxValue.Max;
        }
    }
}