using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace python_lexer_dotnet
{
    public interface ILexer
    {
        bool GetCodeForAnalyze(string code);
        Token RecognizeNumber();
        Token RecognizeString();
        Token RecognizeIdentifier();
        Token RecognizeComment();
        Token GetNextToken();
        int GetPosition();
        Token RecognizeOperatorOrPunctuation();
    }
}
