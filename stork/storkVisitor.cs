using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stork
{
    public class storkVisitor : storkBaseVisitor<object>
    {
        //When the "Statement" node is visited.
        public override object VisitCompileUnit(storkParser.CompileUnitContext context)
        {
            return null;
        }
    }
}