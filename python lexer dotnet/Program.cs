using python_lexer_dotnet;
using System.Text;

internal class Program
{
    static string TokenTypeToString(TokenType type) => type.ToString();

    static string ReadPythonFile(string filePath)
    {
        try
        {
            return File.ReadAllText(filePath);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: Could not read file {filePath}. {ex.Message}");
            return "";
        }
    }

    static void WriteToFile(string filename, string content)
    {
        try
        {
            string? directory = Path.GetDirectoryName(filename);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            File.WriteAllText(filename, content);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: Could not write to file {filename}. {ex.Message}");
        }
    }

    static void ProcessCode(string code)
    {
        Lexer lexer = new Lexer(code);
        Token token;
        StringBuilder result = new();

        while ((token = lexer.GetNextToken()).Type != TokenType.END)
        {
            if (token.Type == TokenType.ERROR)
            {
                string errorMsg = $"Error: Unrecognized token '{token.Lexeme}' at position {lexer.GetPosition()}";
                Console.Error.WriteLine(errorMsg);
                result.AppendLine(errorMsg);
            }
            else
            {
                string output = $"<{token.Lexeme}, {TokenTypeToString(token.Type)}>";
                Console.WriteLine(output);
                result.AppendLine(output);
            }
        }

        WriteToFile("./result/output.txt", result.ToString());
    }

    private static void Main(string[] args)
    {
        string filePath = args.Length > 0 ? args[0] : "./python-code-for-analysis/script.py";
        string code = ReadPythonFile(filePath);

        if (!string.IsNullOrWhiteSpace(code))
        {
            ProcessCode(code);
        }
        else
        {
            Console.Error.WriteLine("Error: No code to process.");
        }
    }
}