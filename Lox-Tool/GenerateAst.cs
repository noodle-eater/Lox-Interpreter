using System.Text;

namespace Lox_Tool;

public class GenerateAst
{
    public static void Main(string[] args)
    {
        // if (args.Length != 1)
        // {
        //     Console.Error.WriteLine("Usage: generate_ast <output directory>");
        //     Environment.Exit(64);
        // }

        string outputDir = @"C:\Users\Hamda\Documents\RiderProjects\Lox-Interpreter\Lox-Interpreter\";
        
        DefineAst(outputDir, "Expr", new List<string>
        {
            "Binary   : Expr left, Token op, Expr right",
            "Grouping : Expr expression",
            "Literal  : Object value",
            "Unary    : Token op, Expr right"
        });
    }

    private static void DefineAst(string outputDir, string baseName, List<string> types)
    {
        string path = $"{outputDir}/{baseName}.cs";
        StringBuilder writer = new StringBuilder();

        writer.AppendLine("namespace Lox_Interpreter;");
        writer.AppendLine();
        writer.AppendLine("public abstract class " + baseName);
        writer.AppendLine("{");
        
        DefineVisitor(writer, baseName, types);

        writer.AppendLine();
        writer.AppendLine("    public abstract T Accept<T>(IVisitor<T> visitor);");
        writer.AppendLine();
        
        // The AST classes.
        foreach (string type in types) {
            string className = type.Split(":")[0].Trim();
            string fields = type.Split(":")[1].Trim(); 
            DefineType(writer, baseName, className, fields);
        }
        
        writer.AppendLine("}");
        
        File.WriteAllText(path, writer.ToString());
    }

    private static void DefineVisitor(StringBuilder writer, string baseName, List<string> types)
    {
        writer.AppendLine("    public interface IVisitor<T>");
        writer.AppendLine("    {");

        foreach (string type in types)
        {
            string typeName = type.Split(":")[0].Trim();
            writer.AppendLine($"        T Visit{ToUpperFistLetter(typeName)}{ToUpperFistLetter(baseName)}" +
                              $"({typeName} {baseName.ToLower()});");
        }
        writer.AppendLine("    }");
    }

    private static void DefineType(StringBuilder writer, string baseName, string className, string fieldList)
    {
        writer.AppendLine($"    public class {className} : {baseName}");
        writer.AppendLine("    {");
        
        // Store parameters in fields.
        string[] fields = fieldList.Split(", ");
        
        // Fields
        writer.AppendLine();
        foreach (var field in fields)
        {
            writer.AppendLine($"        public readonly {field};");
        }
        writer.AppendLine();

        // Constructor
        writer.AppendLine($"        public {className}({fieldList})");
        writer.AppendLine("        {");
        foreach (string field in fields)
        {
            string name = field.Split(" ")[1];
            writer.AppendLine($"            this.{name} = {name};");
        }
        writer.AppendLine("        }");

        writer.AppendLine();
        writer.AppendLine("        public override T Accept<T>(IVisitor<T> visitor)");
        writer.AppendLine("        {");
        writer.AppendLine($"            return visitor.Visit{ToUpperFistLetter(className)}{ToUpperFistLetter(baseName)}(this);");
        writer.AppendLine("        }");

        writer.AppendLine("    }");
    }

    private static string ToUpperFistLetter(string str) => char.ToUpper(str[0]) + str.Substring(1);
}