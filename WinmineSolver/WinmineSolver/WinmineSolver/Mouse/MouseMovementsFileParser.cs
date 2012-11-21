using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;

namespace WinmineSolver.Mouse
{
	public class MouseMovementsFileParser
	{
		private readonly Func<string, MouseMovementInfo> mouseMoveInfoParser;
		private IMouseTrailCollection defaultTrails;

		[ImportingConstructor]
		public MouseMovementsFileParser(Func<string, MouseMovementInfo> mouseMoveInfoParser)
		{
			this.mouseMoveInfoParser = mouseMoveInfoParser;
		}

		[Export(typeof(Func<string, IMouseTrailCollection>))]
		public IMouseTrailCollection ParseFile(string fileName)
		{
			using (var fs = new StreamReader(fileName))
			{
				var infos = new List<MouseMovementInfo>();

				while (!fs.EndOfStream)
				{
					var s = fs.ReadLine();

					if (!string.IsNullOrWhiteSpace(s))
					{
						infos.Add(mouseMoveInfoParser(s));
					}
				}

				var mouseAnalysis = new MouseTrailCollection(infos);

				return mouseAnalysis;
			}
		}

		[Export(typeof(IMouseTrailCollection))]
		public IMouseTrailCollection DefaultTrails
		{
			get
			{
				return defaultTrails ?? (defaultTrails = ParseFile("mouse.txt"));
			}
		}
	}
}