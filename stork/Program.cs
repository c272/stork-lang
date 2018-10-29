using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace stork
{
    class Program
    {
        static void Main(string[] args)
        {
            //Checking that the command line arguments are given.
            if (args.Length<1)
            {
                StorkError.printError(StorkError.Error.invalid_args);
                return;
            }

            //Command line arguments are given, continue.
            switch (args[0])
            {
                case "--help":
                    //Printing the help section.
                    return;
            }

            //Assuming parameter is a file, attempt to load.
            string file = "";
            if(!StorkIO.loadFile(args[0], ref file))
            {
                StorkError.printError(StorkError.Error.invalid_file);
                return;
            }

            //Stripping string of all endline characters.
            file = Regex.Replace(file, @"\r\n?|\n", "");
            Console.WriteLine(file);

            //File is now loaded in, pass it to the lexer.
            StorkLexer lexer = new stork.StorkLexer();
            foreach (char c in file)
            {
                lexer.feed(c);
            }
            
            //Output lexer contents.
            for (int i=0; i<lexer.lexerList.Count; i++)
            {
                Console.WriteLine(lexer.lexerList[i].type+" -> \""+lexer.lexerList[i].item+"\"");
            }

            //Lexing is finished, now transfer onto the LexerScript -> ActionTreeScript conversion.
            StorkActionTree stkact = new StorkActionTree(lexer.lexerList);
        }
    }
}
