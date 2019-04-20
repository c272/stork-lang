using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace stork
{
    class Program
    {
        static void Main(string[] args)
        {
            //Opening the provided file.
            //string script = File.ReadAllText(args[0]);

            //Setting up the tokenizer with the token regex patterns.
            AddToken("[~]{1}", TokenType.PreprocessorStatement, 2);
            AddToken("[(]{1}", TokenType.OpenBracket, 2);
            AddToken("[)]{1}", TokenType.CloseBracket, 2);
            AddToken("[{]{1}", TokenType.OpenCBracket, 2);
            AddToken("[}]{1}", TokenType.CloseCBracket, 2);
            AddToken("[;]{1}", TokenType.EndLine, 2);
            AddToken("[\"]{1}[a-zA-Z][a-zA-Z0-9\\s]*[\"]{1}", TokenType.StringLiteral, 1);
            AddToken("[0-9]+", TokenType.IntLiteral, 2);
            AddToken("func", TokenType.FunctionDefinition, 2);
            AddToken("[,]{1}", TokenType.Separator, 2);
            AddToken("(^|\\s)[a-zA-Z][a-zA-Z0-9]*", TokenType.Identifier, 3);

            //Tokenizing the given script.
            List <Token> tokens = Tokenizer.Process(
            "func b() {" +
            "\"this is epic\";" +
            "\n"+
            "}"
            );

            foreach (var token in tokens)
            {
                Console.WriteLine(token.Value + " | " + token.Type);
            }
        }

        static void AddToken(string reg, TokenType t, int prec)
        {
            Tokenizer.AddToken(reg, t, prec);
        }
    }
}
