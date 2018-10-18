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
            //Checking that the command line arguments are given.
            if (args.Length<1)
            {
                Console.WriteLine("STKI E001: No command line arguments given, use --help for more information.");
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
                Console.WriteLine("STKI E002: Invalid file path given, or error reading the file.");
                return;
            }

            //File is now loaded in, pass it to the lexer.
            StorkLexer lexer = new stork.StorkLexer();
            foreach (char c in file)
            {
                lexer.feed(c);
            }

            List<string> slist = lexer.getStringList();
            List<Type> tlist = lexer.getTypeList();

            Console.WriteLine(slist.Count);
            for (int i=0; i<slist.Count; i++)
            {
                Console.WriteLine(slist[i]);
                Console.WriteLine(tlist[i]);
            }
        }
    }
}
