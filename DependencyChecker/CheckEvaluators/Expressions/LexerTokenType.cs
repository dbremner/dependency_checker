namespace DependencyChecker.CheckEvaluators.Expressions
{
    internal enum LexerTokenType
    {
        NotStarted = 0,
        Identifier,
        And,
        Or,
        Not,
        OpenParen,
        CloseParen,
        End
    }
}