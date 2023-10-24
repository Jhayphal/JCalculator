using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;

namespace JCalculator.ViewModels
{
	public sealed partial class MainWindowViewModel : INotifyPropertyChanged
	{
		private readonly CalculatorModel model;

		public MainWindowViewModel(CalculatorModel model)
		{
			this.model = model;

			InsertCommand = new RelayCommand<string>(model.Insert!);
			ClearCommand = new RelayCommand(model.Clear, model.CanClear);
			PasteExpressionCommand = new RelayCommand(model.PasteExpression);
			CopyResultCommand = new RelayCommand(model.CopyResult);
			DropLastTokenCommand = new RelayCommand(model.DropLastToken, model.CanDropLastToken);
			DeleteCommand = new RelayCommand(model.Delete, model.CanDelete);
			InverseCommand = new RelayCommand(model.Inverse, model.CanInverse);
			GetResultCommand = new RelayCommand(model.GetResult, model.CanGetResult);

			this.model.Screen.PropertyChanged += Screen_PropertyChanged;
		}

		public string Expression
		{
			get => model.Screen.Expression;
			set => model.Screen.Expression = value;
		}

		public string Result
		{
			get => model.Screen.Result;
			set => model.Screen.Result = value;
		}

		public IRelayCommand<string> InsertCommand { get; }

		public IRelayCommand ClearCommand { get; }

		public IRelayCommand PasteExpressionCommand { get; }

		public IRelayCommand CopyResultCommand { get; }

		public IRelayCommand DropLastTokenCommand { get; }

		public IRelayCommand DeleteCommand { get; }

		public IRelayCommand InverseCommand { get; }

		public IRelayCommand GetResultCommand { get; }

		private void Screen_PropertyChanged(object? sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof(ScreenState.Result))
			{
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Result)));

				GetResultCommand.NotifyCanExecuteChanged();
				CopyResultCommand.NotifyCanExecuteChanged();
				ClearCommand.NotifyCanExecuteChanged();
			}
			else if (e.PropertyName == nameof(ScreenState.Expression))
			{
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Expression)));

				DeleteCommand.NotifyCanExecuteChanged();
				DropLastTokenCommand.NotifyCanExecuteChanged();
				InverseCommand.NotifyCanExecuteChanged();
				ClearCommand.NotifyCanExecuteChanged();
			}
		}

		public event PropertyChangedEventHandler? PropertyChanged;
	}
}
