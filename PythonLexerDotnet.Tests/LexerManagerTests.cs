using NSubstitute;
using python_lexer_dotnet;

namespace PythonLexerDotnet.Tests
{
    [TestFixture]
    public class LexerManagerTests
    {
        private ILexer _mockLexer;
        private LexerManager _lexerManager;

        [SetUp]
        public void SetUp()
        {
            _mockLexer = Substitute.For<ILexer>();
            _lexerManager = new LexerManager(_mockLexer);
        }

        [Test]
        public void ProcessCode_CallsMethodsInCorrectOrder()
        {
            // Arrange
            var tokens = new Queue<Token>(new[]
            {
            new Token { Lexeme = "print", Type = TokenType.IDENTIFIER },
            new Token { Lexeme = "(", Type = TokenType.PUNCTUATION },
            new Token { Lexeme = "Hello, World!", Type = TokenType.STRING },
            new Token { Lexeme = ")", Type = TokenType.PUNCTUATION },
            new Token { Lexeme = "", Type = TokenType.END },
        });

            _mockLexer.GetNextToken().Returns(_ => tokens.Dequeue());

            // Act
            _lexerManager.ProcessCode("print('Hello, World!')");

            // Assert
            Received.InOrder(() =>
            {
                _mockLexer.GetCodeForAnalyze("print('Hello, World!')");
                _mockLexer.GetNextToken();
                _mockLexer.GetNextToken();
                _mockLexer.GetNextToken();
                _mockLexer.GetNextToken();
                _mockLexer.GetNextToken();
            });
        }
        [Test]
        public void ProcessCode_WhenErrprToken_LogsErrorWithPosition()
        {
            // Arrange
            var tokens = new Queue<Token>(new[]
            {
                new Token { Lexeme = "@", Type = TokenType.ERROR },
                new Token { Lexeme = "", Type = TokenType.END },
            });

            _mockLexer.GetNextToken().Returns(_ => tokens.Dequeue());
            _mockLexer.GetPosition().Returns(42);

            // Act
            _lexerManager.ProcessCode("@");

            // Assert
            _mockLexer.Received(1).GetPosition();
        }
        [Test]
        public void ProcessCode_ThrowsExceptionFromLexer()
        {
            // Arrange
            _mockLexer.When(x => x.GetCodeForAnalyze(Arg.Any<string>()))
                .Do(x => throw new InvalidOperationException("Lexer failed"));

            // Act & Assert
            Assert.DoesNotThrow(() => _lexerManager.ProcessCode("some code"));
        }
        [Test]
        public void ProcessCode_ParameterMatchingBehavior()
        {
            // Arrange
            _mockLexer.When(x => x.GetCodeForAnalyze(Arg.Is<string>(s => s.Contains("print"))))
                      .Do(x => { /* Simulate successful analysis */ });

            _mockLexer.GetNextToken().Returns(new Token { Lexeme = "", Type = TokenType.END });

            // Act
            _lexerManager.ProcessCode("print('hello')");

            // Assert
            _mockLexer.Received().GetCodeForAnalyze(Arg.Is<string>(s => s.Contains("print")));
        }
        [Test]
        public void ProcessCode_MultipleReturnValues()
        {
            // Arrange
            _mockLexer.GetNextToken().Returns(
                new Token { Lexeme = "x", Type = TokenType.IDENTIFIER },
                new Token { Lexeme = "=", Type = TokenType.OPERATOR },
                new Token { Lexeme = "5", Type = TokenType.NUMBER },
                new Token { Lexeme = "", Type = TokenType.END }
            );

            // Act
            _lexerManager.ProcessCode("x = 5");

            // Assert
            _mockLexer.Received(4).GetNextToken(); // 3 tokens + END
        }
    }
}
