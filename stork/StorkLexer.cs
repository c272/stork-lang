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
        preprocess_identifier,
        preprocess_directive,
        _string,
        endline,
        unknown_identifier
    }

    //"Lexer state" enumeration.
    enum LexerState
    {
        DEFAULT,
        IN_STRING
    }

    //The LexerItem struct.
    class LexerItem
    {
        //Basic constructor. Item contents are not required, so are an optional parameter.
        public LexerItem(Type _type, string _item="")
        {
            type = _type;
            item = _item;
        }

        //Properties.
        public string item = "";
        public Type type;
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
            //Incrementing token.
            token += c;

            //Checking if currently inside a string.
            if (lexerState != LexerState.IN_STRING)
            {
                //Checking token against a table.
                switch (token)
                {
                    case " ":
                        //Reset, ignore whitespace.
                        token = "";
                        break;
                    case "~":
                        //Preprocess character detected, add to lexer list and reset token.
                        addToList(Type.preprocess_identifier);
                        token = "";
                        break;
                    case ";":
                        //End of line character detected, add to list and reset.
                        addToList(Type.endline);
                        token = "";
                        break;
                    case "import ":
                        //Import keyword detected, put inside list and reset.
                        addToList(Type.preprocess_directive, token.Substring(0, token.Length-1));
                        token = "";
                        break;
                    case "\"":
                        //String initiation detected. Reset token and change state.
                        lexerState = LexerState.IN_STRING;
                        token = "";
                        break;
                    default:
                        //Checking if current char is a space.
                        if (c==' ')
                        {
                            //Yes, we're at the end of a keyword and it's unrecognised.
                            //Assume it's a variable/function identifier, and push.
                            addToList(Type.unknown_identifier, token.Substring(0, token.Length - 1));
                            token = "";
                        }
                        //It's not, could still be in the middle of a string.
                        break;
                }
            } else
            {
                //Yeah, inside a string. If the character isn't a quote closing it,
                //ignore everything and just keep adding to token.
                if (c=='\"')
                {
                    //Return to default state, reset token and push to list.
                    lexerState = LexerState.DEFAULT;
                    addToList(Type._string, token.Substring(0, token.Length - 1));
                    token = "";
                }
            }
        }

        //Developer function, adds a specific item to the lexer list by hand.
        public void addToList(Type type, string item="")
        {
            lexerList.Add(new stork.LexerItem(type, item));
        }

        //Private lexer properties.
        string token = "";
        public List<LexerItem> lexerList = new List<LexerItem>();
        LexerState lexerState;
    }
}
