using System;
using System.Drawing;

namespace WinmineSolver
{
	public static class ExtraMath
	{
		public static int Distance(int x1, int x2, int y1, int y2)
		{
			return (int)(Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2)));
		}

		public static int Distance(int x, int y)
		{
			return Distance(0, x, 0, y);
		}

		public static void MultiplyMatrix22ByScalar(double[,] m, double s)
		{
			for (var i = 0; i < 2; i++)
				for (var j = 0; j < 2; j++)
					m[i, j] = s * m[i, j];
		}

		public static Point MultiplyMatrix22ToPoint(double[,] m, int x, int y)
		{
			return new Point
			{
				X = (int)(m[0, 0] * x + m[0, 1] * y),
				Y = (int)(m[1, 0] * x + m[1, 1] * y),
			};
		}

		public static double[,] GetMatrix(int targetDx, int targetDy, int recDx, int recDy)
		{
			var targetLen = Distance(targetDx, targetDy);
			var recLen = Distance(recDx, recDy);

			var cosarec = (double)recDx / recLen;
			var sinarec = (double)recDy / recLen;
			var costarget = (double)targetDx / targetLen;
			var sintarget = (double)targetDy / targetLen;

			var cosa = cosarec * costarget + sinarec * sintarget;
			var sina = sinarec * costarget - cosarec * sintarget;

			var matrix = new[,] { { cosa, sina }, { -sina, cosa }, };
			MultiplyMatrix22ByScalar(matrix, (double)targetLen / recLen);
			return matrix;
		}
	}
}
