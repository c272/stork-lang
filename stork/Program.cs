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
            string input = "int banana = 142;";
            var chars = new AntlrInputStream(input);
            var lexer = new storkLexer(chars);
            var tokens = new CommonTokenStream(lexer);

            //Debug print.
            ANTLRDebug.PrintTokens(lexer);

            //Debug print tree.
            var parser = new storkParser(tokens);
            ANTLRDebug.PrintParseList(parser);

            //Getting tree.
            parser.BuildParseTree = true;
            var tree = parser.compileUnit();

            //Starting the walk.
           // var visitor = new storkVisitor();
           // visitor.VisitCompileUnit(tree);
        }
    }
}
