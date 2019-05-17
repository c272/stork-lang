grammar Stork;

/*
 * Parsing Rules
 */
 
 
/*
 * Lexing Rules
 */

//Can be negative, use numbers 0-9, must include a ".".
FLOAT: [-]? [0-9]+ '.' [0-9]+ ;

//Can be negative, use numbers 0-9.
INTEGER: [-]? [0-9]+ ;

//Any spaces, tabs, newlines, carriage returns, skip.
WHITESPACE : [ \t\r\n]+ -> skip ;
