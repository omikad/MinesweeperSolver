using System;
using System.Collections.Generic;
using System.Linq;

namespace WinmineSolver.Mouse
{
	public interface IMouseTrailCollection
	{
		MouseTrailInfo GetRandomTrail();
	}

	public class MouseTrailCollection : IMouseTrailCollection
	{
		private readonly List<MouseTrailInfo> trails;
		private readonly Random rnd;

		public MouseTrailCollection(IEnumerable<MouseMovementInfo> infos)
		{
			rnd = new Random();
			trails = new List<MouseTrailInfo>();

			var list = new List<MouseMovementInfo>();

			foreach (var info in infos)
			{
				list.Add(info);

				if (info.Type == MouseHook.MouseMessages.WM_LBUTTONUP)
				{
					var first = list[0];
					trails.Add(new MouseTrailInfo
					{
						Len = CalcDistance(first, info),
						DX = info.X - first.X,
						DY = info.Y - first.Y,
						Moves = list.ToArray(),
					});

					list = new List<MouseMovementInfo>();
				}
			}
		}

		public MouseTrailInfo GetRandomTrail()
		{
			if (trails.Count == 0)
				throw new InvalidOperationException();

			return trails[rnd.Next(trails.Count)];
		}

		public override string ToString()
		{
			var clickPrecision = 0;
			foreach (var trail in trails)
			{
				var down = trail.Moves.First(m => m.Type == MouseHook.MouseMessages.WM_LBUTTONDOWN);
				var up = trail.Moves.First(m => m.Type == MouseHook.MouseMessages.WM_LBUTTONUP);
				var len = CalcDistance(down, up);
				if (len > clickPrecision)
					clickPrecision = len;
			}

			return string.Join(", ", trails.Select(m => m.Len)) + " Count = " + trails.Count + " ClickPrecision = " + clickPrecision;
		}

		private static int CalcDistance(MouseMovementInfo a, MouseMovementInfo b)
		{
			return ExtraMath.Distance(a.X, b.X, a.Y, b.Y);
		}
	}
}
