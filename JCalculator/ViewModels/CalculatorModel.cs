using Microsoft.Extensions.Logging;
using System;
using System.Windows;

namespace JCalculator.ViewModels
{
	public class CalculatorModel
	{
		private readonly ICalculatorService calculatorService;
		private readonly ILogger<MainWindowViewModel> logger;
		private readonly InputHelper expressionEditor;

        public CalculatorModel(ICalculatorService calculatorService, ILogger<MainWindowViewModel> logger)
        {
			this.calculatorService = calculatorService;
			this.logger = logger;

			expressionEditor = new InputHelper(Screen);

			Screen.PropertyChanged += (s, e) =>
			{
				if (e.PropertyName == nameof(ScreenState.Expression))
				{
					OnExpressionChanged((ScreenState)s!);
				}
			};
		}

		public readonly ScreenState Screen = new();

		public void Insert(string @char) => expressionEditor.Insert(@char);

		public void Clear()
		{
			Screen.Expression = ScreenState.ExpressionDefault;
			Screen.Result = ScreenState.ResultDefault;
		}

		public bool CanClear()
			=> Screen.Expression != ScreenState.ExpressionDefault || Screen.Result != ScreenState.ResultDefault;

		public void PasteExpression()
			=> expressionEditor.Text.Paste();

		public void CopyResult()
			=> Clipboard.SetText(Screen.Result);

		public void DropLastToken()
			=> expressionEditor.Set(calculatorService.DropLastToken(Screen.Expression));

		public bool CanDropLastToken()
			=> Screen.Expression.Length > 0;

		public void Delete()
			=> expressionEditor.Delete();

		public bool CanDelete()
			=> Screen.Expression.Length > 0;

		public void Inverse()
		{
			if (calculatorService.TryInverseLastToken(Screen.Expression, out var result))
			{
				expressionEditor.Set(result);
			}
		}

		public bool CanInverse()
			=> Screen.Expression.Length > 0;

		public void GetResult()
			=> expressionEditor.Set(Screen.Result);

		public bool CanGetResult()
			=> Screen.Result != ScreenState.ResultDefault;

		private void OnExpressionChanged(ScreenState screen)
		{
			try
			{
				if (calculatorService.TryCalculate(screen.Expression, out var result))
				{
					Screen.Result = result;
				}
				else if (calculatorService.TryCalculate(screen.Expression[..^1], out result))
				{
					Screen.Result = result;
				}
				else
				{
					Screen.Result = ScreenState.ResultError;
				}
			}
			catch(Exception ex)
			{
				logger.LogError(ex, nameof(OnExpressionChanged));
			}
		}
	}
}
