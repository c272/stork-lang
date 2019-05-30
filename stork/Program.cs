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
            string input = @"class oceanMan(int b, int c) {
                                
                                construct {
                                    b = 5;
                                    c = 4;
                                }

                                //Define some fields.
                                int godlyGamer;
                                flt godlyFloat;

                                //Define a function.
                                static func internalClassFunc(int b) {
                                    print(""BRUH"");
                                }
                             }

                             oceanMan.internalClassFunc(3);
                            ";

            //Debug print.
            ANTLRDebug.PrintTokens(input);

            //Debug print tree.
            ANTLRDebug.PrintParseList(input);

            //Getting tree.
            var tree = ANTLRDebug.GetTree(input);

            //Starting the walk.
            var visitor = new storkVisitor();
            visitor.VisitCompileUnit(tree);

            //Logging classes at end of runtime.
            var classes = storkVisitor.Classes;
            Console.WriteLine("---------------------");
            foreach (var cl in classes.GetClasses())
            {
                ANTLRDebug.PrintStorkClassInfo(cl);
                Console.WriteLine("---------------------");
            }
        }
    }
}
