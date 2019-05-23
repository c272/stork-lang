using System;
using System.Collections.Generic;

namespace stork
{
    public class StorkTypeCollection
    {
        public Dictionary<string, StorkType> Types = new Dictionary<string, StorkType>();

        public StorkTypeCollection()
        {
            //Initialize default types here.
            Types.Add("int", new StorkType() { Name = "integer", InternalType = typeof(int) });
            Types.Add("str", new StorkType() { Name = "string", InternalType = typeof(string) });
            Types.Add("flt", new StorkType() { Name = "float", InternalType = typeof(float) });
            Types.Add("bln", new StorkType() { Name = "boolean", InternalType = typeof(bool) });
        }
    }
}