using System;
using System.Dynamic;

namespace Interpreter;

public class BixTypePrototypes
{
    public Dictionary<string, ExpandoObject> Prototypes = new();

    public BixTypePrototypes()
    {
        Prototypes["int_proto"] = CreateIntPrototype();
        Prototypes["io_proto"] = CreateIoPrototype();
    }

    private ExpandoObject CreateIntPrototype()
    {
        dynamic proto = new ExpandoObject();

        proto.siema = "haha";
        proto.lol = new Func<object?>(() => {
            Console.WriteLine("hihihihihihi");
            return "null";
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