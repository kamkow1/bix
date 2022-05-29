using Antlr4.Runtime.Misc;
using Newtonsoft.Json;

namespace Interpreter;

public class AstVisitor : BixParserBaseVisitor<object?>
{
    private Dictionary<string, dynamic?> _environment = new();

    private Functions _functions = new();

    private BixTypePrototypes _prototypes = new();
    
    public override object? VisitFile_content([NotNull] BixParser.File_contentContext context)
    {
        foreach(var statement in context.statement())
            Visit(statement);
        return null;
    }


    public override object VisitAssign_variable([NotNull] BixParser.Assign_variableContext context)
    {
        var name = context.IDENTIFIER(1).GetText();
        var value = Visit(context.expression());
        var typeName = context.IDENTIFIER(0).GetText();

        var variable = new Variable(name, new BixType {
            Name = typeName,
            ProtoType = _prototypes.Prototypes[$"{typeName}_proto"]
        });

        _environment.Add(name, variable);

        Console.WriteLine("eoeoeoeo" + _environment[name].Type.ProtoType.hello());
        return true;
    }

    public override object? VisitLambda([NotNull] BixParser.LambdaContext context)
    {
        var parameters = context.IDENTIFIER().Select(p => p.GetText()).ToArray();
        Console.WriteLine(JsonConvert.SerializeObject(parameters, Formatting.Indented));
        return null;
    }

    public override object? VisitFunction([NotNull] BixParser.FunctionContext context)
    {
        var name = context.IDENTIFIER(0).GetText();

        var body = Visit(context.def_body());
        Console.WriteLine(body);

        _environment.Add(name, body);

        return null;
    }

    public override object VisitFunction_call([NotNull] BixParser.Function_callContext context)
    {
        var args = new List<object?>();
        foreach(var expr in context.expression())
            args.Add(Visit(expr));

        return _functions.CallFunction(context.IDENTIFIER().GetText(), args.ToArray());
    }

    public override object VisitDef_body([NotNull] BixParser.Def_bodyContext context)
    {
        Console.WriteLine(context.statement().Select(s => s.GetText()));
        return context.statement();
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

    public override object VisitIdentifierExpression([NotNull] BixParser.IdentifierExpressionContext context)
    {
        return _environment[context.IDENTIFIER().GetText() ?? throw new Exception("this identifier does not exist!")]!;
    }
}