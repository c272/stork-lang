using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stork
{
    public static class StorkOperators
    {
        //Adds two stork class instances.
        public static StorkClassInstance Add(StorkClassInstance left, StorkClassInstance right)
        {
            //Are they "addable"?
            object toReturn;
            string classType = "";
            switch (left.TypeName)
            {
                case "Stork.String":
                    //Adding the two strings, getting result.
                    toReturn = (string)left.DirectValue + (string)right.DirectValue;
                    classType = "str";
                    break;

                case "Stork.Float":
                    //Adding the two floats, getting result.
                    toReturn = (float)left.DirectValue + (float)right.DirectValue;
                    classType = "flt";
                    break;

                case "Stork.Integer":
                    //Adding the two integers, getting result.
                    toReturn = (int)left.DirectValue + (int)right.DirectValue;
                    classType = "int";

                    break;
                default:
                    StorkError.Print("Cannot perform an add on types '" + left.TypeName + "' and '" + right.TypeName + ", they are not operable.");
                    return null;
            }

            //Getting an instance to return.
            var result = storkVisitor.Classes.CreateInstanceDV(classType, toReturn);
            if (!result.Item1)
            {
                StorkError.Print(result.Item2);
                return null;
            }

            return result.Item3;
        }
    }
}
