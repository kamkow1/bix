using System;
public class Functions
{
    private Dictionary<string, Func<object?[], object?>> _functions = new();

    public Functions()
    {
        _functions["print"] = new Func<object?[], object?>(Print);
    }

    public object? CallFunction(string name, object?[] args) 
    {
        return _functions[name](args);
    }

    private object? Print(object?[] args)
    {
        foreach(var arg in args)
            Console.Write(arg);

        return null;
    }
}