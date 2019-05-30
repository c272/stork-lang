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

statement: ;

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
IDENTIFIER: [A-Za-z_] [A-Za-z_0-9]*;

//Comment, skip this.
COMMENT: '//' (~[\n])* '\n' -> skip;

//Skip all unnecessary whitespace.
WS
	: [ \r\n\t] -> skip
	;

//Unknown symbol.
UNKNOWN_SYMBOL: .;