using JCalculator.Views;
using Microsoft.Extensions.Logging;
using System.Windows;

namespace JCalculator
{
	public sealed class App : Application
	{
		private readonly MainWindow window;

		public App(MainWindow window, ILogger<App> logger)
		{
			this.window = window;
			DispatcherUnhandledException += (sender, e) 
				=> logger.LogCritical(e.Exception.ToString());
		}

		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			window.Show();
		}
	}
}
