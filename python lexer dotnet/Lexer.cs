using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace python_lexer_dotnet
{
    public class Lexer
    {
        private readonly string input;
        private int pos;
        private readonly Dictionary<string, TokenType> reservedWords = new()
        {
            { "if", TokenType.RESERVED },
            { "else", TokenType.RESERVED },
            { "while", TokenType.RESERVED },
            { "for", TokenType.RESERVED },
            { "def", TokenType.RESERVED },
            { "return", TokenType.RESERVED },
            { "class", TokenType.RESERVED },
            { "import", TokenType.RESERVED },
            { "from", TokenType.RESERVED },
            { "as", TokenType.RESERVED },
            { "with", TokenType.RESERVED },
            { "try", TokenType.RESERVED },
            { "except", TokenType.RESERVED },
            { "finally", TokenType.RESERVED },
            { "raise", TokenType.RESERVED },
            { "in", TokenType.RESERVED },
            { "is", TokenType.RESERVED },
            { "not", TokenType.RESERVED },
            { "and", TokenType.RESERVED },
            { "or", TokenType.RESERVED }
        };

        public Lexer(string input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }
            this.input = input;
            pos = 0;
        }

        private void SkipWhitespace()
        {
            while (pos < input.Length && char.IsWhiteSpace(input[pos]))
            {
                pos++;
            }
        }

        public Token RecognizeNumber()
        {
            int start = pos;

            if (input[pos] == '0' && pos + 1 < input.Length && char.ToLower(input[pos + 1]) == 'x')
            {
                pos += 2;
                bool hasHexDigits = false;
                while (pos < input.Length && Uri.IsHexDigit(input[pos]))
                {
                    hasHexDigits = true;
                    pos++;
                }
                if (!hasHexDigits || (pos < input.Length && char.IsLetterOrDigit(input[pos])))
                {
                    while (pos < input.Length && char.IsLetterOrDigit(input[pos])) pos++;
                    return new Token(input.Substring(start, pos - start), TokenType.ERROR);
                }
                return new Token(input.Substring(start, pos - start), TokenType.NUMBER);
            }

            while (pos < input.Length && char.IsDigit(input[pos])) pos++;

            if (pos < input.Length && input[pos] == '.')
            {
                pos++;
                while (pos < input.Length && char.IsDigit(input[pos])) pos++;
            }

            if (pos < input.Length && char.IsLetterOrDigit(input[pos]))
            {
                while (pos < input.Length && char.IsLetterOrDigit(input[pos])) pos++;
                return new Token(input.Substring(start, pos - start), TokenType.ERROR);
            }

            return new Token(input.Substring(start, pos - start), TokenType.NUMBER);
        }

        public Token RecognizeString()
        {
            char quote = input[pos];
            int start = pos++;
            bool escaped = false;

            while (pos < input.Length)
            {
                char current = input[pos];

                if (current == '\\' && !escaped)
                {
                    escaped = true;
                    pos++;
                    continue;
                }

                if (current == quote && !escaped)
                {
                    pos++;
                    break;
                }

                escaped = false;
                pos++;
            }

            return new Token(input.Substring(start, pos - start), TokenType.STRING);
        }

        public Token RecognizeIdentifier()
        {
            int start = pos;
            while (pos < input.Length && (char.IsLetterOrDigit(input[pos]) || input[pos] == '_' || input[pos] == '.')) pos++;

            string lexeme = input.Substring(start, pos - start);
            if (reservedWords.ContainsKey(lexeme))
            {
                return new Token(lexeme, reservedWords[lexeme]);
            }
            return new Token(lexeme, TokenType.IDENTIFIER);
        }

        public Token RecognizeComment()
        {
            int start = pos;
            while (pos < input.Length && input[pos] != '\n') pos++;
            return new Token(input.Substring(start, pos - start), TokenType.COMMENT);
        }

        public Token RecognizeOperatorOrPunctuation()
        {
            return new Token(input[pos++].ToString(), TokenType.OPERATOR);
        }

        public Token GetNextToken()
        {
            SkipWhitespace();
            if (pos >= input.Length) return new Token("", TokenType.END);

            char current = input[pos];

            if (char.IsDigit(current)) return RecognizeNumber();
            if (current == '"' || current == '\'') return RecognizeString();
            if (char.IsLetter(current) || current == '_') return RecognizeIdentifier();
            if (current == '#') return RecognizeComment();
            if (char.IsPunctuation(current)) return RecognizeOperatorOrPunctuation();

            Token errorToken = new Token(current.ToString(), TokenType.ERROR);
            pos++;
            return errorToken;
        }

        public int GetPosition() => pos;
    }
}
