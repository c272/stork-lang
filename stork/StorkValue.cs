namespace stork
{
    //A single raw value in Stork. Can be an integer, string, float.
    public struct StorkValue
    {
        public StorkType Type;
        public object Value;
    }
}