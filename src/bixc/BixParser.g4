parser grammar BixParser;

options {
    tokenVocab='./Compiler/Lexer/BixLexer';
    language=CSharp;
}

parse: file_content EOF;

file_content:
        statement*;

block:
        RBRACE statement* LBRACE;

class:
        CLASS IDENTIFIER block;

method:
        FNC IDENTIFIER LPAREN (IDENTIFIER (COMMA IDENTIFIER))? RPAREN block;

statement:
        expression              TERMINATOR
    |   assign_variable         TERMINATOR;

assign_variable:
        VAL IDENTIFIER ASSIGN expression;

expression:
        constant;

constant:
        STR_VAL
    |   INT_VAL
    |   FLT_VAL;