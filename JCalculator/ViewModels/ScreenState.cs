using CommunityToolkit.Mvvm.ComponentModel;

namespace JCalculator.ViewModels
{
	public partial class ScreenState : ObservableObject
	{
		public const string ResultDefault = "0";
		public const string ResultError = "?";
		public const string ExpressionDefault = "";

		[ObservableProperty]
		private string result = ResultDefault;

		[ObservableProperty]
		private string expression = string.Empty;
	}
}
