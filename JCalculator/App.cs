using JCalculator.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace JCalculator
{
	public sealed class App : Application
	{
		private readonly MainWindow window;

		public App(MainWindow window)
		{
			this.window = window;
		}

		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			window.Show();
		}
	}
}
