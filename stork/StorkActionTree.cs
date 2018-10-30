using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stork
{
    class StorkActionTree
    {
        //Private property definitions here.
        public List<LexerItem> lexerList = new List<LexerItem>();
        public List<ActionItem> actionTree = new List<ActionItem>();
        public List<string> variableList = new List<string>();
        public List<string> variableValues = new List<string>();
        //The array of all evaluable types.
        Type[] evaluables = { Type.statement_open, Type.statement_close, Type.float_literal, Type.int_literal, Type.boolean_literal, Type.equals, Type.binary_and, Type.binary_or, Type.unknown_identifier };

        //Constructor, takes in a lexerList.
        public StorkActionTree(List<LexerItem> list, bool auto = true)
        {
            lexerList = list;

            //Running the converter automatically, unless otherwise specified.
            if (auto) { convertList(); }
        }

        //An action, for the interpreter to carry out.
        public enum Action
        {
            noaction,
            check_if,
            check_if_end,
            number_literal,
            variable,
            check_equals,
            statement_open,
            statement_close,
            check_bin_or,
            check_bin_and,
            block_open,
            block_close,
            access_property,
            iterate_operator,
            addition_operator,
            check_elseif,
            check_elseif_end,
            set_equal
        }

        public class ActionItem
        {
            //Basic constructor. Item contents are not required, so are an optional parameter.
            public ActionItem(Action _act, string _item = "")
            {
                act = _act;
                item = _item;
            }

            //Properties.
            public string item = "";
            public Action act;
        }

        //checkNext, checks if the next element in lexerList is of a certain type.
        public bool checkNext(int i, Type type)
        {
            if (lexerList[i + 1].type==type)
            {
                return true;
            } else
            {
                return false;
            }
        }

        //Adds item to actionTree.
        public void addItem(Action act, string cont="")
        {
            actionTree.Add(new ActionItem(act, cont));
        }

        //Checks if a variable exists.
        public bool checkVariableExists(string name)
        {
            if (variableList.Contains(name))
            {
                return true;
            } else
            {
                return false;
            }
        }

        public bool findLiteral(string token)
        {
            //Trimming spaces from the token. If it's a literal, it won't have spaces.
            token = token.Replace(" ", "");

            //Reached the end of a line, checking for literals.
            bool foundLiteral = false;
            try
            {
                float.Parse(token.Substring(0, token.Length));
                foundLiteral = true;
            }
            catch (Exception)
            {
                //Not a float, try integer.
                try
                {
                    int.Parse(token.Substring(0, token.Length));
                    foundLiteral = true;
                }
                catch (Exception) { }
            }

            return foundLiteral;
        }

        //convertList function, makes the lexer list into an action tree.
        public void convertList()
        {
            for (int i=0; i<lexerList.Count; i++)
            {
                //Switching for the type of list.
                switch (lexerList[i].type)
                {
                    case Type.accessor_symbol:
                        addItem(Action.access_property);
                        break;
                    case Type.addition_operator:
                        if (checkNext(i, Type.addition_operator))
                        {
                            //Double plus, so the iterate operator.
                            addItem(Action.iterate_operator);
                        } else{
                            addItem(Action.addition_operator);
                        }
                        break;
                    case Type.binary_and:
                    case Type.binary_or:
                        StorkError.printError(StorkError.Error.syntax_error_identifier);
                        break;
                    case Type.block_close:
                        addItem(Action.block_close);
                        break;
                    case Type.block_open:
                        addItem(Action.block_open);
                        break;
                    case Type.boolean_literal:
                        break;
                    case Type.elseif_statement:
                        //Carry out the same process as the if statement, but with different headers.
                        addItem(Action.check_elseif);
                        genericIfChecker(ref i);
                        addItem(Action.check_elseif_end);
                        break;
                    case Type.endline:
                        break;
                    case Type.equals:
                        //Checking if it's a double equals.
                        if (lexerList[i+1].type==Type.equals)
                        {
                            //Throw syntax error, it's a check outside of a comparison.
                            StorkError.printError(StorkError.Error.check_outside_comparison);
                        } else
                        {
                            //Add to list.
                            addItem(Action.set_equal);
                        }
                        break;
                    case Type.float_literal:
                        break;
                    case Type.for_statement:
                        break;
                    case Type.if_statement:
                        //Checking if next element is of type "statement_open".
                        if(!checkNext(i, Type.statement_open))
                        {
                            //Is not, throw error.
                            StorkError.printError(StorkError.Error.expected_statement);
                            break;
                        }

                        //Passed check, add if statement to action list.
                        addItem(Action.check_if);
                        genericIfChecker(ref i);
                        addItem(Action.check_if_end);

                        break;
                    case Type.int_literal:
                        break;
                    case Type.less_than:
                        break;
                    case Type.minus_operator:
                        break;
                    case Type.more_than:
                        break;
                    case Type.preprocess_directive:
                        break;
                    case Type.preprocess_identifier:
                        break;
                    case Type.statement_close:
                        break;
                    case Type.statement_open:
                        break;
                    case Type.unknown_identifier:
                        break;
                    case Type.variable_identifier:
                        break;
                    case Type.while_statement:
                        break;
                    case Type._string:
                        break;
                    default:
                        break;
                    
                }
            }
        }

        private void genericIfChecker(ref int i)
        {
            //Find where the statement ends.
            int pos = i + 2;
            while (true)
            {
                try
                {
                    if (lexerList[pos].type == Type.statement_close)
                    {
                        break;
                    }
                    pos++;
                }
                catch
                {
                    //No block close.
                    StorkError.printError(StorkError.Error.expected_statement_close);
                    break;
                }
            }

            //Found the end of the block. See if the statement can be evaluated.
            for (int j = i + 1; j < pos; j++)
            {
                Console.WriteLine(lexerList[j].type);
                if (evaluables.Contains(lexerList[j].type))
                {
                    //Yes, valid.
                    //If not literal, check if it is first, and add to list.
                    if (lexerList[j].type == Type.unknown_identifier)
                    {
                        //Attempt to cast to literal.
                        if (findLiteral(lexerList[j].item))
                        {
                            //Is literal, add to actionlist as literal.
                            addItem(Action.number_literal, lexerList[j].item);
                        }
                        else
                        {
                            //No literal, check if a variable has previously been created under that name.
                            if (!checkVariableExists(lexerList[j].item))
                            {
                                StorkError.printError(StorkError.Error.syntax_error_identifier);
                            }
                            else
                            {
                                addItem(Action.variable, lexerList[j].item);
                            }
                        }
                        //Checking for other special characters that are not literals.
                    }
                    else if (lexerList[j].type == Type.equals && lexerList[j + 1].type == Type.equals)
                    {
                        addItem(Action.check_equals);
                        j++;
                    }
                    else if (lexerList[j].type == Type.statement_open)
                    {
                        if (lexerList[j - 1].type != Type.if_statement && lexerList[j-1].type!=Type.elseif_statement)
                        {
                            addItem(Action.statement_open);
                        }
                    }
                    else if (lexerList[j].type == Type.statement_close)
                    {
                        addItem(Action.statement_close);
                    }
                    else if (lexerList[j].type == Type.binary_or)
                    {
                        addItem(Action.check_bin_or);
                    }
                    else if (lexerList[j].type == Type.binary_and)
                    {
                        addItem(Action.check_bin_and);
                    }
                }
                else
                {
                    //No, invalid identifier detected. Error.
                    StorkError.printError(StorkError.Error.invalid_statement);
                    break;
                }
            }

            //Ending if.
            //Skipping to after pos.
            i = pos;
        }
    }
}
