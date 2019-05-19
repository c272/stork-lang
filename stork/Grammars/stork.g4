grammar stork;

/*
 * Parser Rules
 */

compileUnit
	: statement* EOF
	;

//A single, eval-ready statement.
statement: (stat_assignvar
		 | stat_declarevar
		 | FUNCTION_CALL) EOL;

//A variable assignment statement.
stat_assignvar: IDENTIFIER EQUALS expression;

//A variable declaration statement.
stat_declarevar: (BASE_TYPE | IDENTIFIER) IDENTIFIER EQUALS expression;

//A single expression.
expression :   
			 //Base types.
			   INTEGER
			 | FLOAT
			 | BOOLEAN
			 | STRING
			 | FUNCTION_CALL

			 //Mathematical operations.
			 | expression OPERATOR expression; 

/*
 * Lexer Rules
 */

//Integer, can be 0-9 repeated.
INTEGER: [0-9]+;

//Float, two integers separated by a floating point.
FLOAT: INTEGER '.' INTEGER;

//String, any chars but quotes, surrounded by quotes.
STRING: QUOTE ( ~('"'))* QUOTE;

//Boolean, can be true or false or 1 or 0.
BOOLEAN: 'true' | 'false';

//Function call, an identifier plus parameters.
FUNCTION_CALL: IDENTIFIER '(' PARAMS? ')';

//Parameter definitions.
PARAMS: (VALUE COMMA)* VALUE;

//Type declaration strings.
BASE_TYPE: 'int' 
		| 'flt' 
		| 'bln' 
		| 'str';

//Any mathematical operator.
OPERATOR: '+' | '-' | '/' | '*';

//Value, any single action/token that has a value.
VALUE:   INTEGER 
	   | FLOAT 
	   | STRING 
	   | BOOLEAN
	   | FUNCTION_CALL;

//Constant characters that don't fit other tokens.
COMMA: ',';
EOL: ';';
QUOTE: '"';
EQUALS: '=';

//Ignore any whitespace (spaces, tabs, escapes).
WS:	[ \n\r\t] -> skip;

//Identifier, anything that doesn't fall into the previous categories.
IDENTIFIER: [A-Za-z] [A-Za-z0-9]*;

ERROR_NO_TOKEN
   : .
   ;