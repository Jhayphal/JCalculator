using JCalculator.Services;
using JCalculator.ViewModels;
using Microsoft.Extensions.Logging;
using System;
using System.Windows;

namespace JCalculator.Models;

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
            if (e.PropertyName == nameof(ScreenViewModel.Expression))
            {
                OnExpressionChanged((ScreenViewModel)s!);
            }
        };
    }

    public readonly ScreenViewModel Screen = new();

    public void Insert(string @char) => expressionEditor.Insert(@char);

    public void Clear()
    {
        Screen.Expression = ScreenViewModel.ExpressionDefault;
        Screen.Result = ScreenViewModel.ResultDefault;
    }

    public bool CanClear()
        => Screen.Expression != ScreenViewModel.ExpressionDefault || Screen.Result != ScreenViewModel.ResultDefault;

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
        => Screen.Result != ScreenViewModel.ResultDefault;

    private void OnExpressionChanged(ScreenViewModel screen)
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
                Screen.Result = ScreenViewModel.ResultError;
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, nameof(OnExpressionChanged));
        }
    }
}
