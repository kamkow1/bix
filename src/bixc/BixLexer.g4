lexer grammar BixLexer;

options {
    language = CSharp;
}

VAL                 : 'val';
FNC                 : 'fnc';
CLASS               : 'class';

// values
STR_VAL             : '"' (~[\\"\r\n])* '"';
INT_VAL             : '-'? '0'..'9'+;
FLT_VAL             : '-'? ('0'..'9')+ '.' ('0'..'9')*;

// opertators
ASSIGN              : '=';

TERMINATOR          : ('\r\n' | '\n');
COMMA               : ',';
LPAREN              : '(';
RPAREN              : ')';
LBRACE              : '{';
RBRACE              : '}';

HASH                : '#';
IDENTIFIER          : [a-zA-Z_] [a-zA-Z_0-9]*;
WHITESPACE          : [ \t]+    -> skip;
COMMENT             : '#*' .*? '*#' -> skip;
LINE_COMMENT        : '#' ~[\r\n]* -> skip;