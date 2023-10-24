namespace JCalculator.ViewModels;

public interface ICalculatorService
{
	bool TryCalculate(string expression, out string result);

	bool TryInverseLastToken(string expression, out string result);

	string DropLastToken(string expression);
}
