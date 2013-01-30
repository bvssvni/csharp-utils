using System;
using System.Collections.Generic;

namespace Utils
{
	public class VectorGroup
	{
		public static void Add(Group g, IList<double> x, IList<double> vx, 
		                         	IList<double> res)
		{
			int n = g.Count >> 1;
			for (int i = 0; i < n; i++) {
				int start = g[i << 1];
				int end = g[(i << 1) + 1];
				for (int j = start; j < end; j++) {
					res[j] = x[j] + vx[j];
				}
			}
		}

		public static void Subtract(Group g, IList<double> x, 
		                         IList<double> x2, 
		                         IList<double> res)
		{
			int n = g.Count >> 1;
			for (int i = 0; i < n; i++) {
				int start = g[i << 1];
				int end = g[(i << 1) + 1];
				for (int j = start; j < end; j++) {
					res[j] = x[j] - x2[j];
				}
			}
		}

		public static void Multiply(Group g, IList<double> x, 
		                         IList<double> fx, 
		                         IList<double> res)
		{
			int n = g.Count >> 1;
			for (int i = 0; i < n; i++) {
				int start = g[i << 1];
				int end = g[(i << 1) + 1];
				for (int j = start; j < end; j++) {
					res[j] = x[j] * fx[j];
				}
			}
		}

		public static void LinearInterpolationX(Group g, IList<double> x, IList<double> x2, 
		                              IList<double> t, IList<double> res)
		{
			int n = g.Count >> 1;
			for (int i = 0; i < n; i++) {
				int start = g[i << 1];
				int end = g[(i << 1) + 1];
				for (int j = start; j < end; j++) {
					res[j] = x[j] + (x2[j] - x[j]) * t[j];
				}
			}
		}

		public static void DotXY(Group g, IList<double> x, IList<double> y, 
		                              IList<double> x2, IList<double> y2,
		                              IList<double> res)
		{
			int n = g.Count >> 1;
			for (int i = 0; i < n; i++) {
				int start = g[i << 1];
				int end = g[(i << 1) + 1];
				for (int j = start; j < end; j++) {
					res[j] = x[j] * x2[j] + y[j] * y2[j];
				}
			}
		}

		public static void CrossXY(Group g, IList<double> x, IList<double> y, 
		                         IList<double> x2, IList<double> y2,
		                         IList<double> res)
		{
			int n = g.Count >> 1;
			for (int i = 0; i < n; i++) {
				int start = g[i << 1];
				int end = g[(i << 1) + 1];
				for (int j = start; j < end; j++) {
					res[j] = x[j] * y2[j] - y[j] * x2[j];
				}
			}
		}

		public static void DirectionXY(Group g, IList<double> x, IList<double> y, 
		                              IList<double> tx, IList<double> ty,
		                              IList<double> resx, IList<double> resy)
		{
			int n = g.Count >> 1;
			for (int i = 0; i < n; i++) {
				int start = g[i << 1];
				int end = g[(i << 1) + 1];
				for (int j = start; j < end; j++) {
					double dx = tx[j] - x[j];
					double dy = ty[j] - y[j];
					double len = Math.Sqrt (dx * dx + dy * dy);
					resx[j] = dx / len;
					resy[j] = dy / len;
				}
			}
		}

		public static void LengthXY(Group g, IList<double> x, IList<double> y, IList<double> res)
		{
			int n = g.Count >> 1;
			for (int i = 0; i < n; i++) {
				int start = g[i << 1];
				int end = g[(i << 1) + 1];
				for (int j = start; j < end; j++) {
					double dx = x[j] - x[j];
					double dy = y[j] - y[j];
					res[j] = Math.Sqrt (dx * dx + dy * dy);
				}
			}
		}

	}
}

