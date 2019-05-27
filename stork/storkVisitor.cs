using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Misc;

namespace stork
{
    public partial class storkVisitor : storkBaseVisitor<object>
    {
        //List of classes in Stork.
        public static StorkClassCollection Classes = new StorkClassCollection();
        //Variable scopes in Stork.
        public static StorkScope Scope = new StorkScope();
        //Global functions in Stork.
        public static StorkFunctionCollection Functions = new StorkFunctionCollection();

        //When the compile unit of the project is visited.
        public override object VisitCompileUnit([NotNull] storkParser.CompileUnitContext context)
        {
            //Visit the block tp loop statements.
            VisitBlock(context.block());
            return null;
        }

        //When a block is visited.
        public override object VisitBlock([NotNull] storkParser.BlockContext context)
        {
            //Enumerate and visit all statements, one by one.
            foreach (var stat in context.statement())
            {
                VisitStatement(stat);
            }

            return null;
        }

        //When a single statement is visited.
        public override object VisitStatement([NotNull] storkParser.StatementContext context)
        {
            //What type of statement is it?
            if (context.stat_assign() != null)
            {
                //Variable direct assignment.
                VisitStat_assign(context.stat_assign());
            }
            else if (context.stat_define() != null)
            {
                //Variable creation and assignment.
                VisitStat_define(context.stat_define());
            }
            else if (context.stat_functionCall() != null)
            {
                //Function call.
                VisitStat_functionCall(context.stat_functionCall());
            }
            else if (context.stat_functionDef() != null)
            {
                //Function definition.
                VisitStat_functionDef(context.stat_functionDef());
            }

            return null;
        }

        //When a variable assignment is visited.
        public override object VisitStat_assign([NotNull] storkParser.Stat_assignContext context)
        {
            //Evaluate what's being set.
            var instanceToSet = (StorkClassInstance)VisitObject_reference(context.object_reference());

            //Set the object's Direct Value to the evaluated expression.
            instanceToSet.DirectValue = ((StorkClassInstance)VisitExpr(context.expr())).DirectValue;
            return null;
        }

        //When a variable definition is visited.
        public override object VisitStat_define([NotNull] storkParser.Stat_defineContext context)
        {
            //Is the type of the variable valid?
            var type = Classes.GetClass(context.vartype.Text);
            if (type == default(StorkClass))
            {
                StorkError.Print("The type '" + context.vartype.Text + "' you gave for a variable doesn't exist.");
                return null;
            }

            //Yes, evaluate the expression on the right into a value.
            var value = (StorkClassInstance)VisitExpr(context.expr());

            //Add the variable into the current scope.
            Tuple<bool, string> success = Scope.AddVariable(context.varname.Text, value);

            //If it didn't work, error and print why.
            if (!success.Item1)
            {
                StorkError.Print(success.Item2);
                return null;
            }

            //Done.
            return null;
        }

        //When a function call is visited in Stork.
        public override object VisitStat_functionCall([NotNull] storkParser.Stat_functionCallContext context)
        {
            //Find the function in the global scope.
            var func = Functions.GetFunction(context.IDENTIFIER().GetText());
            if (func.Equals(default(StorkFunction)))
            {
                StorkError.Print("The function '" + context.IDENTIFIER().GetText() + "' does not exist.");
                return null;
            }

            //Check if the provided parameters are the same length.
            if (context.@params().expr().Length != func.Parameters.Count)
            {
                StorkError.Print("Invalid amount of parameters for function '" + context.IDENTIFIER().GetText() + "', expected " + func.Parameters.Count + " but got " + context.@params().expr().Length + ".");
                return null;
            }

            //Evaluate the function parameters.
            var params_ = new List<StorkClassInstance>();
            foreach (var param in context.@params().expr())
            {
                params_.Add((StorkClassInstance)VisitExpr(param));
            }

            //Checking if the types of the parameters are the same.
            for (int i = 0; i < func.Parameters.Count; i++)
            {
                //The wildcard class ("val" -> "Stork.Wildcard") means accept any.
                if (func.Parameters.Values.ElementAt(i).Name != params_[i].TypeName && 
                    func.Parameters.Values.ElementAt(i).Name != "Stork.Wildcard")
                {
                    StorkError.Print("Parameter " + (i + 1) + " in calling function '" + context.IDENTIFIER().GetText() + "' is not of the correct type.\n(Got " + params_[i].TypeName + ", expecting " + func.Parameters.Values.ElementAt(i).Name + ").");
                    return null;
                }
            }

            //Is the function internal?
            if (func.Internal != null)
            {
                //Yes, so call with the parameter list.
                return func.Internal(params_);
            }

            //Nope, so eval all the statements one by one.
            //..
            Console.WriteLine("non internal not set up yet so null returned, oopsie");
            return null;
        }
    }
}