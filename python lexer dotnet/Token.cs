﻿namespace python_lexer_dotnet
{
    public enum TokenType
    {
        NUMBER,
        STRING,
        IDENTIFIER,
        COMMENT,
        RESERVED,
        OPERATOR,
        PUNCTUATION,
        ERROR,
        END
    }

    public class Token
    {
        public string Lexeme { get; set; }
        public TokenType Type { get; set; }

        public Token(string lexeme, TokenType type)
        {
            Lexeme = lexeme;
            Type = type;
        }
    }
}
