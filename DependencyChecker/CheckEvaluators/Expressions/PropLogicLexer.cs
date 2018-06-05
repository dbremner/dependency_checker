//===============================================================================
// Microsoft patterns & practices
// Dependency Checker
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the
// Microsoft patterns & practices license (http://dependencychecker.codeplex.com/license)
//===============================================================================

namespace DependencyChecker.CheckEvaluators.Expressions
{
    using System;
    using System.Text.RegularExpressions;

    internal class PropLogicLexer
    {
        private readonly Regex identifierRegex;

        private string source;

        public PropLogicLexer(string source)
        {
            this.source = source;
            CurrentTokenType = LexerTokenType.NotStarted;
            CurrentTokenValue = null;
            identifierRegex = new Regex("^[A-Za-z_][0-9A-Za-z_]*", RegexOptions.Compiled);
        }

        public LexerTokenType CurrentTokenType { get; private set; }

        public string CurrentTokenValue { get; private set; }

        public bool NextToken()
        {
            if (CurrentTokenType == LexerTokenType.End)
            {
                return false;
            }
            EatLeadingWhiteSpace();
            if (source.Length == 0)
            {
                CurrentTokenType = LexerTokenType.End;
                CurrentTokenValue = string.Empty;
                return false;
            }

            if (MatchesKeyword("&&", LexerTokenType.And))
            {
                return true;
            }
            if (MatchesKeyword("||", LexerTokenType.Or))
            {
                return true;
            }
            if (MatchesKeyword("!", LexerTokenType.Not))
            {
                return true;
            }
            if (MatchesKeyword("(", LexerTokenType.OpenParen))
            {
                return true;
            }
            if (MatchesKeyword(")", LexerTokenType.CloseParen))
            {
                return true;
            }
            if (MatchesIdentifier())
            {
                return true;
            }

            throw new ArgumentException("Invalid input stream in source");
        }

        private void EatLeadingWhiteSpace()
        {
            source = source.TrimStart(new[] { ' ', '\t' });
        }

        private bool MatchesIdentifier()
        {
            Match match = identifierRegex.Match(source);
            if (match.Success)
            {
                CurrentTokenType = LexerTokenType.Identifier;
                CurrentTokenValue = source.Substring(0, match.Length);
                source = source.Substring(match.Length);
                return true;
            }
            return false;
        }

        private bool MatchesKeyword(string keyword, LexerTokenType tokenType)
        {
            if (source.StartsWith(keyword))
            {
                CurrentTokenType = tokenType;
                CurrentTokenValue = keyword;
                source = source.Substring(keyword.Length);
                return true;
            }
            return false;
        }
    }
}