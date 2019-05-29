﻿using Antlr4.Runtime;
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
            string input = @"int b = 3+2+6; print(b);";

            //Debug print.
            ANTLRDebug.PrintTokens(input);

            //Debug print tree.
            ANTLRDebug.PrintParseList(input);

            //Getting tree.
            var tree = ANTLRDebug.GetTree(input);

            //Starting the walk.
            //var visitor = new storkVisitor();
            //visitor.VisitCompileUnit(tree);

            //Logging variables at end of runtime.
            
        }
    }
}
