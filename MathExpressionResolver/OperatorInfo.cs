﻿namespace MathExpressionResolver
{
	internal class OperatorInfo : IEquatable<OperatorInfo>, IComparable<OperatorInfo>
	{
		public readonly string Operator;

		public readonly int Priority;

		public readonly bool LeftAssociative;

		public readonly Func<double, double, double> Calculate;

		public readonly string? Opposite;

		public OperatorInfo(string @operator, int priority, bool leftAssociative, Func<double, double, double> calculate, string? opposite = null)
		{
			if (string.IsNullOrWhiteSpace(@operator))
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

			if (obj is string @operatorString)
				return Equals(@operatorString);

			if (obj is char @operatorChar)
				return Equals(@operatorChar);

			return Equals(obj.ToString()!);
		}

		public bool Equals(OperatorInfo? other)
		{
			if (other == null)
				return false;

			return Equals(other.Operator);
		}

		public bool Equals(string other) => string.Equals(Operator, other);

		public bool Equals(char other) => Equals(other.ToString());

		public override int GetHashCode() => Operator.GetHashCode();

		public override string ToString() => Operator;
	}
}
