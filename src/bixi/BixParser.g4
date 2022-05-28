parser grammar BixParser;

options {
    tokenVocab='./Interpreter/Lexer/BixLexer';
    language=CSharp;
}

parse: file_content;

file_content:
        statement*;




statement:
        expression              TERMINATOR
    |   assign_variable         TERMINATOR;

lambda:
        FNC LPAREN (IDENTIFIER (COMMA IDENTIFIER)*)? RPAREN block;

block:
        LBRACE (statement*)? RBRACE;

assign_variable:
        VAL IDENTIFIER ASSIGN expression;

expression:
        constant
    |   lambda;

constant:
        STR_VAL
    |   INT_VAL
    |   FLT_VAL;