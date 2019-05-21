namespace stork
{
    //A single raw value in Stork. Can be an integer, string, float.
    internal struct StorkValue
    {
        public ValueType Type;
        public object Value;
    }

    internal enum ValueType
    {
        INTEGER,
        FLOAT,
        STRING
    }
}