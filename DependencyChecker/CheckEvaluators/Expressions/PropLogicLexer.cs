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

    internal class PropLogicLexer
    {
        private readonly Regex identifierRegex;

        private string source;

        public PropLogicLexer(string source)
        {
            this.source = source;
            this.CurrentTokenType = LexerTokenType.NotStarted;
            this.CurrentTokenValue = null;
            this.identifierRegex = new Regex("^[A-Za-z_][0-9A-Za-z_]*");
        }

        public LexerTokenType CurrentTokenType { get; private set; }

        public string CurrentTokenValue { get; private set; }

        public bool NextToken()
        {
            if (this.CurrentTokenType == LexerTokenType.End)
            {
                return false;
            }
            this.EatLeadingWhiteSpace();
            if (this.source.Length == 0)
            {
                this.CurrentTokenType = LexerTokenType.End;
                this.CurrentTokenValue = string.Empty;
                return false;
            }

            if (this.MatchesKeyword("&&", LexerTokenType.And))
            {
                return true;
            }
            if (this.MatchesKeyword("||", LexerTokenType.Or))
            {
                return true;
            }
            if (this.MatchesKeyword("!", LexerTokenType.Not))
            {
                return true;
            }
            if (this.MatchesKeyword("(", LexerTokenType.OpenParen))
            {
                return true;
            }
            if (this.MatchesKeyword(")", LexerTokenType.CloseParen))
            {
                return true;
            }
            if (this.MatchesIdentifier())
            {
                return true;
            }

            throw new ArgumentException("Invalid input stream in source");
        }

        private void EatLeadingWhiteSpace()
        {
            this.source = this.source.TrimStart(new[] { ' ', '\t' });
        }

        private bool MatchesIdentifier()
        {
            Match match = this.identifierRegex.Match(this.source);
            if (match.Success)
            {
                this.CurrentTokenType = LexerTokenType.Identifier;
                this.CurrentTokenValue = this.source.Substring(0, match.Length);
                this.source = this.source.Substring(match.Length);
                return true;
            }
            return false;
        }

        private bool MatchesKeyword(string keyword, LexerTokenType tokenType)
        {
            if (this.source.StartsWith(keyword))
            {
                this.CurrentTokenType = tokenType;
                this.CurrentTokenValue = keyword;
                this.source = this.source.Substring(keyword.Length);
                return true;
            }
            return false;
        }
    }
}