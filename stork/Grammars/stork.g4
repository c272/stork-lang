grammar stork;

/*
 * Parser Rules
 */

compileUnit: statement* EOF;

statement: (stat_assign) ENDLINE;

stat_assign: IDENTIFIER IDENTIFIER EQUALS value;

//An object in Stork that can return or hold a value.
value: INTEGER 
	 | STRING 
	 | IDENTIFIER;

/*
 * Lexer Rules
 */

//An integer.
INTEGER: [1-9] [0-9]* | '0';

//A string.
STRING: QUOTE (~["])* QUOTE;

//Identifier, should be after all other possibilities.
IDENTIFIER: [A-Za-z][0-9A-Za-z]*;

//////////////////////
// SYMBOL CONSTANTS //
//////////////////////
QUOTE: '"';
ENDLINE: ';';
EQUALS: '=';

//Ignore whitespace.
WS: [ \n\r\t]+ -> skip;