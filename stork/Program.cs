using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stork
{
    class Program
    {
        static void Main(string[] args)
        {
            //Test input string.
            string input = "int banana = 3;";

            //Debug print.
            ANTLRDebug.PrintTokens(input);

            //Debug print tree.
            ANTLRDebug.PrintParseList(input);

            //Getting tree.
            var tree = ANTLRDebug.GetTree(input);

            //Starting the walk.
            var visitor = new storkVisitor();
            visitor.VisitCompileUnit(tree);

            //Logging variables at end of runtime.
            Console.WriteLine("Types:\n");
            foreach (var typePair in storkVisitor.types.GetTypes())
            {
                Console.WriteLine(typePair.Key + " containing internal type " + typePair.Value.InternalType + ".");
            }

            Console.WriteLine("\nVariables:\n");
            foreach (var varPair in storkVisitor.scope.GetVariables())
            {
                Console.WriteLine(varPair.Key + ": Type " + varPair.Value.Type.Name + " with value " + varPair.Value.Value);
            }
        }
    }
}
