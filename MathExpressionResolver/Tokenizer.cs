using System.Globalization;
using System.Text;

namespace MathExpressionResolver;

public class Tokenizer
{
	private enum TokenizerContext
	{
		Number,
		Other
	}

	private static readonly string NumberDecimalSeparator = NumberFormatInfo.CurrentInfo.NumberDecimalSeparator;

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

				cache.Append(lexeme);
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

		if (NumberDecimalSeparator != ",")
		{
			cache.Replace(",", NumberDecimalSeparator);
		}
		else if (NumberDecimalSeparator != ".")
		{
			cache.Replace(".", NumberDecimalSeparator);
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
