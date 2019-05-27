using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stork
{
    //A collection of Stork Functions. Mutable by method.
    public class StorkFunctionCollection
    {
        //The list of functions.
        private Dictionary<string, StorkFunction> Functions = new Dictionary<string, StorkFunction>();

        //Constructor for test functions.
        public StorkFunctionCollection()
        {
            Functions.Add("print", new StorkFunction(
                StorkTestFunctions.Print,
                new Dictionary<string, StorkClass>()
                {
                    {"toPrint", storkVisitor.Classes.GetClass("val") }
                }
            ));
        }

        //Add a function.
        public Tuple<bool, string> AddFunction(string name, List<storkParser.StatementContext> body, Dictionary<string, StorkClass> params_)
        {
            //Checking collection does not have a dupe name.
            if (Functions.ContainsKey(name))
            {
                return new Tuple<bool, string>(false, "A function with this name already exists, cannot duplicate.");
            }

            //Add the function.
            Functions.Add(name, new StorkFunction(body, params_));
            return new Tuple<bool, string>(true, "");
        }

        //Get a function by name.
        public StorkFunction GetFunction(string name)
        {
            try
            {
                return Functions[name];
            } catch
            {
                return default(StorkFunction);
            }
        }
    }
}
