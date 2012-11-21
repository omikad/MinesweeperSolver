using System.ComponentModel.Composition;
using System.Windows.Controls;
using System.Windows.Documents;

namespace WinmineSolver
{
	[InheritedExport]
	public interface IBoardLogger
	{
		void SetOutput(RichTextBox _textbox);
		void Log(Board board);
	}

	public class BoardLogger : IBoardLogger
	{
		private RichTextBox textbox;

		public void SetOutput(RichTextBox _textbox)
		{
			textbox = _textbox;
		}

		public void Log(Board board)
		{
			if (textbox == null)
				return;

			textbox.Document = new FlowDocument(new Paragraph(new Run(board.ToString())));
		}
	}
}