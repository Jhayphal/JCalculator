using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;

namespace JCalculator.ViewModels
{
	public sealed partial class MainWindowViewModel : ObservableObject
	{
		private const string ExecutedExpressionDefault = "0";
		private const string ExecutedExpressionError = "?";
		private const string ValueDefault = "";

		private readonly ICalculatorService calculator;
		private readonly ILogger<MainWindowViewModel> logger;

		[ObservableProperty]
		private string executedExpression = ExecutedExpressionDefault;

		[ObservableProperty]
		[NotifyCanExecuteChangedFor(nameof(ClearCommand))]
		[NotifyCanExecuteChangedFor(nameof(DropLastTokenCommand))]
		[NotifyCanExecuteChangedFor(nameof(DeleteCommand))]
		[NotifyCanExecuteChangedFor(nameof(InverseCommand))]
		private string value = string.Empty;

        public MainWindowViewModel(ICalculatorService calculatorService, ILogger<MainWindowViewModel> logger)
        {
			calculator = calculatorService;
			this.logger = logger;
        }

		[RelayCommand]
		private void Push(string @char) => Value += @char;

		[RelayCommand(CanExecute = nameof(CanClear))]
		private void Clear()
		{
			Value = ValueDefault;
			ExecutedExpression = ExecutedExpressionDefault;
		}

		private bool CanClear()
			=> Value != ValueDefault || ExecutedExpression != ExecutedExpressionDefault;

		[RelayCommand(CanExecute = nameof(CanDropLastToken))]
		private void DropLastToken()
		{
			Value = calculator.DropLastToken(Value);
		}

		private bool CanDropLastToken() => Value.Length > 0;

		[RelayCommand(CanExecute = nameof(CanDelete))]
		private void Delete() => Value = Value[..^1];

		private bool CanDelete() => Value.Length > 0;

		[RelayCommand(CanExecute = nameof(CanInverse))]
		private void Inverse()
		{
			if (calculator.TryInverseLastToken(Value, out var result))
			{
				Value = result;
			}
		}

		private bool CanInverse() => Value.Length > 0;

		partial void OnValueChanged(string value)
		{
			if (calculator.TryCalculate(Value, out var result))
			{
				ExecutedExpression = result;
			}
			else if (calculator.TryCalculate(Value[..^1], out result))
			{
				ExecutedExpression = result;
			}
			else
			{
				ExecutedExpression = ExecutedExpressionError;
			}
		}
	}
}
