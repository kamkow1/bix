using Antlr4.Runtime;

namespace Interpreter;

public static class InterpreterRunner
{
    public static void Run(string fileContent, string fileName) 
    {
        var inputStream         = new AntlrInputStream(fileContent);
        var lexer               = new BixLexer(inputStream);
        var commonTokenStream   = new CommonTokenStream(lexer);
        var parser              = new BixParser(commonTokenStream);

        var parseContext        = parser.parse();
        var visitor             = new AstVisitor(); 

        visitor.Visit(parseContext); 
    }
}