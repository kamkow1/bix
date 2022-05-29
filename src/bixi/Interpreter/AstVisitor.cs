using System.Linq;
using System.Dynamic;
using Antlr4.Runtime.Misc;
using Newtonsoft.Json;

namespace Interpreter;

public class AstVisitor : BixParserBaseVisitor<object?>
{
    private Dictionary<string, dynamic?> _environment = new();

    private BixTypePrototypes _prototypes = new();

    public AstVisitor()
    {
        _environment["io"] = new Variable("io", new BixType {
            Name = "Io",
            ProtoType = _prototypes.Prototypes[$"io_proto"]
        });
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

        //Console.WriteLine(typeName);

        var variable = new Variable(name, new BixType {
            Name = typeName,
            ProtoType = _prototypes.Prototypes[$"{typeName}_proto"]
        });

        //Console.WriteLine(JsonConvert.SerializeObject(_prototypes.Prototypes[$"{typeName}_proto"], Formatting.Indented));

        _environment.Add(name, variable);

        //Console.WriteLine("eoeoeoeo" + _environment[name].Type.ProtoType.hello());
        return true;
    }

    public override object? VisitLambda([NotNull] BixParser.LambdaContext context)
    {
        var parameters = context.IDENTIFIER().Select(p => p.GetText()).ToArray();
        //Console.WriteLine(JsonConvert.SerializeObject(parameters, Formatting.Indented));
        return null;
    }

    public override object? VisitFunction([NotNull] BixParser.FunctionContext context)
    {
        var name = context.IDENTIFIER(0).GetText();

        var body = Visit(context.def_body());
        //Console.WriteLine(body);

        _environment.Add(name, body);

        return null;
    }

    public override object VisitFunction_call([NotNull] BixParser.Function_callContext context)
    {
        /*var args = new List<object?>();
        foreach(var expr in context.expression())
            args.Add(Visit(expr));

        return _functions.CallFunction(context.IDENTIFIER().GetText(), args.ToArray());*/
        return "30300303";
    }

    public override object VisitDef_body([NotNull] BixParser.Def_bodyContext context)
    {
        //Console.WriteLine(context.statement().Select(s => s.GetText()));
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

    public override object? VisitObject_property([NotNull] BixParser.Object_propertyContext context)
    {

        var obj = _environment[context.IDENTIFIER(0).GetText()];
        dynamic next = new ExpandoObject();
        var counter = 0;
        foreach(var prop in context.IDENTIFIER().Select((value, i) => new { value, i }))
        {
            if (prop.i == 0)
                continue;
            //Console.WriteLine(prop.i);
            next = obj.GetType().GetProperty(context.IDENTIFIER(prop.i).GetText());
            if (next is not null)
            {
                obj = next;
                //Console.WriteLine(context.IDENTIFIER(prop.i + 1));
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
                var args = context.expression().Select(Visit).ToArray();
                //Console.WriteLine(JsonConvert.SerializeObject(args, Formatting.Indented));

                return ((Func<object?[], object?>)((IDictionary<string, object>)obj.Type.ProtoType)[context.IDENTIFIER(counter + 1).GetText()])(args);
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
}