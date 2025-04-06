using python_lexer_dotnet;

namespace PythonLexerDotnet.Tests;

[TestFixture]
public class TokenTests
{
    private Token token;

    [SetUp]
    public void SetUp() {}

    [Test]
    public void TokenConstructor_InputIsTestIdentifier_ReturnThisIdentifier()
    {
        token = new Token("test", TokenType.IDENTIFIER);

        Assert.AreEqual("test", token.Lexeme);
        Assert.AreEqual(TokenType.IDENTIFIER, token.Type);
    }

    [Test]
    public void TokenConstructor_InputIsNumber_ReturnThisNumber()
    {
        token = new Token("123", TokenType.NUMBER);

        Assert.AreEqual("123", token.Lexeme);
        Assert.AreEqual(TokenType.NUMBER, token.Type);
    }

    [Test]
    public void TokenConstructor_InputIsString_ReturnThisString()
    {
        token = new Token("\"hello\"", TokenType.STRING);

        Assert.AreEqual("\"hello\"", token.Lexeme);
        Assert.AreEqual(TokenType.STRING, token.Type);
    }

    [Test]
    public void TokenConstructor_InputIsReservedWord_ReturnThisReservedWord()
    {
        token = new Token("if", TokenType.RESERVED);

        Assert.AreEqual("if", token.Lexeme);
        Assert.AreEqual(TokenType.RESERVED, token.Type);
    }
}
