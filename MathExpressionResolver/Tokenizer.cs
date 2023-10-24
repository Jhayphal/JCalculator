﻿using System.Text;

namespace MathExpressionResolver
{
	public class Tokenizer
	{
		private enum TokenizerContext
		{
			Number,
			Other
		}

		public Tokenizer(SupportedOperators supportedOperators)
		{
			Operators = supportedOperators ?? throw new ArgumentNullException(nameof(supportedOperators));
		}

		public readonly SupportedOperators Operators;

		public IEnumerable<(TokenType Type, string Value)> GetTokens(string expression)
		{
			StringBuilder cache = new();

			TokenizerContext context = TokenizerContext.Other;
			foreach(var lexeme in expression)
			{
				if (lexeme == '(')
				{
					context = TokenizerContext.Other;

					foreach(var result in GetNumberAndClearCache(cache))
					{
						yield return result;
					}

					yield return (TokenType.OpenBracket, "(");
				}
				else if (lexeme == ')')
				{
					context = TokenizerContext.Other;

					foreach (var result in GetNumberAndClearCache(cache))
					{
						yield return result;
					}

					yield return (TokenType.CloseBracket, ")");
				}
				else if (Operators.IsSupported(lexeme) && (lexeme != '-' || context == TokenizerContext.Number))
				{
					context = TokenizerContext.Other;

					foreach (var result in GetNumberAndClearCache(cache))
					{
						yield return result;
					}

					yield return (TokenType.Operator, lexeme.ToString());
				}
				else
				{
					context = TokenizerContext.Number;

					if (lexeme == ',')
					{
						cache.Append('.');
					}
					else
					{
						cache.Append(lexeme);
					}
				}
			}

			foreach (var result in GetNumberAndClearCache(cache))
			{
				yield return result;
			}
		}

		private static IEnumerable<(TokenType Type, string Value)> GetNumberAndClearCache(StringBuilder cache)
		{
			if (cache.Length == 0)
			{
				yield break;
			}

			var result = cache.ToString();
			cache.Clear();

			if (result == "-")
			{
				yield return (TokenType.Number, "-1");
				yield return (TokenType.Operator, "*");
			}
			else
			{
				yield return (TokenType.Number, result);
			}
		}
	}
}
