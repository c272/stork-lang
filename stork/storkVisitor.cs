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
            return base.VisitStat_assign(context);
        }

        //When a variable definition is visited.
        public override object VisitStat_define([NotNull] storkParser.Stat_defineContext context)
        {
            //Is the type of the variable valid?
            var type = Classes.GetClass(context.vartype.Text);
            if (type == default(StorkClass))
            {
                StorkError.Print("The type '" + context.vartype.Text + "' you gave for a variable doesn't exist.");
                Environment.Exit(0);
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
                Environment.Exit(0);
                return null;
            }

            //Done.
            return null;
        }
    }
}