using System.Dynamic;
using Antlr4.Runtime.Misc;

namespace Interpreter;

public class AstVisitor : BixParserBaseVisitor<object?>
{
    private Dictionary<string, dynamic?> _environment = new();

    private BixTypePrototypes _prototypes = new();

    public AstVisitor()
    {
        _environment["io"] = new Variable(
            "io", 
            new BixType {
                Name = "Io",
                ProtoType = _prototypes.Prototypes[$"io_proto"]
            },
            new object()
        );
    }
    
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

        var variable = new Variable(
            name, 
            new BixType {
                Name = typeName,
                ProtoType = _prototypes.Prototypes[$"{typeName}_proto"],
            },
            value
        );

        _environment.Add(name, variable);

        return true;
    }

    public override object? VisitLambda([NotNull] BixParser.LambdaContext context)
    {
        var parameters = context.IDENTIFIER().Select(p => p.GetText()).ToArray();
        return null;
    }

    public override object? VisitFunction([NotNull] BixParser.FunctionContext context)
    {
        var name = context.IDENTIFIER(0).GetText();

        var body = Visit(context.def_body());

        _environment.Add(name, body);

        return null;
    }

    public override object VisitFunction_call([NotNull] BixParser.Function_callContext context)
    {
        throw new Exception("unimplemented");
    }

    public override object VisitDef_body([NotNull] BixParser.Def_bodyContext context)
    {
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
        return _environment[context.IDENTIFIER().GetText()]!.Value;
    }

    public override object? VisitObject_property([NotNull] BixParser.Object_propertyContext context)
    {

        var obj = _environment[context.IDENTIFIER(0).GetText()];
        dynamic next = new ExpandoObject();
        var counter = 0;
        foreach(var prop in context.IDENTIFIER().Select((value, i) => new { value, i }))
        {
            if (prop.i == 0)
                continue;
            next = obj.GetType().GetProperty(context.IDENTIFIER(prop.i).GetText());
            if (next is not null)
            {
                obj = next;
                counter = prop.i + 1;
            }
            else continue;
        }
        
        try
        {
            if (context.LPAREN() is null)
                return ((IDictionary<string, object>)obj.Type.ProtoType)[context.IDENTIFIER(counter + 1).GetText()];

            if (context.expression().Length != 0)
            {
                var expressionArgs = context.expression().Select(Visit).ToArray();
                
                var args = new List<object>();
                foreach(var arg in expressionArgs)
                {
                    if (_environment.TryGetValue(arg.ToString(), out var result))
                        args.Add(result.Value);
                }

                return ((Func<object?[], object?>)((IDictionary<string, object>)obj.Type.ProtoType)[context.IDENTIFIER(counter + 1).GetText()])(context.expression()[0].GetText().Contains("self") ? args.ToArray() : expressionArgs.ToArray());
            }

            return ((Func<object?>)((IDictionary<string, object>)obj.Type.ProtoType)[context.IDENTIFIER(counter + 1).GetText()])();
        }
        catch (System.InvalidCastException)
        {
            Console.WriteLine("cannot invoke method with provided argument list!");
            Environment.Exit(1);
            return null;
        }       
    }

    public override object VisitSelfExpression([NotNull] BixParser.SelfExpressionContext context)
    {
        return context.Parent.GetChild(context.Parent.ChildCount - 6).GetText();
    }
}