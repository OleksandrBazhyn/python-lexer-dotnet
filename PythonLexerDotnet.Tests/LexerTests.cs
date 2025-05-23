using NUnit.Framework;
using System.Collections.Generic;
using python_lexer_dotnet;

namespace PythonLexerDotnet.Tests
{
    [TestFixture]
    public class LexerTests
    {
        private Lexer lexer;

        [SetUp]
        public void Setup()
        {
            lexer = new Lexer("if 123 0x1G 'test' #comment");
        }

        [Test]
        public void TestRecognizesReservedWord()
        {
            Token token = lexer.GetNextToken();
            Assert.That(token.Lexeme, Is.EqualTo("if"));
            Assert.That(token.Type, Is.EqualTo(TokenType.RESERVED));
        }

        [Test]
        public void TestRecognizesDecimalNumber()
        {
            lexer.GetNextToken(); // skip "if"
            Token token = lexer.GetNextToken();
            Assert.That(token.Lexeme, Is.EqualTo("123"));
            Assert.That(token.Type, Is.EqualTo(TokenType.NUMBER));
        }

        [Test]
        public void TestInvalidHexNumber_ReturnsError()
        {
            lexer.GetNextToken(); // skip "if"
            lexer.GetNextToken(); // skip "123"
            Token token = lexer.GetNextToken(); // 0x1G
            Assert.That(token.Lexeme, Is.EqualTo("0x1G"));
            Assert.That(token.Type, Is.EqualTo(TokenType.ERROR));
        }

        [Test]
        public void TestRecognizesStringToken()
        {
            lexer.GetNextToken(); // "if"
            lexer.GetNextToken(); // "123"
            lexer.GetNextToken(); // "0x1G"
            Token token = lexer.GetNextToken(); // "'test'"
            Assert.That(token.Lexeme, Is.EqualTo("'test'"));
            Assert.That(token.Type, Is.EqualTo(TokenType.STRING));
        }

        [Test]
        public void TestRecognizesComment()
        {
            for (int i = 0; i < 4; i++) lexer.GetNextToken(); // skip till #
            Token token = lexer.GetNextToken();
            StringAssert.StartsWith("#", token.Lexeme);
            Assert.That(token.Type, Is.EqualTo(TokenType.COMMENT));
        }

        [Test]
        public void TestTokenCollectionSequence()
        {
            var input = "return value42 3.14";
            var expected = new List<(string lexeme, TokenType type)>
            {
                ("return", TokenType.RESERVED),
                ("value42", TokenType.IDENTIFIER),
                ("3.14", TokenType.NUMBER),
            };

            var lex = new Lexer(input);
            var actual = new List<(string, TokenType)>();

            Token token;
            while ((token = lex.GetNextToken()).Type != TokenType.END)
            {
                actual.Add((token.Lexeme, token.Type));
            }

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestCase("0x10", TokenType.NUMBER)]
        [TestCase("while", TokenType.RESERVED)]
        [TestCase("\"abc\"", TokenType.STRING)]
        public void ParameterizedTest(string input, TokenType expectedType)
        {
            var lex = new Lexer(input);
            var token = lex.GetNextToken();
            Assert.That(token.Type, Is.EqualTo(expectedType));
        }

        [Test]
        public void TestExceptionOnNullInput()
        {
            Assert.Throws<System.ArgumentNullException>(() => new Lexer(null));
        }

        [TestCase("0x")]
        [TestCase("123abc")]
        [TestCase("1e")]
        [TestCase("1e+")]
        [TestCase("1.2e")]
        [TestCase("1e10jz")]
        public void TestUnrecognizedTokens(string invalidToken)
        {
            var lexer = new Lexer(invalidToken);
            var token = lexer.RecognizeNumber();

            Assert.That(token.Type, Is.EqualTo(TokenType.ERROR));
        }

        [Test]
        public void TestRecognizesDottedIdentifier()
        {
            var lex = new Lexer("module.var");
            Token token = lex.GetNextToken();

            Assert.That(token.Lexeme, Is.EqualTo("module.var"));
            Assert.That(token.Type, Is.EqualTo(TokenType.IDENTIFIER));
            Assert.That(token.Lexeme.Contains("."));
            Assert.That(token.Type, Is.Not.EqualTo(TokenType.ERROR));
        }

    }
}
