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

        //The global scope collection for an instance of Stork.
        public static StorkScopeCollection scope = new StorkScopeCollection();
        public static StorkTypeCollection types = new StorkTypeCollection();

        //The base "compile unit" node of the entire program.
        public override object VisitCompileUnit(storkParser.CompileUnitContext context)
        {
            Console.WriteLine("Begin evaluation...");

            //Visit the block statement within the compile unit.
            VisitBlock(context.block());

            return null;
        }

        //When the block within the base compile unit is visited.
        public override object VisitBlock([NotNull] storkParser.BlockContext context)
        {
            //For every statement, evaluate them one by one.
            foreach (var statement in context.statement())
            {
                VisitStatement(statement);
            }

            //Returning null, blocks don't return anything.
            return null;
        }

        //When a statement is visited.
        public override object VisitStatement([NotNull] storkParser.StatementContext context)
        {
            //Check what type of statement it is.
            if (context.stat_assign()!=null)
            {
                //A variable assign statement.
                VisitStat_assign(context.stat_assign());
            }
            else if (context.stat_define()!=null)
            {
                //A variable define statement.
                VisitStat_define(context.stat_define());
            }
            else if (context.stat_functionCall()!=null)
            {
                //A function call statement.
                VisitStat_functionCall(context.stat_functionCall());
            }

            //Return null, statements don't return anything.
            return null;
        }

        //When an "assign" statement is visited.
        public override object VisitStat_assign([NotNull] storkParser.Stat_assignContext context)
        {
            //Check whether a variable of this name already exists.
            var identiValue = scope.GetVariable(context.IDENTIFIER().GetText());
            if (identiValue.Equals(default(StorkValue)))
            {
                //Variable doesn't exist. Provide a detailed and simple error message.
                Console.WriteLine("Stork Runtime Error: You tried to assign a value to variable \"" + context.IDENTIFIER().GetText() + "\", but it hadn't been created or doesn't exist.");
                Environment.Exit(0);
            }

            //Variable does exist, get the value to assign.
            var value = (StorkValue)VisitExpr(context.expr());

            //Check if the types are the same.
            if (value.Type.Name != identiValue.Type.Name)
            {
                //Not the same, error and notify user.
                Console.WriteLine("Stork Runtime Error: The variable \"" + context.IDENTIFIER().GetText() + "\" has type " + identiValue.Type.ToString() + ", but you tried to assign it a type of " + value.Type.ToString() + ".");
                Environment.Exit(0);
            }

            //They are, now set the value.
            scope.SetVariable(context.IDENTIFIER().GetText(), value.Value);

            return null;
        }

        //When a "define" statement is visited.
        public override object VisitStat_define([NotNull] storkParser.Stat_defineContext context)
        {
            //Check whether a variable of this name already exists.
            var identiValue = scope.GetVariable(context.varname.Text);
            if (!identiValue.Equals(default(StorkValue)))
            {
                //Variable exists already. Provide a detailed and simple error message.
                Console.WriteLine("Stork Runtime Error: You tried to create a variable \"" + context.varname.Text + "\", but it already exists in scope.");
                Environment.Exit(0);
            }

            //Check if the type of variable is valid.
            var varType = types.GetTypeByShortname(context.vartype.Text);
            if (varType.Equals(default(StorkType)))
            {
                //Type is invalid.
                Console.WriteLine("Stork Runtime Error: The type you provided while creating variable " + context.varname.Text + ", \"" + context.vartype.Text + "\", doesn't exist.");
                Environment.Exit(0);
            }

            //Variable does exist, get the value to assign.
            //Temporarily disabled value fetching.
            var value = new StorkValue() { Type = types.GetTypeByShortname("int"), Value = 3 }; //(StorkValue)VisitExpr(context.expr());

            //They are, now set the value.
            scope.CreateVariable(context.varname.Text, value.Value, varType);

            return null;
        }
    }
}