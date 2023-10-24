using JCalculator.Views;
using Microsoft.Extensions.Logging;
using System.Windows;

namespace JCalculator;

public sealed class App : Application
{
	public readonly MainWindow Window;

	public App(MainWindow window, ILogger<App> logger)
	{
		Window = window;
		DispatcherUnhandledException += (sender, e) 
			=> logger.LogCritical(e.Exception.ToString());
	}

	protected override void OnStartup(StartupEventArgs e)
	{
		base.OnStartup(e);

		Window.Show();
	}
}
