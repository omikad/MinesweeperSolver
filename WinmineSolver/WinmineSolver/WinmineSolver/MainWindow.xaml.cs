using System.ComponentModel.Composition;
using System.Windows;
using WinmineSolver.Mouse;

namespace WinmineSolver
{
	[Export]
	public partial class MainWindow
	{
		private readonly IMouseHookSaveToFile mouseHookSaveToFile;
		private readonly IInputGenerator inputGenerator;
		private readonly IEyes eyes;

		[ImportingConstructor]
		public MainWindow(
			IMouseHookSaveToFile mouseHookSaveToFile, 
			IInputGenerator inputGenerator,
			IEyes eyes,
			IBoardLogger boardLogger)
		{
			this.inputGenerator = inputGenerator;
			this.eyes = eyes;
			this.mouseHookSaveToFile = mouseHookSaveToFile;
			
			InitializeComponent();

			boardLogger.SetOutput(textbox);
		}

		private void buttonGoClick(object sender, RoutedEventArgs e)
		{
			for (var i = 0; i < 10; i++)
			{
				eyes.Look();
			}
		}

		private void buttonSaveMouseClick(object sender, RoutedEventArgs e)
		{
			mouseHookSaveToFile.Hook();
		}

		private void buttonStopMouseClick(object sender, RoutedEventArgs e)
		{
			mouseHookSaveToFile.Unhook();
		}

		private void buttonShowMousePropertiesClick(object sender, RoutedEventArgs e)
		{
			MessageBox.Show(inputGenerator.ToString());
		}

		private void buttonReadBoardClick(object sender, RoutedEventArgs e)
		{
			eyes.LookFleetly();
		}
	}
}
