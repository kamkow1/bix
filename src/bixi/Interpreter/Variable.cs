namespace Interpreter;

public class Variable
{
    public string VariableName { get; set; }

    public BixType Type { get; set; }

    public Variable(string name, BixType type)
    {
        VariableName = name;
        Type = type;
    }
}