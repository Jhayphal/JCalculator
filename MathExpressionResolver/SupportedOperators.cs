namespace MathExpressionResolver;

public sealed class SupportedOperators
{
	private readonly HashSet<OperatorInfo> Operators = new();

	public bool Add(char @operator, int priority, bool leftAssociative, Func<double, double, double> calculate, char? opposite = null)
	{
		var newItem = new OperatorInfo(@operator, priority, leftAssociative, calculate, opposite);

		return Operators.Add(newItem);
	}

	public double Calculate(char @operator, double a, double b)
	{
		var info = GetOperator(@operator);

		return info.Calculate(a, b);
	}

	public int CompareTo(char operatorToCompare, char compareWith) => GetOperator(operatorToCompare).CompareTo(GetOperator(compareWith));

	public int GetOperatorPriority(char @operator) => GetOperator(@operator).Priority;

	public bool IsLeftAssociative(char @operator) => GetOperator(@operator).LeftAssociative;

	public bool TryGetOpposite(char @operator, out char result)
	{
		var opposite = GetOperator(@operator).Opposite;
		result = opposite ?? char.MinValue;
		return opposite is not null;
	}

	public bool IsSupported(char @operator)
	{
		foreach (var item in Operators)
			if (item.Equals(@operator))
				return true;

		return false;
	}

	private OperatorInfo GetOperator(char @operator)
	{
		foreach (var item in Operators)
			if (item.Equals(@operator))
				return item;

		throw new ArgumentException($"Оператора '{@operator}' не существует.");
	}
}
