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
            var InstanceFields = new Dictionary<string, StorkClass>();
            var StaticFields = new Dictionary<string, StorkClass>();
            foreach (var field in context.class_fieldDefine())
            {

                //Get the class template for this field.
                var classTemplate = Classes.GetClass(field.vartype.Text);
                if (classTemplate==null)
                {
                    StorkError.Print("Invalid type '" + field.vartype.Text + "' given for field '" + field.varname.Text + "' in class '" + context.IDENTIFIER().GetText() + "'.");
                    return null;
                }

                //What type of field is it? Static or instance.
                if (field.STATIC_SYM() != null)
                {
                    //Static.
                    StaticFields.Add(field.varname.Text, classTemplate);
                }
                else
                {
                    //Instance.
                    InstanceFields.Add(field.varname.Text, classTemplate);
                }
            }

            //Evaluating functions.
            var InstanceMethods = new Dictionary<string, StorkFunction>();
            var StaticMethods = new Dictionary<string, StorkFunction>();
            foreach (var method in context.class_functionDef())
            {
                //Getting function definition.
                var funcDef = method.stat_functionDef();

                //Evaluating.
                StorkFunction func = GetFunctionFromContext(funcDef);

                //Is it static or instance?
                if (method.STATIC_SYM() != null)
                {
                    //Static.
                    StaticMethods.Add(funcDef.IDENTIFIER().GetText(), func);
                }
                else
                {
                    //Instance.
                    InstanceMethods.Add(funcDef.IDENTIFIER().GetText(), func);
                }
            }

            //Evaluating class parameters.
            var params_ = new Dictionary<string, StorkClass>();

            //Only run if there are actually any parameters.
            if (context.funcdefparams() != null)
            {
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
            }

            //Grabbing constructor statements, creating function.
            StorkFunction constructorFunc;
            if (context.stat_constructor() == null)
            {
                //Create a blank constructor.
                constructorFunc = new StorkFunction(new List<storkParser.StatementContext>(), params_);
            }
            else
            {
                //Constructor defined.
                constructorFunc = new StorkFunction(context.stat_constructor().statement().ToList(), params_);
            }

            //Creating a class and adding it to the collection.
            StorkClass returnClass = new StorkClass()
            {
                InstanceFields = InstanceFields,
                InstanceMethods = InstanceMethods,
                StaticFields = StaticFields,
                StaticMethods = StaticMethods,
                CanDirectAssign = false,
                DirectValue = null,
                Name = context.IDENTIFIER().GetText(),
                Constructor = constructorFunc
            };

            //Adding, returning.
            Classes.AddClass(returnClass.Name, returnClass);
            return null;
        }
    }
}
