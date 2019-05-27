using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stork
{
    //A mutable collection of stork classes.
    public class StorkClassCollection
    {
        //The base classes dictionary.
        private Dictionary<string, StorkClass> Classes = new Dictionary<string, StorkClass>();

        //Constructor for the class collection. Includes default values.
        public StorkClassCollection()
        {
            //Integer.
            Classes.Add("int", new StorkClass()
            {
                CanDirectAssign = true,
                DirectValueType = typeof(int),
                Name = "Stork.Integer"
            });

            //Float.
            Classes.Add("flt", new StorkClass()
            {
                CanDirectAssign = true,
                DirectValueType = typeof(float),
                Name = "Stork.Float"
            });


            //String.
            Classes.Add("str", new StorkClass()
            {
                CanDirectAssign = true,
                DirectValueType = typeof(string),
                Name = "Stork.String"
            });

            //Boolean.
            Classes.Add("bln", new StorkClass()
            {
                CanDirectAssign = true,
                DirectValueType = typeof(int),
                Name = "Stork.Boolean"
            });
        }

        //Add a class to the list.
        public void AddClass(string name, StorkClass classTemplate)
        {
            Classes.Add(name, classTemplate);
        }

        //Get a class from the list by name.
        public StorkClass GetClass(string name)
        {
            try
            {
                return Classes[name];
            }
            catch
            {
                return default(StorkClass);
            }
        }
    }
}
