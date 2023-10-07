using System;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using JCalculator.Views;
using JCalculator.ViewModels;

namespace JCalculator
{
	public sealed class Program
	{
		[STAThread]
		public static void Main(string[] args)
		{
			var host = Host.CreateDefaultBuilder(args)
				.ConfigureServices(RegisterServices)
				.Build();

			var app = host.Services.GetService<App>();
			app?.Run();
		}

		private static void RegisterServices(IServiceCollection services)
		{
			services.AddSingleton<App>();
			services.AddSingleton<MainWindow>();
			services.AddTransient<MainWindowViewModel>();
			services.AddTransient<ICalculatorService, CalculatorService>();
		}
	}
}
