using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4;
using Antlr4.Runtime;

namespace stork
{
    public static class ANTLRDebug
    {
        public static void PrintTokens(Lexer lexer)
        {
            //Getting tokens.
            var tokens = lexer.GetAllTokens();

            //Getting lexer vocabulary.
            var vocab = lexer.Vocabulary;

            //Printing, for each token.
            Console.WriteLine("ANTLR Lexed Tokens:");
            foreach (var tok in tokens)
            {
                Console.WriteLine("[" + vocab.GetSymbolicName(tok.Type) + ", " + tok.Text + ", channel=" + tok.Channel + "]");
            }
            Console.WriteLine("");
        }

        public static void PrintParseList(storkParser parser)
        {
            parser.BuildParseTree = true;
            var tree = parser.compileUnit();

            //Printing debug string.
            Console.WriteLine("ANTLR Debug Parse:");
            Console.WriteLine(tree.ToInfoString(parser));
            Console.WriteLine("");

            //Printing parse tree.
            Console.WriteLine("ANTLR Parse Tree:");
            Console.WriteLine(tree.ToStringTree());
            Console.WriteLine(tree.block().statement().Length);
        }
    }
}