using MathExpressionResolver;
using Microsoft.Extensions.Logging;
using System;
using System.Globalization;
using System.Linq;

namespace JCalculator.Services;

public class CalculatorService : ICalculatorService
{
    private readonly SupportedOperators supportedOperators;
    private readonly ILogger<CalculatorService> logger;

    public CalculatorService(ILogger<CalculatorService> logger)
    {
        this.logger = logger;

        supportedOperators = new SupportedOperators();

        try
        {
            supportedOperators.Add(@operator: '+', priority: 0, leftAssociative: true, calculate: (a, b) => a + b, opposite: '-');
            supportedOperators.Add(@operator: '-', priority: 0, leftAssociative: true, calculate: (a, b) => a - b, opposite: '+');
            supportedOperators.Add(@operator: '*', priority: 1, leftAssociative: true, calculate: (a, b) => a * b);
            supportedOperators.Add(@operator: '×', priority: 1, leftAssociative: true, calculate: (a, b) => a * b);
            supportedOperators.Add(@operator: '/', priority: 1, leftAssociative: true, calculate: (a, b) => a / b);
            supportedOperators.Add(@operator: '÷', priority: 1, leftAssociative: true, calculate: (a, b) => a / b);
            supportedOperators.Add(@operator: '^', priority: 2, leftAssociative: false, calculate: (a, b) => Math.Pow(a, b));
            supportedOperators.Add(@operator: '%', priority: 3, leftAssociative: false, calculate: (a, b) => a * (b / 100));
        }
        catch (Exception ex)
        {
            logger.LogInformation(ex, nameof(CalculatorService));
        }
    }

    public bool TryCalculate(string expression, out string result)
    {
        try
        {
            var tokenizer = new Tokenizer(supportedOperators);
            var tokens = tokenizer.GetTokens(expression);
            var reversePolishNotation = ShuntingYard.Convert(tokens, tokenizer.Operators);
            result = ReversePolishNotationResolver.Calculate(reversePolishNotation, tokenizer.Operators).ToString();
            return true;
        }
        catch (Exception ex)
        {
            logger.LogInformation(ex, nameof(TryCalculate));
            result = string.Empty;
            return false;
        }
    }

    public bool TryInverseLastToken(string expression, out string result)
    {
        try
        {
            var tokenizer = new Tokenizer(supportedOperators);
            var tokens = tokenizer.GetTokens(expression).ToList();
            var lastToken = tokens[^1];
            var opposite = char.MinValue;

            switch (lastToken.Type)
            {
                case TokenType.Number:
                    if (tokens.Count > 1 && tokens[^2].Type == TokenType.Operator && supportedOperators.TryGetOpposite(tokens[^2].Value[0], out opposite))
                    {
                        var prevToken = tokens[^2];
                        prevToken.Value = opposite.ToString();
                        tokens[^2] = prevToken;
                    }
                    else
                    {
                        lastToken.Value = (-double.Parse(lastToken.Value, NumberFormatInfo.InvariantInfo)).ToString();
                        tokens[^1] = lastToken;
                    }

                    break;

                case TokenType.Operator when supportedOperators.TryGetOpposite(lastToken.Value[0], out opposite):
                    lastToken.Value = opposite.ToString();
                    tokens[^1] = lastToken;
                    break;

                default:
                    result = string.Empty;
                    return false;
            }

            result = string.Join(string.Empty, tokens.Select(t => t.Value));
            return true;
        }
        catch (Exception ex)
		{
			logger.LogInformation(ex, nameof(TryInverseLastToken));
			result = string.Empty;
            return false;
        }
    }

    public string DropLastToken(string expression)
    {
        try
        {
            var tokenizer = new Tokenizer(supportedOperators);
            var tokens = tokenizer.GetTokens(expression).ToList();

            return string.Join(string.Empty, tokens.Select(t => t.Value).Take(tokens.Count - 1));
        }
        catch (Exception ex)
        {
            logger.LogInformation(ex, nameof(DropLastToken));

            return expression;
        }
    }
}
