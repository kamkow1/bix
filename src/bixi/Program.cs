using Colorify;
using Colorify.UI;
using McMaster.Extensions.CommandLineUtils;
using Interpreter;


var colorify = new Format(Theme.Dark);

var app = new CommandLineApplication
{
    Name = "bixc",
    FullName = "bix language interpreter",
    Description = "this is an interpreter for a toy language \"bix\" made by kamkow1"
};

app.Command("run", cmd => 
{
    cmd.Description = "execute a bix file";

    var filePathArg = cmd.Argument("[root]", "path to a bix project");

    cmd.OnExecute(() => 
    {
        var filePath = filePathArg.Value;

        if (filePath is null)
        {
            colorify.WriteLine($"could not resolve path: \"{filePath}\"", Colors.txtDanger);
            return;
        }
            
        var targetFile = File.ReadAllText(filePath);

        InterpreterRunner.Run(targetFile, Path.GetFileName(filePath));
    });
});

return app.Execute(args);