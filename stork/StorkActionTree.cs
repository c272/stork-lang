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
        public Dictionary<string, int> functionDictionary = new Dictionary<string, int>();
        bool inFunctionParameters = false;

        //The array of all evaluable types.
        Type[] evaluables = { Type.statement_open, Type.statement_close, Type.float_literal, Type.int_literal, Type.boolean_literal, Type.equals, Type.binary_and, Type.binary_or, Type.unknown_identifier };
        //The array of all literal types.
        Type[] literals = { Type.int_literal, Type.float_literal, Type.boolean_literal, Type.unknown_identifier};
        //The array of all possible separators.
        Type[] separators = { Type.parameter_separator, Type.accessor_symbol };

        //Constructor, takes in a lexerList.
        public StorkActionTree(List<LexerItem> list, bool auto = true)
        {
            lexerList = list;

            // WARNING WARNING WARNING //
            // ADDING EXAMPLE DEBUG FUNCTION, REMOVE THIS IF NOT DEBUGGING FUNCTIONS!
            // WARNING WARNING WARNING //
            functionDictionary.Add("example", 0);

            //Running the converter automatically, unless otherwise specified.
            if (auto) { convertList(); }
        }

        //An action, for the interpreter to carry out.
        public enum Action
        {
            noaction,
            check_if,
            check_if_end,
            boolean_literal,
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
            set_equal,
            run_function_param,
            run_function_param_end,
            run_function_noparams
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
                        StorkError.printError(StorkError.Error.syntax_error_identifier, true, "||");
                        break;
                    case Type.block_close:
                        addItem(Action.block_close);
                        break;
                    case Type.block_open:
                        addItem(Action.block_open);
                        break;
                    case Type.boolean_literal:
                        addItem(Action.boolean_literal);
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
                    case Type.for_statement:
                        break;
                    case Type.if_statement:
                        //Checking if next element is of type "statement_open".
                        if(!checkNext(i, Type.statement_open))
                        {
                            //Is not, throw error.
                            StorkError.printError(StorkError.Error.expected_statement, true, "No open bracket after an if statement is declared.");
                            break;
                        }

                        //Passed check, add if statement to action list.
                        addItem(Action.check_if);
                        genericIfChecker(ref i);
                        addItem(Action.check_if_end);
                        break;
                    case Type.int_literal:
                    case Type.float_literal:
                        addItem(Action.number_literal, lexerList[i].item);
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
                        //Check if currently in function parameters.
                        if (inFunctionParameters)
                        {
                            //Yes, so end the function parameter block.
                            addItem(Action.run_function_param_end);
                        } else
                        {
                            //No, just a normal end statement then.
                            addItem(Action.statement_close);
                        }
                        break;
                    case Type.statement_open:
                        break;
                    case Type.unknown_identifier:
                        //Checking if the identifier can be converted to a number literal.
                        if (findLiteral(lexerList[i].item))
                        {
                            //It can, push to list as literal.
                            addItem(Action.number_literal, lexerList[i].item);
                        } else
                        {
                            //No, see if it's a variable.
                            if (variableList.Contains(lexerList[i].item))
                            {
                                //Yes, push to list.
                                addItem(Action.variable, lexerList[i].item);
                            } else
                            {
                                //No, finally see if it's a function or not before calling error.
                                if (functionDictionary.ContainsKey(lexerList[i].item))
                                {
                                    //Is a function, check if it's being called or not.
                                    if (lexerList[i+1].type==Type.statement_open && lexerList[i+2].type==Type.statement_close)
                                    {
                                        //A function call with no parameters, so can immediately push.
                                        addItem(Action.run_function_noparams, lexerList[i].item);
                                        //Increase "i" to the position of the end bracket.
                                        i += 2;
                                    } else if (lexerList[i+1].type==Type.statement_open)
                                    {
                                        //A function call with 1 or more parameters.
                                        //Check where the end of the statement is.
                                        int j = i + 2;
                                        while (true)
                                        {
                                            try
                                            {
                                                if (lexerList[j].type == Type.statement_close)
                                                {
                                                    //End is here.
                                                    break;
                                                }
                                                j++;
                                            } catch
                                            {
                                                StorkError.printError(StorkError.Error.expected_statement_close, true, "No function bracket close after function call.");
                                                break;
                                            }
                                        }

                                        //Found the end, now look for how many parameters there are, and if they're valid.
                                        for (int k = i + 2; k < j; k++)
                                        {
                                            //Checking it's a literal or accessor symbol (.).
                                            if (!literals.Contains(lexerList[k].type) && !separators.Contains(lexerList[k].type))
                                            {
                                                //It isn't, throw error.
                                                StorkError.printError(StorkError.Error.invalid_argument_syntax,true, "Arguments weren't literals, variables, or function variables.");
                                            }
                                        }

                                        //Adding function open statement.
                                        addItem(Action.run_function_param);
                                        inFunctionParameters = true;
                                    }
                                } else
                                {
                                    //Not a function, send it to the error handler.
                                    StorkError.printError(StorkError.Error.syntax_error_identifier, true, "Unknown identifier \""+lexerList[i].item+"\".");
                                }
                            }
                        }
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
                    StorkError.printError(StorkError.Error.expected_statement_close, true, "No end statement on a comparison (if, else, etc.).");
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
                                StorkError.printError(StorkError.Error.syntax_error_identifier, true, "Unknown identifier \""+lexerList[i].item+"\".");
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
                    StorkError.printError(StorkError.Error.invalid_statement,true, "Inevaluable type given (not a variable or a literal), so invalid.");
                    break;
                }
            }

            //Ending if.
            //Skipping to after pos.
            i = pos;
        }
    }
}
