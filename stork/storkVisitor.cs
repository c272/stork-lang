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
            

            return null;
        }

    }
}