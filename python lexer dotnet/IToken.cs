using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace python_lexer_dotnet
{
    internal interface IToken
    {
        string Lexeme { get; }
        TokenType Type { get; }
    }
}
