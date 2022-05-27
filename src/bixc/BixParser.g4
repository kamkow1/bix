parser grammar BixParser;

options {
    tokenVocab='./Compiler/Lexer/BixLexer';
    language=CSharp;
}

parse: file_content EOF;

file_content:
        statement*;

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