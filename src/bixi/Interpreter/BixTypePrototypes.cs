using System;
using System.Dynamic;
using Newtonsoft.Json;

namespace Interpreter;

public class BixTypePrototypes
{
    public Dictionary<string, ExpandoObject> Prototypes = new();

    public BixTypePrototypes()
    {
        Prototypes["int_proto"] = CreateIntPrototype();
        Prototypes["str_proto"] = CreateStringPrototype();

        Prototypes["io_proto"] = CreateIoPrototype();
    }

    private ExpandoObject CreateIntPrototype()
    {
        dynamic proto = new ExpandoObject();

        return proto;
    }
    
    private ExpandoObject CreateStringPrototype()
    {
        dynamic proto = new ExpandoObject();

        proto.toInt = new Func<object?[], object?>((object?[] arg) => {
            return int.Parse(arg[0] as string);
        });

        return proto;
    }

    private ExpandoObject CreateIoPrototype()
    {
        dynamic proto = new ExpandoObject();

        proto.println = new Func<object?[]?, object?>((object?[]? args) => {
            foreach(var arg in args)
                Console.WriteLine(arg);
            return null;
        });

        proto.print = new Func<object?[]?, object?>((object?[]? args) => {
            foreach(var arg in args)
                Console.Write(arg);
            return null;
        });

        proto.input = new Func<object?>(() => Console.ReadLine());

        return proto;
    }
}