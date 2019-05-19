using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4;
using Antlr4.Runtime;
using stork.Grammars;

namespace stork
{
    class Program
    {
        static void Main(string[] args)
        {
            //Test input string.
            string input = "flt banana = epicFunctionTime(2, 13.42, \"margaret\");";
            var chars = new AntlrInputStream(input);
            var lexer = new storkLexer(chars);
            var tokens = new CommonTokenStream(lexer);

            //Debug print.
            ANTLRDebug.PrintTokens(lexer);

            var parser = new storkParser(tokens);
            parser.BuildParseTree = true;

            //Getting tree.
            var tree = parser.compileUnit();

            //Starting the walk.
            var visitor = new storkVisitor();
            visitor.VisitCompileUnit(tree);
        }
    }
}
