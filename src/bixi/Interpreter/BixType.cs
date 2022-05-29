using System.Dynamic;

namespace Interpreter;

public class BixType
{
    public string Name { get; set; }

    public ExpandoObject ProtoType { get; set; }
}