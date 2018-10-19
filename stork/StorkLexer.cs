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
        unknown_identifier,
        statement_open,
        statement_close,
        block_open,
        block_close,
        variable_identifier,
        if_statement,
        elseif_statement,
        while_statement,
        float_literal,
        int_literal,
        boolean_literal,
        equals
    }

    //"Lexer state" enumeration.
    enum LexerState
    {
        DEFAULT,
        IN_STRING,
        IN_COMMENT
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

        //Search for a literal in the string given.
        public bool findLiteral(string token)
        {
            //Reached the end of a line, checking for literals.
            bool foundLiteral = false;
            try
            {
                float.Parse(token.Substring(0, token.Length - 1));
                foundLiteral = true;
                addToList(Type.float_literal, token.Substring(0, token.Length - 1));
            }
            catch (Exception)
            {
                //Not a float, try integer.
                try
                {
                    int.Parse(token.Substring(0, token.Length - 1));
                    foundLiteral = true;
                    addToList(Type.int_literal, token.Substring(0, token.Length - 1));
                }
                catch (Exception) { }
            }

            return foundLiteral;
        }

        //"Feed" function, which lets the main program feed input into the lexer.
        public void feed(char c)
        {
            //Incrementing token.
            token += c;

            //Checking if currently in default state (not in string/comment etc.)
            if (lexerState == LexerState.DEFAULT)
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
                    case "(":
                        //Open statement character detected, add to list and reset.
                        addToList(Type.statement_open);
                        token = "";
                        break;
                    case ")":
                        //Close statement character detected.
                        addToList(Type.statement_close);
                        token = "";
                        break;
                    case "{":
                        //Open block character detected, add to list.
                        addToList(Type.block_open);
                        token = "";
                        break;
                    case "}":
                        //Close block character detected, add to list.
                        addToList(Type.block_close);
                        token = "";
                        break;
                    case "=":
                        //Equals character detected, push.
                        addToList(Type.equals);
                        token = "";
                        break;
                    case "//":
                        //Comment start detected, set state.
                        lexerState = LexerState.IN_COMMENT;
                        token = "";
                        break;
                    case "if ":
                        //If statement detected.
                        addToList(Type.if_statement);
                        token = "";
                        break;
                    case "int ":
                    case "float ":
                    case "bool ":
                    case "gen ":
                        //Variable keyword detected. Add to list.
                        addToList(Type.variable_identifier, token.Substring(0, token.Length - 1));
                        token = "";
                        break;
                    case "true":
                    case "false":
                        //Boolean literal detected. Add to list.
                        addToList(Type.boolean_literal, token);
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
                        //Checking if current char is a space or a function call bracket.
                        if (c==' ')
                        {
                            //Yes, we're at the end of a keyword and it's unrecognised.
                            //Assume it's a variable/function identifier, and push.
                            addToList(Type.unknown_identifier, token.Substring(0, token.Length - 1));
                            token = "";
                        } else if (c=='(')
                        {
                            //Check for if/else if/while statements, etc.
                            bool foundI = false;
                            string tok = token.Substring(0, token.Length - 1);
                            switch (tok)
                            {
                                case "if":
                                    addToList(Type.if_statement);
                                    foundI = true;
                                    break;
                                case "else if":
                                    addToList(Type.elseif_statement);
                                    foundI = true;
                                    break;
                                case "while":
                                    addToList(Type.while_statement);
                                    foundI = true;
                                    break;
                            }

                            //If not found, assume unknown identifier.
                            if (!foundI)
                            {
                                addToList(Type.unknown_identifier, token.Substring(0, token.Length - 1));
                            }
                            addToList(Type.statement_open);
                            token = "";
                        } else if (c==')')
                        {
                            //Check literal.
                            bool foundLiteral = findLiteral(token);

                            //Assume end of function call.
                            if (!foundLiteral)
                            {
                                addToList(Type.unknown_identifier, token.Substring(0, token.Length - 1));
                            }
                            addToList(Type.statement_close);
                            token = "";
                        } else if (c=='=')
                        {
                            //Checking for literals.
                            bool foundLiteral = findLiteral(token);
                            if (!foundLiteral) {
                                //Assuming unknown identifier.
                                addToList(Type.unknown_identifier, token.Substring(0, token.Length - 1));
                            }

                            //Adding an equals.
                            addToList(Type.equals);
                            //Resetting token.
                            token = "";
                        }

                        //Checking if character is an end of line.
                        if (c == ';')
                        {
                            bool foundLiteral = findLiteral(token);

                            //Checking if literal has been found.
                            if (!foundLiteral)
                            {
                                //No literal found.
                                //Assuming it's some unknown identifier, and marking as such.
                                addToList(Type.unknown_identifier, token.Substring(0, token.Length - 1));
                            }

                            //Add an endline.
                            addToList(Type.endline);

                            //Resetting token.
                            token = "";
                        }

                        //Unknown symbol/token or is unfinished, so skip it.
                        break;
                }
            } else
            {
                //Checking the state of the lexer.
                if (lexerState == LexerState.IN_STRING)
                {
                    //Yeah, inside a string. If the character isn't a quote closing it,
                    //ignore everything and just keep adding to token.
                    if (c == '\"')
                    {
                        //Return to default state, reset token and push to list.
                        lexerState = LexerState.DEFAULT;
                        addToList(Type._string, token.Substring(0, token.Length - 1));
                        token = "";
                    }
                } else if (lexerState == LexerState.IN_COMMENT)
                {
                    //In a comment, ignore unless token is an end comment.
                    if (token.Length>=2 && token.Substring(token.Length-2)=="//")
                    {
                        //End of comment, return lexer back to normal state and reset token.
                        lexerState = LexerState.DEFAULT;
                        token = "";
                    }
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
