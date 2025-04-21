namespace python_lexer_dotnet
{
    internal class Program
    {

        private static void Main(string[] args)
        {
            LexerManager lexerManager = new LexerManager(new Lexer());
            string filePath = args.Length > 0 ? args[0] : "./python-code-for-analysis/script.py";
            string code = lexerManager.ReadPythonFile(filePath);

            if (!string.IsNullOrWhiteSpace(code))
            {
                lexerManager.ProcessCode(code);
            }
            else
            {
                Console.Error.WriteLine("Error: No code to process.");
            }
        }
    }
}