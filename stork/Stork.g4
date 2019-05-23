 grammar stork;

/*
 * Parser Rules
 */

 // HIGH LEVEL CONSTRUCTS

 //The final compile unit sent to the interpreter.
compileUnit
	: block	EOF
	;

//A block, array of statements.
block: statement*
	 ;

statement: ( stat_define
		   | stat_assign
		   | stat_functionCall)
			 ENDLINE
		   ;

//A single expression which returns a value.
expr: value |
			//Expression in precedence brackets.
			LBRACKET value RBRACKET |

			//Expressions with an operator.
			lexpr=expr operator rexpr=expr |
			expr postfix_op;

// STATEMENT TYPES

//A single variable definition/assignment.
stat_define: vartype=IDENTIFIER varname=IDENTIFIER EQUALS expr;
stat_assign: IDENTIFIER EQUALS expr;

//A single function call.
stat_functionCall: IDENTIFIER LBRACKET params? RBRACKET;


// MID LEVEL CONSTRUCTS

//Value, a thing that contains value in Stork.
value: INTEGER 
	   | FLOAT 
	   | BOOLEAN 
	   | STRING 
	   | IDENTIFIER
	   | stat_functionCall;

//A generic list of parameters.
params: (expr COMMA)* expr;

//An infix operator.
operator: ADD_OP
		  | TAKE_OP
		  | MUL_OP
		  | DIV_OP
		  ;

//A prefix operator.
//prefix_op: 

//A postfix operator.
postfix_op:   INCREMENT_POSTFIX_OP
			| DECREMEMT_POSTFIX_OP;


/*
 * Lexer Rules
 */

//Integer, a number from 0-inf, whole value.
INTEGER: [1-9] [0-9]* | '0';

//Float, two integers with a floating point between them.
FLOAT: INTEGER POINT INTEGER;

//Boolean, true or false.
BOOLEAN: 'true' | 'false';

//String, characters surrounded by quotes.
STRING: QUOTE (~["])* QUOTE;

/////////////////////
// MATHS OPERATORS //
/////////////////////

ADD_OP: '+';
TAKE_OP: '-';
MUL_OP: '*';
DIV_OP: '/';
EQUALS_OP : '==';
GREATER_OR_EQUALS_OP: '>=';
LESS_OR_EQUALS_OP: '<=';
GREATER_OP: '>';
LESS_OP: '<';
INCREMENT_POSTFIX_OP: '++';
DECREMENT_POSTFIX_OP: '--';
ADDTO_OP: '+=';
TAKEFROM_OP: '-=';

//////////////////////
// CONSTANT SYMBOLS //
//////////////////////

POINT: '.';
ENDLINE: ';';
COMMA: ',';
QUOTE: '"';
LBRACKET: '(';
RBRACKET: ')';
LBRACE: '{';
RBRACE: '}';
EQUALS: '=';

//////////////////////

//Identifier, second last resort for tokens.
IDENTIFIER: [A-Za-z_][A-Za-z_0-9]*;

//Skip all unnecessary whitespace.
WS
	: [ \n\t] -> skip
	;

//Unknown symbol.
UNKNOWN_SYMBOL: .;