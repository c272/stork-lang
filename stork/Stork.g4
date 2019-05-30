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
		   | stat_functionCall
		   | stat_functionDef 
		   | stat_classDef 
		   | stat_return )
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
stat_define: vartype=IDENTIFIER varname=IDENTIFIER EQUALS expr ENDLINE;
stat_assign: object_reference EQUALS expr ENDLINE;

//A single function call.
stat_functionCall: IDENTIFIER LBRACKET params? RBRACKET ENDLINE;

//A single function definition.
stat_functionDef: FUNCDEF_SYM IDENTIFIER LBRACKET funcdefparams? RBRACKET
				  LBRACE
					statement*
				  RBRACE;

//A single class definition.
stat_classDef: CLASS_SYM IDENTIFIER LBRACKET funcdefparams? RBRACKET
			   LBRACE
					 stat_constructor?
					(class_fieldDefine | class_functionDef)*
			   RBRACE;

//A class field or function definition. Can be static or non-static.
class_fieldDefine: (STATIC_SYM)? vartype=IDENTIFIER varname=IDENTIFIER ENDLINE;
class_functionDef: (STATIC_SYM)? stat_functionDef;

//A class constructor.
stat_constructor: CONSTRUCTOR_SYM LBRACE statement* RBRACE;

//A return statement.
stat_return: RETURN_SYM object_reference;

// MID LEVEL CONSTRUCTS

//An object reference, eg. "foo.bar().somefield"
object_reference: (object_subreference POINT)* object_subreference;
object_subreference: (IDENTIFIER | stat_functionCall);

//Value, a thing that contains value in Stork.
value: INTEGER 
	   | FLOAT 
	   | BOOLEAN 
	   | STRING
	   | object_reference
	   | stat_functionCall;

//A generic list of parameters.
params: (expr COMMA)* expr;

//A list of type defined parameters.
funcdefparams: (typeparam COMMA)* typeparam;
typeparam: typename=IDENTIFIER paramname=IDENTIFIER;

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
FUNCDEF_SYM: 'func';
IF_SYM: 'if';
ELSE_SYM: 'else';
STATIC_SYM: 'static';
CLASS_SYM: 'class';
CONSTRUCTOR_SYM: 'construct';
RETURN_SYM: 'return';

//////////////////////

//Identifier, second last resort for tokens.
IDENTIFIER: [A-Za-z_] [A-Za-z_0-9]*;

//Comment, skip this.
COMMENT: '//' (~[\n])* '\n' -> skip;

//Skip all unnecessary whitespace.
WS
	: [ \r\n\t] -> skip
	;

//Unknown symbol.
UNKNOWN_SYMBOL: .;