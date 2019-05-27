using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stork
{
    //Error processor in Stork.
    public static class StorkError
    {
        public static void Print(string errorMsg)
        {
            Console.WriteLine("Stork Runtime Error: " + errorMsg);
        }
    }
}
