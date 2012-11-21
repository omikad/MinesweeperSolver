using System;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;

namespace WinmineSolver
{
	public partial class App
	{
		private void App_OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
		{
			MessageBox.Show(e.Exception.ToString());
			e.Handled = true;
			Environment.Exit(-1);
		}

		private void App_OnStartup(object sender, StartupEventArgs e)
		{
			var catalog = new AssemblyCatalog(Assembly.GetExecutingAssembly());
			var container = new CompositionContainer(catalog);
			var mainWindow = container.GetExportedValue<MainWindow>();
			mainWindow.Show();
		}
	}
}
