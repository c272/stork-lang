﻿using System;
using System.Collections.Generic;

namespace stork
{
    //Represents a single function in Stork.
    public class StorkFunction
    {
        //The internal function reference, if it is internal.
        public InternalFunctionDelegate Internal = null;

        //For non-internal functions, the statement body to evaluate.
        public List<storkParser.StatementContext> StatementBody = new List<storkParser.StatementContext>();

        //The list of parameters.
        public Dictionary<string, StorkClass> Parameters = new Dictionary<string, StorkClass>();

        //Constructors for internal and custom.
        public StorkFunction(InternalFunctionDelegate internalFuncRef)
        {
            Internal = internalFuncRef;
        }

        public StorkFunction(List<storkParser.StatementContext> funcBody, Dictionary<string, StorkClass> parameters)
        {
            StatementBody = funcBody;
            Parameters = parameters;
        }
    }

    //Delegate for internal functions.
    public delegate StorkClassInstance InternalFunctionDelegate(StorkClassInstance[] args);
}