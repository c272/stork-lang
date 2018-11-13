using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stork
{
    //The variable class. Contains a type property, an access property, and a contents property.
    class Variable
    {
        public Variable(string type_, string contents_, bool public__)
        {
            type = type_;
            contents = contents_;
            public_ = public__;
        }
        public string type;
        public string contents;
        public bool public_;
    }

    //The function class. Contains a function location in the ActionScript, an access property and a parameters property.
    class Function
    {
        public Function(Dictionary<string,StorkActionTree.Action> parameters_, bool public__=true, int location_=-1)
        {
            parameters = parameters_;
            public_ = public__;
            location = location_;
        }
        public Dictionary<string, StorkActionTree.Action> parameters = new Dictionary<string, StorkActionTree.Action>();
        public bool public_;
        int location;
    }

    enum ATState
    {
        DEFAULT,
        IN_FUNC_PARAMS,
        IN_FUNC,
        IN_FOR_LOOP
    }

    class StorkActionTree
    {
        //Private property definitions here.
        public List<LexerItem> lexerList = new List<LexerItem>();
        public static List<ActionItem> actionTree = new List<ActionItem>();
        public static Dictionary<string, Variable> variableDictionary = new Dictionary<string, Variable>();
        public static Dictionary<string, Function> functionDictionary = new Dictionary<string, Function>();
        public static Dictionary<string, string> preprocessQueue = new Dictionary<string, string>();
        ATState actionTreeState = ATState.DEFAULT;
        public int blockLayer = 0;
        public int forLoopStatementPosition = 0;

        //The array of all evaluable types.
        Type[] evaluables = { Type.less_than, Type.more_than, Type.statement_open, Type.statement_close, Type.float_literal, Type.int_literal, Type.boolean_literal, Type.equals, Type.binary_and, Type.binary_or, Type.unknown_identifier };
        //The array of all literal types.
        Type[] literals = { Type.int_literal, Type.float_literal, Type.boolean_literal, Type._string, Type.unknown_identifier };
        //The array of all action types.
        Type[] actions = { Type.unknown_identifier, Type.addition_operator, Type.minus_operator, Type.equals, Type.int_literal, Type.float_literal, Type.boolean_literal, Type._string};
        //The array of all possible separators.
        Type[] separators = { Type.parameter_separator, Type.accessor_symbol };
        //Array of all data types.
        Type[] data_types = { Type.variable_identifier };

        //Constructor, takes in a lexerList.
        public StorkActionTree(List<LexerItem> list, bool auto = true)
        {
            lexerList = list;

            // WARNING WARNING WARNING //
            // ADDING EXAMPLE DEBUG FUNCTION, REMOVE THIS IF NOT DEBUGGING FUNCTIONS!
            // WARNING WARNING WARNING //
            addFunc("example");

            //Running the converter automatically, unless otherwise specified.
            if (auto) { convertList(); }
        }

        //Adds a function to the function dictionary.
        public void addFunc(string name, Dictionary<string, Action> param=null, bool public_=true, int location=-1)
        {
            if (param == null)
            {
                functionDictionary.Add(name, new Function(null));
            } else if (location!=-1)
            {
                functionDictionary.Add(name, new Function(param, public_, location));
            } else 
            {
                functionDictionary.Add(name, new Function(param));
            }
        }

        //An action, for the interpreter to carry out.
        public enum Action
        {
            noaction,
            check_if,
            check_if_end,
            boolean_literal,
            number_literal,
            func_literal,
            string_literal,
            gen_literal,
            variable,
            create_variable,
            create_variable_type,
            create_variable_value,
            create_variable_end,
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
            run_function_noparams,
            create_function,
            function_create,
            function_create_position,
            function_create_unknown_position,
            function_create_end,
            function_end,
            function_start,
            check_less_or_equal,
            check_less,
            check_more_or_equal,
            check_more,
            for_statement_start,
            for_statement_end_init,
            for_statement_start_init,
            for_statement_start_check,
            for_statement_end_check,
            for_statement_start_step,
            for_statement_end_step,
            for_statement_end,
            decrement_operator,
            minus_operator
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
            if (variableDictionary.ContainsKey(name))
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
                            //Skip one.
                            i++;
                        } else {
                            addItem(Action.addition_operator);
                        }
                        break;
                    case Type.binary_and:
                    case Type.binary_or:
                        StorkError.printError(StorkError.Error.syntax_error_identifier, true, "|| or &&");
                        break;
                    case Type.block_close:
                        //Check if currently in a function.
                        if (actionTreeState == ATState.IN_FUNC)
                        {
                            //Yes, is this the outermost block layer?
                            if (blockLayer == 0)
                            {
                                //End func.
                                addItem(Action.function_end);
                                //Resetting state to default.
                                actionTreeState = ATState.DEFAULT;
                            } else
                            {
                                //Not at zero yet, take away block layer.
                                blockLayer--;
                                addItem(Action.block_close);
                            }
                        } else
                        {
                            //No, just add a block_close.
                            addItem(Action.block_close);
                        }
                        break;
                    case Type.block_open:
                        //Checking if in a func.
                        if (actionTreeState == ATState.IN_FUNC)
                        {
                            //Yes, add to block layer.
                            blockLayer++;
                            addItem(Action.block_open);
                        }
                        else
                        {
                            addItem(Action.block_open);
                        }
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
                        //Checking if action tree is in a for loop.
                        if (actionTreeState == ATState.IN_FOR_LOOP)
                        {
                            //Yes, end the current statement.
                            switch (forLoopStatementPosition)
                            {
                                case 0:
                                    addItem(Action.for_statement_end_init);
                                    addItem(Action.for_statement_start_check);
                                    forLoopStatementPosition++;
                                    break;
                                case 1:
                                    addItem(Action.for_statement_end_check);
                                    addItem(Action.for_statement_start_step);
                                    forLoopStatementPosition++;
                                    break;
                                case 2:
                                    addItem(Action.for_statement_end_step);
                                    break;
                                default:
                                    //Throw error, too many statements!
                                    StorkError.printError(StorkError.Error.for_statement_overflow);
                                    break;
                            }
                        }
                        break;
                    case Type.equals:
                        //Checking if it's a double equals.
                        if (lexerList[i + 1].type == Type.equals)
                        {
                            if (actionTreeState == ATState.DEFAULT)
                            {
                                //Equals outside of check.
                                StorkError.printError(StorkError.Error.check_outside_comparison);
                            } else
                            {
                                //Add double equals and skip one.
                                addItem(Action.check_equals);
                                i++;
                            }
                        } else
                        {
                            //Add to list.
                            addItem(Action.set_equal);
                        }
                        break;
                    case Type.for_statement:
                        //Adding "for_statement_start" to ActionTree.
                        addItem(Action.for_statement_start);
                        addItem(Action.for_statement_start_init);

                        //Outlining for loop syntax.
                        // for (TYPE NAME = VALUE; CHECK; ENDLOOPACT) {};
                        if (lexerList[i + 1].type != Type.statement_open)
                        {
                            StorkError.printError(StorkError.Error.expected_statement);
                        } else
                        {
                            //Checking for variable identifier and then an unknown, followed by "= literal".
                            int j = i + 2;
                            while (true)
                            {
                                if (j == i + 2 && lexerList[j].type != Type.variable_identifier)
                                {
                                    //No valid variable identifier.
                                    StorkError.printError(StorkError.Error.no_type_found);
                                }
                                else if (j == i + 3 && lexerList[j].type != Type.unknown_identifier)
                                {
                                    //No variable name given.
                                    StorkError.printError(StorkError.Error.invalid_variable_name);
                                }
                                else if (j == i + 4 && lexerList[j].type != Type.equals)
                                {
                                    //Nope, no equals value detected after init.
                                    StorkError.printError(StorkError.Error.invalid_statement);
                                } else if (j == i + 5 && !literals.Contains(lexerList[j].type))
                                {
                                    //No literal found after equals.
                                    StorkError.printError(StorkError.Error.cannot_assign_variable_value);
                                } else if (j == i + 6 && lexerList[j].type != Type.endline)
                                {
                                    //No endline character found in if loop.
                                    StorkError.printError(StorkError.Error.expected_endline);
                                } else if (j == i + 7)
                                {
                                    break;
                                }

                                j++;
                            }

                            //Now evaluating the "check" statement to be run at the start of each loop.
                            int endStatement = findEndLine(j);
                            if (endStatement == -1)
                            {
                                StorkError.printError(StorkError.Error.expected_endline);
                            }

                            //Checking if the "checkstat" contains valid types.
                            string statString = "";
                            for (int k = j; k < endStatement; k++) {
                                statString += lexerList[k].item;
                                if (!evaluables.Contains(lexerList[k].type))
                                {
                                    //Invalid non-evaluable type found, throw error.
                                    StorkError.printError(StorkError.Error.invalid_statement, true, statString);
                                }
                            }

                            //Contains valid statement, now check loopstart statement.
                            j = endStatement + 1;
                            endStatement = findEndLine(j);
                            if (endStatement == -1)
                            {
                                StorkError.printError(StorkError.Error.expected_endline);
                            }

                            //Checking for valid types.
                            statString = "";
                            for (int k = j; k < endStatement; k++)
                            {
                                statString += lexerList[k].item;
                                if (!actions.Contains(lexerList[k].type))
                                {
                                    //Invalid type.
                                    StorkError.printError(StorkError.Error.invalid_statement, true, statString);
                                }
                            }

                            //Set ATState as inside a for loop.
                            actionTreeState = ATState.IN_FOR_LOOP;
                        }
                        break;
                    case Type.if_statement:
                        //Checking if next element is of type "statement_open".
                        if (!checkNext(i, Type.statement_open))
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
                        //Checking if next character is equals.
                        if (lexerList[i + 1].type == Type.equals)
                        {
                            //Adding a less than/equal to operator.
                            addItem(Action.check_less_or_equal);
                            i++;
                        } else {
                            //Adding a normal operator.
                            addItem(Action.check_less);
                        }
                        break;
                    case Type.more_than:
                        //Checking if next character is equals.
                        if (lexerList[i + 1].type == Type.equals)
                        {
                            //Adding a less than/equal to operator.
                            addItem(Action.check_more_or_equal);
                            i++;
                        }
                        else
                        {
                            //Adding a normal operator.
                            addItem(Action.check_more);
                        }
                        break;
                    case Type.minus_operator:
                        //Checking if the next character is minus.
                        if (checkNext(i, Type.minus_operator))
                        {
                            //Adding a decrement operator.
                            addItem(Action.decrement_operator);
                            i++;
                        } else
                        {
                            addItem(Action.minus_operator);
                        }
                        break;
                    case Type.preprocess_identifier:
                        //Detected a preprocess directive symbol. Checking if the next symbol is a preprocess identifier.
                        if (checkNext(i, Type.preprocess_directive) && literals.Contains(lexerList[i+2].type))
                        {
                            //It is a preprocess directive, so send to the processing class.
                            //Add to the preprocessing queue.
                            preprocessQueue.Add(lexerList[i+1].item, lexerList[i+2].item);

                            //Skip to after the item in the lexerList.
                            i += 2;
                        } else
                        {
                            if (!checkNext(i, Type.preprocess_directive))
                            {
                                //Error, preprocess symbol detected but no identifier/value.
                                StorkError.printError(StorkError.Error.preprocess_no_identifier);
                            }
                            else
                            {
                                //Error, identifier detected but no value.
                                StorkError.printError(StorkError.Error.preprocess_no_value);
                            }
                        }
                        foreach (KeyValuePair<string, string> entry in preprocessQueue)
                        {
                            Console.WriteLine(entry.Value);
                        }
                        break;
                    case Type.preprocess_directive:
                        //Error, preprocess directive should only be after symbol.
                        StorkError.printError(StorkError.Error.syntax_error_identifier, true, lexerList[i].item);
                        break;
                    case Type.statement_close:
                        //Check if currently in function parameters.
                        if (actionTreeState == ATState.IN_FUNC_PARAMS)
                        {
                            //Yes, so end the function parameter block.
                            addItem(Action.run_function_param_end);
                        } else if (actionTreeState == ATState.IN_FOR_LOOP)
                        {
                            //Check if the statement quota for the loop has been met.
                            if (forLoopStatementPosition!=2)
                            {
                                StorkError.printError(StorkError.Error.for_statement_underflow);
                            } else
                            {
                                //Resetting.
                                forLoopStatementPosition = 0;
                                addItem(Action.for_statement_end);
                                actionTreeState = ATState.DEFAULT;
                            }
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
                            if (variableDictionary.ContainsKey(lexerList[i].item))
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
                                    } else if (lexerList[i+1].type==Type.statement_open && lexerList[i-1].type!=Type.function_identifier)
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
                                        actionTreeState = ATState.IN_FUNC_PARAMS;
                                    }
                                } else
                                {
                                    //Not a function, send it to the error handler.
                                    Console.WriteLine("BREAK AT: "+lexerList[i].type + " " + lexerList[i].item);
                                    StorkError.printError(StorkError.Error.syntax_error_identifier, true, "Unknown identifier \""+lexerList[i].item+"\".");
                                }
                            }
                        }
                        break;
                    case Type.variable_identifier:
                        //Found a variable identifier, check if the next token is an unknown.
                        if (checkNext(i, Type.unknown_identifier))
                        {
                            //Yes, this is a single variable instantiation, no array of any sort, so push to variable list and add as an instruction.
                            addItem(Action.create_variable, lexerList[i+1].item);
                            addItem(Action.create_variable_type, lexerList[i].item);
                            //Adding to variable dictionary.
                            addVariable(lexerList[i+1].item, lexerList[i].item, null, true);
                            
                            //Checking if value is set after instantiation.
                            if (lexerList[i+2].type==Type.equals)
                            {
                                //Assuming that it is being set to a value.
                                //Checking if the value is a literal or a variable.
                                if (literals.Contains(lexerList[i+3].type) || variableDictionary.ContainsKey(lexerList[i+3].item))
                                {
                                    //Is a literal/var, add as a "set_value".
                                    addItem(Action.create_variable_value, lexerList[i + 3].item);
                                    //End creation.
                                    addItem(Action.create_variable_end);
                                    i += 3;
                                    break;
                                }
                                else
                                {
                                    //Invalid variable assignment.
                                    StorkError.printError(StorkError.Error.cannot_assign_variable_value, true, lexerList[i+3].item);
                                }
                            } else
                            {
                                //End creation, is null value.
                                addItem(Action.create_variable_end);
                                i++;
                                break;
                            }
                        } else
                        {
                            //Error, invalid syntax for variable creation.
                            StorkError.printError(StorkError.Error.invalid_variable_name);
                        }
                        break;
                    case Type.function_identifier:
                        //Detected a proc identifier, track through to find parameters (if there are any).
                        if (lexerList[i+1].type!=Type.unknown_identifier)
                        {
                            //Error, invalid function definition.
                            StorkError.printError(StorkError.Error.invalid_function_name);
                        } else
                        {
                            //Found the name of the function, now check for statement_open.
                            addItem(Action.function_create, lexerList[i+1].item);
                            if (lexerList[i+2].type==Type.statement_open)
                            {
                                //Found, no errors yet.
                                //Iterate through for parameters.
                                int pos = i + 3;
                                Dictionary<string, StorkActionTree.Action> parameters = new Dictionary<string, Action>();
                                bool isNextParam = true;
                                while (true)
                                {
                                    try
                                    {
                                        //Checking if literal or variable.
                                        if (lexerList[pos].type == Type.variable_identifier)
                                        {
                                            //Found an identifier, check for the name in next position.
                                            if (lexerList[pos + 1].type == Type.unknown_identifier)
                                            {
                                                //Checking if parameter is expected.
                                                if (!isNextParam)
                                                {
                                                    //Error, no parameter separator given.
                                                    StorkError.printError(StorkError.Error.function_missing_parameter_separator);
                                                }

                                                //Name found. Add to parameter list.
                                                Action act = 0;
                                                switch (lexerList[pos].item)
                                                {
                                                    case "int":
                                                    case "float":
                                                        act = Action.number_literal;
                                                        break;
                                                    case "bool":
                                                        act = Action.boolean_literal;
                                                        break;
                                                    case "gen":
                                                        act = Action.gen_literal;
                                                        break;
                                                    case "func":
                                                        act = Action.func_literal;
                                                        break;
                                                    default:
                                                        StorkError.printError(StorkError.Error.invalid_parameter_type);
                                                        break;
                                                }
                                                parameters.Add(lexerList[pos + 1].item, act);

                                                //Checking if the next identifier is a separator.
                                                if (lexerList[pos+2].type==Type.parameter_separator)
                                                {
                                                    isNextParam = true;
                                                } else
                                                {
                                                    isNextParam = false;
                                                }
                                            } 
                                            else
                                            {
                                                //Invalid variable declaration.
                                                StorkError.printError(StorkError.Error.invalid_variable_name, true, lexerList[pos + 1].item);
                                            }
                                        }
                                        //Checking if parameter was asked for but not given.
                                        else if (lexerList[pos-1].type==Type.parameter_separator && parameters.Count > 0 && isNextParam)
                                        {
                                            //Parameter required but not detected.
                                            StorkError.printError(StorkError.Error.function_missing_parameter);
                                        }

                                        //Checking if the position is end of statement.
                                        if (lexerList[pos].type == Type.statement_close)
                                        {
                                            //End of parameter block, break and add all params to a function in the dictionary.
                                            break;
                                        }
                                        pos++;
                                    } catch
                                    {
                                        //Gone out of range with the array, bracket was never closed.
                                        StorkError.printError(StorkError.Error.expected_statement_close);
                                    }
                                }

                                //Finished the parameter loop, check if there is any definition after the break.
                                try
                                {
                                    if (checkNext(pos, Type.block_open))
                                    {
                                        //Is defined, add position and push to dictionary.
                                        addFunc(lexerList[i + 1].item, parameters, true, pos + 1);
                                        addItem(Action.function_create_position, (actionTree.Count + 2).ToString());
                                    } else
                                    {
                                        //Not defined, check if the character is an endline character.
                                        if (checkNext(pos, Type.endline))
                                        {
                                            //Yes, push as function proto with location as -1.
                                            addFunc(lexerList[i + 1].item, parameters, true);
                                            addItem(Action.function_create_unknown_position);
                                        } else
                                        {
                                            //Error, invalid function definition.
                                            StorkError.printError(StorkError.Error.function_proto_nodefine);
                                        }
                                    }
                                    addItem(Action.function_create_end);
                                    addItem(Action.function_start);

                                    //Skipping to the block opening of the func, and setting state to "IN_FUNC".
                                    actionTreeState = ATState.IN_FUNC;
                                    Console.WriteLine(functionDictionary);
                                    i = pos+1;
                                } catch
                                {
                                    //Throw error, line is not ended and no definition.
                                    StorkError.printError(StorkError.Error.function_proto_nodefine);
                                }
                            } else
                            {
                                //Invalid syntax, throw error, all funcs need a parameter list.
                                StorkError.printError(StorkError.Error.expected_statement);
                            }
                        }
                        break;
                    case Type.while_statement:
                        break;
                    case Type._string:
                        //Throw error, can't just have a string without a declaration or other.
                        StorkError.printError(StorkError.Error.syntax_error_identifier, true, lexerList[i].item);
                        break;
                    default:
                        break;
                    
                }
            }
        }

        private int findEndLine(int endStatement)
        {
            while (true)
            {
                try
                {
                    if (lexerList[endStatement].type == Type.endline)
                    {
                        return endStatement;
                    }
                    endStatement++;
                }
                catch
                {
                    //Nope, ran out of range.
                    return -1;
                }
            }
        }

        //Adds a variable to the variable dictionary according to parameters.
        public void addVariable(string name, string type, string contents, bool public_=true)
        {
            variableDictionary.Add(name, new Variable(type, contents, public_));
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
                                StorkError.printError(StorkError.Error.syntax_error_identifier, true, "Unknown identifier \""+lexerList[j].item+"\".");
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
                    else if (lexerList[j].type==Type.less_than)
                    {
                        if (lexerList[j+1].type==Type.equals)
                        {
                            addItem(Action.check_less_or_equal);
                        } else
                        {
                            addItem(Action.check_less);
                        }
                        j++;
                    }
                    else if (lexerList[j].type == Type.more_than)
                    {
                        if (lexerList[j + 1].type == Type.equals)
                        {
                            addItem(Action.check_more_or_equal);
                        }
                        else
                        {
                            addItem(Action.check_more);
                        }
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
                    } else if (lexerList[j].type==Type.float_literal || lexerList[j].type==Type.int_literal)
                    {
                        addItem(Action.number_literal, lexerList[j].item);
                    } else
                    {
                        //Print error, no type found.
                        StorkError.printError(StorkError.Error.no_type_found, true, lexerList[j].type+" -> "+lexerList[j].item);
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
