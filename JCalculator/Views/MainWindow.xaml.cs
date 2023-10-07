using JCalculator.ViewModels;
using System.Windows;

namespace JCalculator.Views
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow(MainWindowViewModel viewModel)
		{
			InitializeComponent();

			DataContext = viewModel;
		}
	}
}
