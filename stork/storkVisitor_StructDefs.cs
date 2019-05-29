using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Misc;

namespace stork
{
    public partial class storkVisitor
    {
        //When a function is defined in Stork.
        public override object VisitStat_functionDef([NotNull] storkParser.Stat_functionDefContext context)
        {
            //Get a StorkFunction from the current context.
            StorkFunction func = GetFunctionFromContext(context);

            //Does this function already exist in global scope?
            if (!Functions.GetFunction(context.IDENTIFIER().GetText()).Equals(default(StorkFunction)))
            {
                StorkError.Print("A function with this name already exists, and cannot be defined again.");
                return null;
            }

            //Add the function to global scope.
            Functions.AddFunction(context.IDENTIFIER().GetText(), func.StatementBody, func.Parameters);
            return null;
        }

        //Returns a function data object (StorkFunction) from a definition context.
        public StorkFunction GetFunctionFromContext(storkParser.Stat_functionDefContext context)
        {
            //Evaluating function parameters.
            var params_ = new Dictionary<string, StorkClass>();
            foreach (var param in context.funcdefparams().typeparam())
            {
                //Does this class exist? Grab it.
                var classTemplate = Classes.GetClass(param.typename.Text);
                if (classTemplate == null)
                {
                    StorkError.Print("Function parameter '" + param.paramname.Text + "' has invalid/nonexistant type '" + param.typename.Text + ".");
                    return null;
                }

                //Got the type, now add to params.
                params_.Add(param.paramname.Text, classTemplate);
            }

            //Return the new function.
            return new StorkFunction(context.statement().ToList(), params_);
        }

        //When a class is defined in Stork.
        public override object VisitStat_classDef([NotNull] storkParser.Stat_classDefContext context)
        {
            //Check if this class already exists.
            if (Classes.GetClass(context.IDENTIFIER().GetText())!=null)
            {
                StorkError.Print("A class with this name already exists, can't redefine it.");
                return null;
            }

            //Class doesn't exist. Evaluate fields, functions etc. then create.
            var Fields = new Dictionary<string, StorkClassInstance>();
            foreach (var field in context.stat_define())
            {
                //Get the class template for this field.
                var classTemplate = Classes.GetClass(field.vartype.Text);
                if (classTemplate==null)
                {
                    StorkError.Print("Invalid type '" + field.vartype.Text + "' given for field '" + field.varname.Text + "' in class '" + context.IDENTIFIER().GetText() + "'.");
                    return null;
                }

                //Evaluate the expression on the right.
                var evaluated_expr = (StorkClassInstance)VisitExpr(field.expr());

                //Check if the class instance and the field types are the same.
                if (classTemplate.Name != evaluated_expr.TypeName)
                {
                    //Not the same, error out!
                    StorkError.Print("You tried to assign the field '" + field.varname.Text + "', however you attempted to assign type '" + evaluated_expr.TypeName + "' whereas the field is of type '" + field.vartype.Text + "'.");
                    return null;
                }

                //Add field.
                Fields.Add(field.varname.Text, evaluated_expr);
            }

            return null;
        }
    }
}
