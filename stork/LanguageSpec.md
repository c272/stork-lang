# "Stork" Language Specification

## Examples
**The basic "Hello World" program in Stork:**

    ~import "log";
    log("Hello World");

**C-like "if" and preprocessor statements.**

    ~preprocesscommand "parameter1" "parameter2";
    if (somestatement && 3==4 || 5==exampleVar) {
	    dosomething();
    }
    
**Functions and parameters:**

    //This is an example of function predefinition.//
    proc exampleFunc(int,string);
    exampleFunc(2, "hello");
    
    proc exampleFunc(int parameter1, string parameter2) {
    	log("This is a function example.");
	return true;
    }

**Variable declaration:**

    int variable1;
    int variable2 = 3;
    gen genericVariable;