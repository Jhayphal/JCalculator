namespace MathExpressionResolver
{
	internal class OperatorInfo : IEquatable<OperatorInfo>, IComparable<OperatorInfo>
	{
		public readonly char Operator;

		public readonly int Priority;

		public readonly bool LeftAssociative;

		public readonly Func<double, double, double> Calculate;

		public readonly char? Opposite;

		public OperatorInfo(char @operator, int priority, bool leftAssociative, Func<double, double, double> calculate, char? opposite = null)
		{
			if (char.IsWhiteSpace(@operator))
				throw new ArgumentException(nameof(@operator));

			Operator = @operator;
			Priority = priority;
			LeftAssociative = leftAssociative;
			Opposite = opposite;

			Calculate = calculate
				?? throw new ArgumentNullException(nameof(calculate));
		}

		public int CompareTo(OperatorInfo? other)
		{
			if (other == null)
				throw new ArgumentNullException(nameof(other));

			return Priority.CompareTo(other.Priority);
		}

		public override bool Equals(object? obj)
		{
			if (obj == null)
				return false;

			if (obj is OperatorInfo @operator)
				return Equals(@operator.Operator);

			if (obj is char @operatorChar)
				return Equals(@operatorChar);

			return false;
		}

		public bool Equals(OperatorInfo? other)
		{
			if (other == null)
				return false;

			return Equals(other.Operator);
		}

		public bool Equals(char other) => Operator == other;

		public override int GetHashCode() => Operator.GetHashCode();

		public override string ToString() => Operator.ToString();
	}
}
