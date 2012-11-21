using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;

namespace WinmineSolver.Mouse
{
	[InheritedExport]
	public interface IMouseHookSaveToFile
	{
		void Hook();
		void Unhook();
	}

	public class MouseHookSaveToFile : MouseHook, IMouseHookSaveToFile
	{
		private readonly string outputFileName;

		public MouseHookSaveToFile()
			: this("mouse_out.txt")
		{
		}

		public MouseHookSaveToFile(string outputFileName)
		{
			this.outputFileName = outputFileName;
			action = Action;
		}

		private FileStream fs;

		private void Action(MouseLLHookStruct data, MouseMessages message)
		{
			var strCaption = string.Format("time = {3}, x = {0}, y = {1}, flags = {4}, message = {2}\r\n",
				data.pt.x.ToString("d"),
				data.pt.y.ToString("d"),
				(int)message,
				data.time,
				data.flags);
			var bytes = Encoding.ASCII.GetBytes(strCaption);
			fs.Write(bytes, 0, bytes.Length);
		}

		[Export(typeof(Func<string, MouseMovementInfo>))]
		public static MouseMovementInfo GetMovementInfoFromString(string s)
		{
			var parts = s.Split(new[] { ',', '=' }, StringSplitOptions.RemoveEmptyEntries).Select(ss => ss.Trim()).ToArray();
			if (parts.Length != 10)
				throw new FormatException(s);

			var time = int.Parse(parts[1]);
			var x = int.Parse(parts[3]);
			var y = int.Parse(parts[5]);
			var type = (MouseMessages)int.Parse(parts[9]);

			return new MouseMovementInfo { Time = time, Type = type, X = x, Y = y };
		}

		public override void Hook()
		{
			base.Hook();
			if (fs != null)
				fs.Close();
			fs = File.Open(outputFileName, FileMode.Create, FileAccess.ReadWrite, FileShare.None);
		}

		public override void Unhook()
		{
			if (fs != null)
				fs.Close();
			base.Unhook();
		}
	}
}
