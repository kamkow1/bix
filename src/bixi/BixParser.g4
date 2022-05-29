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
    |   assign_variable         TERMINATOR
    |   function;

lambda:
        FNC LPAREN (IDENTIFIER (COMMA IDENTIFIER)*)? RPAREN block;

function:
        FNC IDENTIFIER LPAREN (IDENTIFIER (COMMA IDENTIFIER)*)? RPAREN def_body;

def_body:
        LBRACE (statement*)? RBRACE;

function_call:
        IDENTIFIER LPAREN (expression (COMMA expression)*)? RPAREN;

block:
        LBRACE (statement*)? RBRACE;

assign_variable:
        VAL IDENTIFIER IDENTIFIER ASSIGN expression;

object_property:
        IDENTIFIER (DOT IDENTIFIER)*;

expression:
        constant                #ConstantExpression
    |   lambda                  #LambdaExpression
    |   function_call           #FunctionCallExpression
    |   IDENTIFIER              #IdentifierExpression
    |   object_property         #ObjectProtertyExpression;

constant:
        STR_VAL
    |   INT_VAL
    |   FLT_VAL;