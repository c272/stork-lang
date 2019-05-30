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
        public static void PrintTokens(string testString)
        {
            //Creating lexer.
            var chars = new AntlrInputStream(testString);
            var lexer = new storkLexer(chars);

            //Getting tokens.
            var tokens = lexer.GetAllTokens();

            //Getting lexer vocabulary.
            var vocab = lexer.Vocabulary;

            //Printing, for each token.
            Console.WriteLine("ANTLR Lexed Tokens:");
            foreach (var tok in tokens)
            {
                Console.WriteLine("[" + vocab.GetSymbolicName(tok.Type) + ", " + tok.Text + "]");
            }
            Console.WriteLine("");

            //Reset lexer so it can still be used.
            lexer.Reset();
        }

        //Prints the entire processed ANTLR parse tree to console.
        public static void PrintParseList(string testString)
        {
            //Creating parser.
            var chars = new AntlrInputStream(testString);
            var lexer = new storkLexer(chars);
            var tokens = new CommonTokenStream(lexer);
            var parser = new storkParser(tokens);
            var tree = parser.compileUnit();

            //Printing parse tree.
            Console.WriteLine("ANTLR Parse Tree:");
            Console.WriteLine(tree.ToStringTree(parser));
            Console.WriteLine("Total Statements: "+tree.block().statement().Length+"\n");
        }

        //Returns the whole Stork tree.
        public static storkParser.CompileUnitContext GetTree(string testString)
        {
            var chars = new AntlrInputStream(testString);
            var lexer = new storkLexer(chars);
            var tokens = new CommonTokenStream(lexer);
            var parser = new storkParser(tokens);
            return parser.compileUnit();
        }

        public static void PrintStorkClassInfo(KeyValuePair<string, StorkClass> cl)
        {
            Console.WriteLine("Class '" + cl.Key + "':");
            Console.WriteLine("--");
            Console.WriteLine("Static Fields:");
            foreach (var field in cl.Value.StaticFields)
            {
                Console.WriteLine(field.Key + " of type " + field.Value.Name);
            }
            Console.WriteLine("Instance Fields:");
            foreach (var field in cl.Value.InstanceFields)
            {
                Console.WriteLine(field.Key + " of type " + field.Value.Name);
            }
            Console.WriteLine("--");
            Console.WriteLine("Static Methods:");
            foreach (var method in cl.Value.StaticMethods)
            {
                Console.WriteLine(method.Key + " of statement length " + method.Value.StatementBody.Count);
            }
            Console.WriteLine("Instance Methods:");
            foreach (var method in cl.Value.InstanceMethods)
            {
                Console.WriteLine(method.Key + " of statement length " + method.Value.StatementBody.Count);
            }
            Console.WriteLine("--");
            Console.WriteLine("Has Constructor?");
            if (cl.Value.Constructor==null || cl.Value.Constructor.StatementBody.Count == 0)
            {
                Console.WriteLine("Default.");
            }
            else
            {
                Console.WriteLine("User defined, length "+cl.Value.Constructor.StatementBody.Count+".");
            }
            Console.WriteLine("--");
            Console.WriteLine("Constructor Parameters:");
            if (cl.Value.Constructor != null)
            {
                foreach (var param in cl.Value.Constructor.Parameters)
                {
                    Console.WriteLine(param.Key + " of type " + param.Value.Name);
                }
            } else
            {
                Console.WriteLine("None.");
            }
        }
    }
}