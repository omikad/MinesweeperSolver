using System.ComponentModel.Composition;
using WinmineSolver.Mouse;

namespace WinmineSolver
{
	[InheritedExport]
	public interface IHands
	{
		void ClickToNewGame(ViewInfo viewInfo);
		void Click(ViewInfo viewInfo, int x, int y);
	}

	public class Hands : IHands
	{
		private readonly IInputGenerator inputGenerator;
		private readonly IScreenCapturer screenCapturer;

		[ImportingConstructor]
		public Hands(IInputGenerator inputGenerator, IScreenCapturer screenCapturer)
		{
			this.inputGenerator = inputGenerator;
			this.screenCapturer = screenCapturer;
		}

		public void ClickToNewGame(ViewInfo viewInfo)
		{
			var lu = screenCapturer.GetWindowLUPoint(viewInfo.Hwnd);
			inputGenerator.MoveMouseAbsolute((int)(lu.X + 260), (int)(lu.Y + 80));
			inputGenerator.DoMouseClick();
		}

		public void Click(ViewInfo viewInfo, int x, int y)
		{
			var lu = screenCapturer.GetWindowLUPoint(viewInfo.Hwnd);
			inputGenerator.MoveMouseAbsolute((int)(lu.X + 23 + viewInfo.CellSize * x), (int)(lu.Y + 108 + viewInfo.CellSize * y));
			inputGenerator.DoMouseClick();
		}
	}
}