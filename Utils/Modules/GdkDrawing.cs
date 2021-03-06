using System;
using Gdk;

namespace Utils
{
	/// <summary>
	/// Gdk drawing.
	/// 
	/// Some measurements:
	/// 23		dr.Window.FrameExtends
	/// </summary>
	public class GdkDrawing
	{
		// cl 18.
		public static void Clear(DrawingArgs dr, Gdk.Color color) {
			var rect = dr.Rectangle;
			var filled = true;
			dr.GC.RgbFgColor = color;
			dr.Window.DrawRectangle(dr.GC, filled, rect);
		}

		// cl 18.
		public static void DrawLine(DrawingArgs dr, Gdk.Color color, int width, int x1, int y1, int x2, int y2) {
			dr.GC.RgbFgColor = color;
			dr.GC.SetLineAttributes(width, LineStyle.Solid, CapStyle.Butt, JoinStyle.Round);
			dr.Window.DrawLine(dr.GC, x1, y1, x2, y2);
		}

		public static void DrawRectangle(DrawingArgs dr, Gdk.Color color, Gdk.Rectangle rect)
		{
			dr.GC.RgbFgColor = color;
			dr.Window.DrawRectangle(dr.GC, true, rect);
		}

		private static void DrawMetricShortLine
		(DrawingArgs dr, Gdk.Color color, int lineWidth, int x, int y, int longHeight) {
			DrawLine(dr, color, lineWidth, x, y, x, y + longHeight/2);
		}

		private static void DrawMetricLongLine
		(DrawingArgs dr, Gdk.Color color, int lineWidth, int x, int y, int longHeight) {
			DrawLine(dr, color, lineWidth, x, y, x, y + longHeight);
		}

		public static void DrawMetric
		(DrawingArgs dr, Gdk.Color color, int lineWidth, int space, int longIndex, int longHeight) {
			var bounds = dr.Rectangle;
			var start = 0;
			var end = bounds.Width;
			int i = 0;
			int y = 0;
			for (int x = start; x < end; x += space) {
				bool isLong = (i % longIndex) == 0;
				if (isLong) DrawMetricLongLine(dr, color, lineWidth, x, y, longHeight);
				else DrawMetricShortLine(dr, color, lineWidth, x, y, longHeight);

				i++;
			}
		}

		private static void DrawPolygon(DrawingArgs dr, Gdk.Color color, Point[] points) {
			dr.GC.RgbFgColor = color;
			dr.Window.DrawPolygon(dr.GC, true, points);
		}

		private static void DrawPolygonBorder(DrawingArgs dr, Gdk.Color color, Point[] points) {
			dr.GC.RgbFgColor = color;
			dr.Window.DrawPolygon(dr.GC, false, points);
		}

		public static void DrawYellowRightTriangle(DrawingArgs dr, int x, int y, int w, int h)
		{
			var points = new Point[]{
				new Point(x, y), 
				new Point(x+w, y+h/2), 
				new Point(x, h)};
			DrawPolygon(dr, new Gdk.Color(255, 255, 0), points);
			DrawPolygonBorder(dr, new Gdk.Color(0, 0, 0), points);
		}

		public static void DrawYellowLeftTriangle(DrawingArgs dr, int x, int y, int w, int h) {
			var points = new Point[]{
				new Point(x, y),
				new Point(x-w, y+h/2), 
				new Point(x, h)};
			DrawPolygon(dr, new Gdk.Color(255, 255, 0), points);
			DrawPolygonBorder(dr, new Gdk.Color(0, 0, 0), points);
		}

	}
}

