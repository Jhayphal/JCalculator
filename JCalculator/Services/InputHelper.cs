using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using JCalculator.ViewModels;

namespace JCalculator.Services;

public sealed class InputHelper
{
    private readonly ScreenViewModel viewModel;
    private TextBox? text;

    public InputHelper(ScreenViewModel mainWindowViewModel)
    {
        viewModel = mainWindowViewModel;
    }

    public TextBox Text
        => text ??= (TextBox)FocusManager.GetFocusedElement(((App)Application.Current).Window);

    public void Set(string value)
    {
        viewModel.Expression = value;
        Text.CaretIndex = value.Length;
    }

    public void Insert(string @char)
    {
        if (Text.SelectionLength > 0)
        {
            var newCaretIndex = Text.SelectionStart;
            viewModel.Expression = viewModel.Expression.Remove(newCaretIndex, Text.SelectionLength);
            Text.CaretIndex = newCaretIndex;
        }

        var lastCaretIndex = Text.CaretIndex;
        viewModel.Expression = viewModel.Expression.Insert(lastCaretIndex, @char);
        Text.CaretIndex = lastCaretIndex + 1;
    }

    public void Delete()
    {
        if (Text.SelectionLength > 0)
        {
            var newCaretIndex = Text.SelectionStart;
            viewModel.Expression = viewModel.Expression.Remove(newCaretIndex, Text.SelectionLength);
            Text.CaretIndex = newCaretIndex;
        }
        else
        {
            var lastCaretIndex = Text.CaretIndex;
            viewModel.Expression = viewModel.Expression.Remove(lastCaretIndex - 1, 1);
            Text.CaretIndex = lastCaretIndex - 1;
        }
    }
}
