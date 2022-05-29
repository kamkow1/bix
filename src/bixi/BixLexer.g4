lexer grammar BixLexer;

options {
    language = CSharp;
}

VAL                 : 'val';
FNC                 : 'func';
CLASS               : 'class';

// values
STR_VAL             : '"' (~[\\"\r\n])* '"';
INT_VAL             : '-'? '0'..'9'+;
FLT_VAL             : '-'? ('0'..'9')+ '.' ('0'..'9')*;

// opertators
ASSIGN              : '=';

TERMINATOR          : ';';
COMMA               : ',';
LPAREN              : '(';
RPAREN              : ')';
LBRACE              : '{';
RBRACE              : '}';
COLON               : ':';
DOT                 : '.';

HASH                : '#';
IDENTIFIER          : ('a'..'z' | 'A'..'Z' | '_') ('a'..'z' | 'A' .. 'Z' | '0'..'9' | '_')*;
WHITESPACE          : [ \r\n\t]+    -> skip;
COMMENT             : '#*' .*? '*#' -> channel(HIDDEN);
LINE_COMMENT        : '#' ~[\r\n]* -> channel(HIDDEN);