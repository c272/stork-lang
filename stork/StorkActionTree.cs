﻿using System;
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
            number_literal
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

        //convertList function, makes the lexer list into an action tree.
        public void convertList()
        {
            for (int i=0; i<lexerList.Count; i++)
            {
                //Switching for the type of list.
                switch (lexerList[i].type)
                {
                    case Type.accessor_symbol:
                        break;
                    case Type.addition_operator:
                        break;
                    case Type.binary_and:
                        break;
                    case Type.binary_or:
                        break;
                    case Type.block_close:
                        break;
                    case Type.block_open:
                        break;
                    case Type.boolean_literal:
                        break;
                    case Type.elseif_statement:
                        break;
                    case Type.endline:
                        break;
                    case Type.equals:
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

                        //Find where the statement ends.
                        int pos = i+2;
                        while (true)
                        {
                            try
                            {
                                if (lexerList[pos].type == Type.statement_close)
                                {
                                    break;
                                }
                                pos++;
                            } catch
                            {
                                //No block close.
                                StorkError.printError(StorkError.Error.expected_statement_close);
                                break;
                            }
                        }

                        //Found the end of the block. See if the statement can be evaluated.
                        for (int j=i; j<pos; j++)
                        {
                            if (new[] {Type.float_literal,Type.int_literal,Type.boolean_literal,Type.binary_and,Type.binary_or,Type.unknown_identifier}.Contains(lexerList[j].type))
                            {
                                //Yes, valid.
                                //If not literal, check if it is first, and add to list.
                                if (lexerList[j].type==Type.unknown_identifier)
                                {
                                    //Attempt to cast to literal.
                                    if(StorkLexer.findLiteral(lexerList[j].item, true))
                                    {
                                        //Is literal, add to actionlist as literal.
                                        addItem(Action.number_literal, lexerList[j].item);
                                    } else
                                    {
                                        //No literal, check if a variable has previously been created under that name.
                                        checkVariableExists(lexerList[j].item);
                                    }
                                }
                            } else
                            {
                                //No, invalid identifier detected. Error.
                                StorkError.printError(StorkError.Error.invalid_statement);
                                break;
                            }
                        }

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

        
    }
}
