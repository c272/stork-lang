using System;
using System.Collections.Generic;

namespace stork
{
    //A mirror for StorkClass, but without the static fields.
    public class StorkClassInstance
    {
        //Can directly assign to this eg. "int b = 3;" over "int b = new int(3);".
        public string TypeName;
        public bool CanDirectAssign;

        //The direct assign value (if it's enabled).
        public Type DirectValueType;
        public object DirectValue;
        
        //Instance methods and fields.
        public Dictionary<string, StorkFunction> InstanceMethods;
        public Dictionary<string, StorkClassInstance> InstanceFields;
    }
}