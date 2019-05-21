using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Misc;

namespace stork
{
    public class storkVisitor : storkBaseVisitor<object>
    {
        //The base "compile unit" node of the entire program.
        public override object VisitCompileUnit(storkParser.CompileUnitContext context)
        {
            Console.WriteLine("Begin evaluation...");

            //Evaluate every statement, one by one.
            foreach (var statement in context.block().statement())
            {
                VisitStatement(statement);
            }

            return null;
        }

        //When a statement is visited in Stork.
        public override object VisitStatement([NotNull] storkParser.StatementContext context)
        {
            //What type of statement is it?
            if (!context.stat_assign().IsEmpty)
            {
                //Assign statement.
                VisitStat_assign(context.stat_assign());
                
            }
            else if (!context.stat_functionCall().IsEmpty)
            {
                //Function call statement.
                VisitStat_functionCall(context.stat_functionCall());
            } else
            {
                Console.WriteLine("No statement type found.");
            }

            return null;
        }

        //When an assignment is visited in Stork.
        public override object VisitStat_assign([NotNull] storkParser.Stat_assignContext context)
        {
            //Evaluate the expression, and get a value back.
            StorkValue val = (StorkValue)VisitValue(context.expr().value());
            Console.WriteLine(val.Value.ToString());

            //Set the variable in the current scope to that.
            //...

            return null;
        }

        //When a value is visited in Stork.
        public override object VisitValue([NotNull] storkParser.ValueContext context)
        {
            //Is it simply a raw value?
            if (context.INTEGER() != null)
            {
                //Return a raw integer.
                return new StorkValue()
                {
                    Type = ValueType.INTEGER,
                    Value = context.INTEGER().GetText()
                };
            }
            else if (context.FLOAT() != null)
            {
                //Return a raw float.
                return new StorkValue()
                {
                    Type = ValueType.FLOAT,
                    Value = context.FLOAT().GetText()
                };
            }
            else if (context.STRING() != null)
            {
                //Return a raw string.
                return new StorkValue()
                {
                    Type = ValueType.STRING,
                    Value = context.STRING().GetText().Replace("\"", "")
                };
            }

            //Not a raw value, so it's a function call. Return that instead.
            return new StorkValue()
            {
                Value = "func call"
            };
            return (StorkValue)VisitStat_functionCall(context.stat_functionCall());
        }
    }
}