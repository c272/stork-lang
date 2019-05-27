using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stork
{
    public static class StorkTestFunctions
    {
        //Prints the first given argument's value to console.
        public static StorkClassInstance Print(List<StorkClassInstance> args)
        {
            if (args[0].DirectValue != null)
            {
                Console.WriteLine(args[0].DirectValue.ToString());
            }
            else
            {
                Console.WriteLine(args[0].TypeName);
            }
            return null;
        }
    }
}
