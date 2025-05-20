using System.Text;
using System.IO;
using System;

namespace python_lexer_dotnet
{
    public class LexerManager
    {
        private readonly ILexer lexer;
        public LexerManager()
        {
            this.lexer = new Lexer();
        }
        public LexerManager(ILexer lexer)
        {
            this.lexer = lexer ?? throw new ArgumentNullException(nameof(lexer));
        }
        private string TokenTypeToString(TokenType type) => type.ToString();
        public string ReadCodeFile(string filePath)
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
        private void WriteToFile(string filename, string content)
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
            if (string.IsNullOrWhiteSpace(code))
            {
                Console.Error.WriteLine("Error: No code to process.");
                return;
            }
            try
            {
                lexer.GetCodeForAnalyze(code);
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
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: Failed to analyze code. {ex.Message}");
                return;
            }
        }
    }
}
