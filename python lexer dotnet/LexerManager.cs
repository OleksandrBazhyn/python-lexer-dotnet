using System.Text;

namespace python_lexer_dotnet
{
    public class LexerManager
    {
        private readonly ILexer lexer;
        public LexerManager()
        {
            lexer = new Lexer();
        }
        public LexerManager(ILexer lexer)
        {
            this.lexer = lexer ?? throw new ArgumentNullException(nameof(lexer));
        }
        public string TokenTypeToString(TokenType type) => type.ToString();
        public string ReadPythonFile(string filePath)
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
        public void WriteToFile(string filename, string content)
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
        public void ProcessCode(string code)
        {
            ILexer lexer = new Lexer(code);
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
    }
}
