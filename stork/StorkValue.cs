namespace stork
{
    //A single raw value in Stork. Can be an integer, string, float.
    public struct StorkValue
    {
        public ValueType Type;
        public object Value;
    }

    public enum ValueType
    {
        INTEGER,
        FLOAT,
        STRING
    }
}