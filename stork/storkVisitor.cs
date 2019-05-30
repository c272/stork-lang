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
        //When the compile unit of the project is visited.
        public override object VisitCompileUnit([NotNull] storkParser.CompileUnitContext context)
        {
            //Visit the block to loop statements.
            return null;
        }
    }
}