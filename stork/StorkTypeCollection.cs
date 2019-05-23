using System;
using System.Collections.Generic;
using System.Linq;

namespace stork
{
    public class StorkTypeCollection
    {
        //List of all the types in the collection.
        private Dictionary<string, StorkType> Types = new Dictionary<string, StorkType>();

        public StorkTypeCollection()
        {
            //Initialize default types here.
            Types.Add("int", new StorkType() { Name = "integer", InternalType = typeof(int) });
            Types.Add("str", new StorkType() { Name = "string", InternalType = typeof(string) });
            Types.Add("flt", new StorkType() { Name = "float", InternalType = typeof(float) });
            Types.Add("bln", new StorkType() { Name = "boolean", InternalType = typeof(bool) });
        }

        //Returns whether the given type exists in the collection.
        public bool ContainsType(string typeName)
        {
            return Types.ContainsKey(typeName);
        }

        //Returns all types.
        public Dictionary<string, StorkType> GetTypes()
        {
            return Types;
        }

        //Returns a type given the SHORTNAME, not the LONGNAME.
        public StorkType GetTypeByShortname(string shortname)
        {
            try
            {
                return Types[shortname];
            } catch
            {
                return default(StorkType);
            }
        }

        //Returns a type given the LONGNAME, not the SHORTNAME.
        public StorkType GetTypeByLongname(string longname)
        {
            //Getting type.
            var longType = Types.FirstOrDefault(x => x.Value.Name == longname);
            if (longType.Equals(default(KeyValuePair<string, StorkType>)))
            {
                //Type does not exist.
                return default(StorkType);
            }

            //Return the type.
            return longType.Value;
        }
    }
}