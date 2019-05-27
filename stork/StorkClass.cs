﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stork
{
    //A class template in Stork.
    public class StorkClass
    {
        //Name of the class.
        public string Name;

        //Can directly assign to this eg. "int b = 3;" over "int b = new int(3);".
        public bool CanDirectAssign;

        //The direct assign value (if it's enabled).
        public Type DirectValueType;
        public object DirectValue;

        //Static/instance methods for the class.
        public Dictionary<string, StorkFunction> StaticMethods;
        public Dictionary<string, StorkFunction> InstanceMethods;

        //Static/instance fields for the class.
        public Dictionary<string, StorkClass> StaticFields;
        public Dictionary<string, StorkClass> InstanceFields;

        ///////////////////
        ///STORK METHODS///
        ///////////////////
        
        //Default constructor which initializes lists.
        public StorkClass()
        {
            StaticFields = new Dictionary<string, StorkClass>();
            StaticMethods = new Dictionary<string, StorkFunction>();
            InstanceFields = new Dictionary<string, StorkClass>();
            InstanceMethods = new Dictionary<string, StorkFunction>();
            DirectValue = null;
        }

        //Creates an instance of this class template to be used.
        public StorkClassInstance CreateInstance()
        {
            var fieldInstances = new Dictionary<string, StorkClassInstance>();
            foreach (var field in InstanceFields)
            {
                fieldInstances.Add(field.Key, field.Value.CreateInstance());
            }

            return new StorkClassInstance()
            {
                CanDirectAssign = CanDirectAssign,
                DirectValue = null,
                DirectValueType = DirectValueType,
                InstanceFields = fieldInstances,
                InstanceMethods = InstanceMethods,
                TypeName = Name
            };
        }
    }
}
