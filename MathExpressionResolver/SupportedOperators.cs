namespace MathExpressionResolver
{
	public sealed class SupportedOperators
	{
		private readonly HashSet<OperatorInfo> Operators = new HashSet<OperatorInfo>();

		public bool Add(string @operator, int priority, bool leftAssociative, Func<double, double, double> calculate, string? opposite = null)
		{
			var newItem = new OperatorInfo(@operator, priority, leftAssociative, calculate, opposite);

			return Operators.Add(newItem);
		}

		public double Calculate(string @operator, double a, double b)
		{
			var info = GetOperator(@operator);

			return info.Calculate(a, b);
		}

		public int CompareTo(string operatorToCompare, string compareWith) => GetOperator(operatorToCompare).CompareTo(GetOperator(compareWith));

		public int GetOperatorPriority(char @operator) => GetOperatorPriority(@operator.ToString());

		public int GetOperatorPriority(string @operator) => GetOperator(@operator).Priority;

		public bool IsLeftAssociative(char @operator) => IsLeftAssociative(@operator.ToString());

		public bool IsLeftAssociative(string @operator) => GetOperator(@operator).LeftAssociative;

		public bool TryGetOpposite(string @operator, out string result)
		{
			var opposite = GetOperator(@operator).Opposite;
			result = opposite ?? string.Empty;
			return opposite is not null;
		}

		public bool IsSupported(char @operator) => IsSupported(@operator.ToString());

		public bool IsSupported(string @operator)
		{
			foreach (var item in Operators)
				if (item.Equals(@operator))
					return true;

			return false;
		}

		private OperatorInfo GetOperator(string @operator)
		{
			foreach (var item in Operators)
				if (item.Equals(@operator))
					return item;

			throw new ArgumentException($"Оператора '{@operator}' не существует.");
		}
	}
}
