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

                //Switch on the operator.
                if (context.@operator().ADD_OP() != null)
                {

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
            }
        }
    }
}
