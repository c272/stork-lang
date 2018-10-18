using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace stork
{
    //"Type" enumeration.
    enum Type
    {
        identifier,
        import_token,
        _string,
        _symbol,
        number,
        in_statement,
        vartype,
        nulltype
    }

    class StorkLexer
    {
        //Class constructor, currently empty, but may take parameters in the future.
        public StorkLexer()
        {

        }

        //"Feed" function, which lets the main program feed input into the lexer.
        public void feed(char c)
        {
            //Checking if the statement is over. If not, add to current statement.
            if (c==' '||c=='('||c=='{'||c==';')
            {
                Console.WriteLine("FIRED");
                //Statement over, end statement and pass to the type interpreter.
                interpretType(currentStatement);
                //Reset currentStatement to blank.
                currentStatement = "";
            } else
            {
                currentStatement += c;
            }
        }

        public void interpretType(string s)
        {
            Type typeOut;
            //Switching to determine type.
            switch (s)
            {
                case "void":
                    typeOut = Type.nulltype;
                    break;
                case "int":
                case "float":
                case "bool":
                case "string":
                    typeOut = Type.vartype;
                    break;
                default:
                    //Checking if it's a string.
                    Regex stringReg = new Regex("^\\\"(\\w+)\\\"\\z");
                    if (stringReg.IsMatch(s))
                    {
                        //Valid, type as string.
                        typeOut = Type._string;
                        s = s.Substring(1);
                        s = s.Remove(s.Length - 1);
                        
                        //Send to the event list.
                        lexerTypeList.Add(typeOut);
                        lexerStringList.Add(s);
                        return;
                    }

                    //Checking if it's an import command.
                    Regex importReg = new Regex("^~import\\z");
                    if (importReg.IsMatch(s))
                    {
                        //It's an import, type as such and push to event list.
                        typeOut = Type.import_token;
                        lexerTypeList.Add(typeOut);
                        lexerStringList.Add(s);
                    }

                    //Not a string.
                    break;
            }
        }

        //Getters for the type and string list.
        public List<Type> getTypeList()
        {
            return lexerTypeList;
        }
        public List<string> getStringList()
        {
            return lexerStringList;
        }

        //Private lexer properties.
        private string currentStatement;
        private List<Type> lexerTypeList = new List<Type>();
        private List<string> lexerStringList = new List<string>();
    }
}
