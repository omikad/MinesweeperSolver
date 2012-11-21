namespace WinmineSolver.Mouse
{
	public interface IInputGenerator
	{
		void MoveMouseAbsolute(int x, int y);
		void DoMouseClick();
	}
}