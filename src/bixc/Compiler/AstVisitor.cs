using Antlr4.Runtime.Misc;

namespace Compiler;

public class AstVisitor : BixParserBaseVisitor<object?>
{
    public override object? VisitFile_content([NotNull] BixParser.File_contentContext context)
    {
        foreach(var statement in context.statement())
            Visit(statement);
        return null;
    }

    public override object VisitAssign_variable([NotNull] BixParser.Assign_variableContext context)
    {
        var name = context.IDENTIFIER().GetText();
        var value = Visit(context.expression());

        Console.WriteLine($"{name} = {value}");
        return true;
    }

    public override object VisitConstant([NotNull] BixParser.ConstantContext context)
    {
        if (context.INT_VAL() is { } integerVal)
            return int.Parse(integerVal.GetText());
        
        if (context.FLT_VAL() is { } floatVal)
            return float.Parse(floatVal.GetText());

        if (context.STR_VAL() is { } stringVal)
            return stringVal.GetText()[1..^1];
        
        return new NotImplementedException("unknown constant value type");
    }
}