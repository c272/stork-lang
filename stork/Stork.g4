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
statement: ( 
			  stat_assign
			| stat_functionCall
		   )
			ENDLINE
		   ;

//A variable assign statement.
stat_assign: type=IDENTIFIER name=IDENTIFIER SET_EQUALS expr;

//A function call statement.
stat_functionCall: id=IDENTIFIER LBRACKET params? RBRACKET;

//A generic parameter list.
params: (expr COMMA)* expr;

//An expression to be evaluated which returns a value.
expr:  
		//Bracketed statement or a value.
		LBRACKET expr RBRACKET
	   | value

		//Mathematical operations.
	   | expr operator expr;
	   

//Something that contains a value in Stork.
value:   INTEGER
	   | FLOAT
	   | STRING
	   | IDENTIFIER
	   | stat_functionCall;

//A mathematical operator.
operator: OP_ADD | OP_MUL | OP_MINUS | OP_DIV;

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

//Operator symbols.
OP_ADD: '+';
OP_MINUS: '-';
OP_MUL: '*';
OP_DIV: '/';

//Character constants.
QUOTE: '"';
ENDLINE: ';';
SET_EQUALS: '=';
LBRACKET: '(';
RBRACKET: ')';
COMMA: ',';

//Identifier, should be a last resort.
IDENTIFIER: [A-Za-z_] [A-Za-z_0-9]*;

//Skip all unnecessary whitespace.
WS
	: [ \n\t] -> skip
	;

//Unknown symbol.
UNKNOWN_SYMBOL: .;