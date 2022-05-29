namespace Interpreter;

public class Variable
{
    public string VariableName { get; set; }

    public BixType Type { get; set; }

    
    public object? Value { get; set; }

    public Variable(string name, BixType type, object? value)
    {
        VariableName = name;
        Type = type;
        Value = value;
    }
}