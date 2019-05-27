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

            //Wildcard (any class) class.
            Classes.Add("val", new StorkClass()
            {
                CanDirectAssign = false,
                Name = "Stork.Wildcard"
            });

            //Test class "c". Contains an integer "d".
            Classes.Add("c", new StorkClass()
            {
                CanDirectAssign = false,
                Name = "Stork.TestClass.C",
                StaticFields = new Dictionary<string, StorkClass>()
                {
                    {"d", Classes["int"] } //integer
                }
                
            });

            //Creating the static instances.
            Classes["c"].ReloadStaticInstance();
        }

        //Creates an instance of a class given a class name and value.
        public Tuple<bool, string, StorkClassInstance> CreateInstanceDV(string className, object value)
        {
            //Getting the relevant class template.
            var classTemplate = GetClass(className);
            if (classTemplate == null)
            {
                return new Tuple<bool, string, StorkClassInstance>(false, "Invalid internal class name, contact developer.", null);
            }

            //Checking the type is DirectValue enabled.
            if (!classTemplate.CanDirectAssign)
            {
                return new Tuple<bool, string, StorkClassInstance>(false, "Cannot use operators/create direct value instances from a class that has direct values disabled.", null);
            }

            //Return a new instance of the template.
            var instance = classTemplate.CreateInstance();
            instance.DirectValue = value;
            return new Tuple<bool, string, StorkClassInstance>(true, "", instance);
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
                return null;
            }
        }
    }
}
