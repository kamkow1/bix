using Antlr4.Runtime;

namespace Compiler;

public static class CompilationExecutor
{
    public static void Compile(string fileContent, string fileName) 
    {
        Console.WriteLine(fileContent);

        var inputStream         = new AntlrInputStream(fileContent);
        var lexer               = new BixLexer(inputStream);
        var commonTokenStream   = new CommonTokenStream(lexer);
        var parser              = new BixParser(commonTokenStream);

        var parseContext        = parser.parse();
        var visitor             = new AstVisitor(); 

        visitor.Visit(parseContext);

        
    }
}