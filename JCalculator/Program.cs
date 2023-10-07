using System;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using JCalculator.Views;

namespace JCalculator
{
	public sealed class Program
	{
		[STAThread]
		public static void Main(string[] args)
		{
			var host = Host.CreateDefaultBuilder(args)
				.ConfigureServices(services =>
				{
					services.AddSingleton<App>();
					services.AddSingleton<MainWindow>();
				})
				.Build();

			var app = host.Services.GetService<App>();
			app?.Run();
		}
	}
}
