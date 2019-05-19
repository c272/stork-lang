using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4;
using Antlr4.Runtime;

namespace stork
{
    public static  class ANTLRDebug
    {
        public static void PrintTokens(Lexer lexer)
        {
            //Getting tokens.
            var tokens = lexer.GetAllTokens();

            //Getting lexer vocabulary.
            var vocab = lexer.Vocabulary;

            //Printing, for each token.
            Console.WriteLine("ANTLR Debug Tokens:");
            foreach (var tok in tokens)
            {
                Console.WriteLine("[" + vocab.GetSymbolicName(tok.Type) + ", " + tok.Text + ", channel=" + tok.Channel + "]");
            }
        }
    }
}
