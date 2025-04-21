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

        Assert.That(token.Lexeme, Is.EqualTo("test"));
        Assert.That(token.Type, Is.EqualTo(TokenType.IDENTIFIER));
    }

    [Test]
    public void TokenConstructor_InputIsNumber_ReturnThisNumber()
    {
        token = new Token("123", TokenType.NUMBER);

        Assert.That(token.Lexeme, Is.EqualTo("123"));
        Assert.That(token.Type, Is.EqualTo(TokenType.NUMBER));
    }

    [Test]
    public void TokenConstructor_InputIsString_ReturnThisString()
    {
        token = new Token("\"hello\"", TokenType.STRING);

        Assert.That(token.Lexeme, Is.EqualTo("\"hello\""));
        Assert.That(token.Type, Is.EqualTo(TokenType.STRING));
    }

    [Test]
    public void TokenConstructor_InputIsReservedWord_ReturnThisReservedWord()
    {
        token = new Token("if", TokenType.RESERVED);

        Assert.That(token.Lexeme, Is.EqualTo("if"));
        Assert.That(token.Type, Is.EqualTo(TokenType.RESERVED));
    }
}
