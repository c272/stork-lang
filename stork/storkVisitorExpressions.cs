using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Misc;

namespace stork
{
    //The "expressions and value manipulation" section of the Stork language evaluator.
    public partial class storkVisitor : storkBaseVisitor<object>
    {
        public override object VisitExpr([NotNull] storkParser.ExprContext context)
        {
            //Does the expression contain an operator?
            if (context.@operator() != null)
            {
                //Yep, evaluate the right and left of the operator and then operate.
                var leftValue = (StorkClassInstance)VisitExpr(context.lexpr);
                var rightValue = (StorkClassInstance)VisitExpr(context.rexpr);

                //Check the two values are of the same type.
                if (leftValue.TypeName != rightValue.TypeName)
                {
                    StorkError.Print("Cannot use operator on two different types (Attempted on '" + leftValue.TypeName + "' and '" + rightValue.TypeName + "'.");          
                    return null;
                }

                //Switch on the operator.
                StorkClassInstance returnValue = default(StorkClassInstance);
                if (context.@operator().ADD_OP() != null)
                {
                    returnValue = StorkOperators.Add(leftValue, rightValue);
                }
                else if (context.@operator().DIV_OP() != null)
                {

                }
                else if (context.@operator().MUL_OP() != null)
                {

                }
                else if (context.@operator().TAKE_OP() != null)
                {

                }

                //Returning instanc.e
                return returnValue;
            }

            //No operator, so just evaluate the value and return.
            return (StorkClassInstance)VisitValue(context.value());
        }

        //When a value is visited in Stork.
        public override object VisitValue([NotNull] storkParser.ValueContext context)
        {
            //Is the value a function call?
            if (context.stat_functionCall() != null)
            {
                //Evaluate function call and return.
                return (StorkClassInstance)VisitStat_functionCall(context.stat_functionCall());
            }

            //Is the value an object reference? If so, evaluate.
            if (context.object_reference() != null)
            {
                var instance = (StorkClassInstance)VisitObject_reference(context.object_reference());
                Console.WriteLine("INSTANCE: ");
                Console.WriteLine(instance.TypeName);
                return instance;
            }

            //Must be a raw value. Evaluate and return.
            Tuple<bool, string, StorkClassInstance> result = null;
            if (context.BOOLEAN() != null)
            {
                result = Classes.CreateInstanceDV("bln", bool.Parse(context.BOOLEAN().GetText()));
            }
            else if (context.FLOAT() != null)
            {
                result = Classes.CreateInstanceDV("flt", float.Parse(context.FLOAT().GetText()));
            }
            else if (context.INTEGER() != null)
            {
                result = Classes.CreateInstanceDV("int", int.Parse(context.INTEGER().GetText()));
            }
            else if (context.STRING() != null)
            {
                result = Classes.CreateInstanceDV("str", context.STRING().GetText().Substring(1, context.STRING().GetText().Length - 2));
            } else
            {
                //Unrecognised raw.
                StorkError.Print("Unrecognized raw value passed to expression parser. Submit an issue on GitHub with your code if possible.");
                return null;
            }

            if (!result.Item1)
            {
                //Failed to get value, print the error and terminate.
                StorkError.Print(result.Item2);
                return null;
            }

            return result.Item3;
        }

        //When an object reference is visited to be evaluated.
        public override object VisitObject_reference([NotNull] storkParser.Object_referenceContext context)
        {
            //Get the individual parts of the reference as dictionaries of indexes.
            var funcCalls = new Dictionary<int, storkParser.Stat_functionCallContext>();
            var identifiers = new Dictionary<int, string>();
            for (int i=0; i<context.object_subreference().Length; i++)
            {
                //Get a reference to this subexpression.
                var ref_ = context.object_subreference()[i];
                if (ref_.stat_functionCall() != null)
                {
                    //It's a function call, add to the dict.
                    funcCalls.Add(i, ref_.stat_functionCall());
                } else
                {
                    //Must be an identifier, push to dictionary.
                    identifiers.Add(i, ref_.IDENTIFIER().GetText());
                }
            }

            //What's the depth of the reference?
            int depth = funcCalls.Count + identifiers.Count;

            //Traverse down through the class instances.
            StorkClassInstance currentInstance = null;
            for (int i=0; i<depth; i++)
            {
                //Check if currentInstance is null, and i!=0.
                if (currentInstance==null && i!=0)
                {
                    //Didn't find a property to go into. Was it a function call?
                    if (funcCalls.ContainsKey(i-1))
                    {
                        //Yes.
                        StorkError.Print("The function '" + funcCalls[i - 1].IDENTIFIER().GetText() + "' did not return a value to traverse into while referencing an object.");
                        return null;
                    }

                    //No, identifier.
                    StorkError.Print("No variable/field '" + identifiers[i - 1] + "' found to traverse into on object search.");
                    return null;
                }

                if (funcCalls.ContainsKey(i))
                {
                    //It's a function call, set the current instance as the result of that.
                    currentInstance = (StorkClassInstance)VisitStat_functionCall(funcCalls[i]);
                    continue;
                }

                //Not a function call, must be an identifier.
                //If i==0, we're at the top of the depth, so search globally.
                if (i == 0)
                {
                    //Is there a type with this name?
                    if (Classes.GetClass(identifiers[i]) != null)
                    {
                        //Yes, use that.
                        currentInstance = Classes.GetClass(identifiers[i]).StaticInstance;
                        continue;
                    }

                    //First depth, so search global space.
                    currentInstance = Scope.GetVariable(identifiers[i]);
                } else
                {
                    //Not first depth, so search inside the current instance.
                    currentInstance = currentInstance.GetField(identifiers[i]);
                }
            }

            //Found the instance required, return it.
            return currentInstance;
        }
    }
}
