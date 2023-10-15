using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace JCalculator.ViewModels
{
	internal sealed class InputHelper
	{
		private readonly MainWindowViewModel viewModel;
		private TextBox? text;

        public InputHelper(MainWindowViewModel mainWindowViewModel)
        {
			viewModel = mainWindowViewModel;
        }

        public TextBox Text
		{
			get
			{
				if (text is null)
				{
					text = (TextBox)FocusManager.GetFocusedElement(((App)Application.Current).Window);
				}

				return text;
			}
		}

		public void Set(string value)
		{
			viewModel.Value = value;
			Text.CaretIndex = value.Length;
		}

		public void Insert(string @char)
		{
			if (Text.SelectionLength > 0)
			{
				var newCaretIndex = Text.SelectionStart;
				viewModel.Value = viewModel.Value.Remove(newCaretIndex, Text.SelectionLength);
				Text.CaretIndex = newCaretIndex;
			}

			var lastCaretIndex = Text.CaretIndex;
			viewModel.Value = viewModel.Value.Insert(lastCaretIndex, @char);
			Text.CaretIndex = lastCaretIndex + 1;
		}

		public void Delete()
		{
			if (Text.SelectionLength > 0)
			{
				var newCaretIndex = Text.SelectionStart;
				viewModel.Value = viewModel.Value.Remove(newCaretIndex, Text.SelectionLength);
				Text.CaretIndex = newCaretIndex;
			}
			else
			{
				var lastCaretIndex = Text.CaretIndex;
				viewModel.Value = viewModel.Value.Remove(lastCaretIndex - 1, 1);
				Text.CaretIndex = lastCaretIndex - 1;
			}
		}
	}
}
