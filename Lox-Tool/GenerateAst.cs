using System.Text;

namespace Lox_Tool;

public class GenerateAst
{
    public static void Main(string[] args)
    {
        if (args.Length != 1)
        {
            Console.Error.WriteLine("Usage: generate_ast <output directory>");
            Environment.Exit(64);
        }

        string outputDir = args[0];
        
        DefineAst(outputDir, "Expr", new List<string>
        {
            "Binary   : Expr left, Token operator, Expr right",
            "Grouping : Expr expression",
            "Literal  : Object value",
            "Unary    : Token operator, Expr right"
        });
    }

    private static void DefineAst(string outputDir, string baseName, List<string> types)
    {
        string path = $"{outputDir}/{baseName}.cs";
        StringBuilder writer = new StringBuilder();

        writer.AppendLine("namespace Lox_Interpreter;");
        writer.AppendLine();
        writer.AppendLine("public abstract class" + baseName);
        writer.AppendLine("{");

        writer.AppendLine("}");
    }
}