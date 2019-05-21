grammar stork;

/*
 * Parser Rules
 */

 //The final compile unit sent to the interpreter.
compileUnit
	: block	EOF
	;

//A block, array of statements.
block: statement*
	 ;

//A single statement.
statement: stat_ass;

stat_ass: IDENTIFIER IDENTIFIER SET_EQUALS value ENDLINE;

//An expression that can contain a value.
value:   INTEGER
	   | FLOAT
	   | STRING
	   | IDENTIFIER;

/*
 * Lexer Rules
 */

//Integer. Any combination of whole numbers, starting with non-zero.
INTEGER: [1-9] [0-9]* | '0'
       ;

//Float. Two integers separated by a '.'.
FLOAT: INTEGER '.' INTEGER
     ;

//String.
STRING: QUOTE (~["])* QUOTE
	  ;

// CONSTANT SYMBOLS!
QUOTE: '"';
ENDLINE: ';';
SET_EQUALS: '=';

//Identifier, should be a last resort.
IDENTIFIER: [A-Za-z_] [A-Za-z_0-9]*;

//Skip all unnecessary whitespace.
WS
	: [ \n\t] -> skip
	;

//Unknown symbol.
UNKNOWN_SYMBOL: .;