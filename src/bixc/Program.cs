using Colorify;
using Colorify.UI;
using McMaster.Extensions.CommandLineUtils;
using Compiler;


var colorify = new Format(Theme.Dark);

var app = new CommandLineApplication
{
    Name = "bixc",
    FullName = "bix language compiler",
    Description = "this is a compiler for a toy language \"bix\" made by kamkow1"
};

app.Command("build", cmd => 
{
    cmd.Description = "build a bix project";

    var filePathArg = cmd.Argument("[root]", "path to a bix project");

    cmd.OnExecute(async () => 
    {
        var filePath = filePathArg.Value;

        if (filePath is null)
        {
            colorify.WriteLine($"could not resolve path: \"{filePath}\"", Colors.txtDanger);
            return;
        }
            
        var targetFile = await File.ReadAllTextAsync(filePath);

        CompilationExecutor.Compile(targetFile);
    });
});

return app.Execute(args);