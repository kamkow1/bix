using System;
using System.Dynamic;

namespace Interpreter;

public class BixTypePrototypes
{
    public Dictionary<string, ExpandoObject> Prototypes = new();

    public BixTypePrototypes()
    {
        Prototypes["int_proto"] = CreateIntPrototype();
    }

    private ExpandoObject CreateIntPrototype()
    {
        dynamic proto = new ExpandoObject();

        //proto.hello = new Func<string>(() => "eoeooeo");
        proto.siema = "haha";

        return proto;
    }
}