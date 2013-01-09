using System;

namespace Utils
{
	public class PolygonHitTest
	{
		public static Gdk.Rectangle Bounds(Gdk.Point[] points)
		{
			int n = points.Length;
			var minx = 0;
			var miny = 0;
			var maxx = 0;
			var maxy = 0;
			var setbounds = false;
			for (int i = 0; i < n; i++) {
				var p = points[i];
				minx = !setbounds || p.X < minx ? p.X : minx;
				miny = !setbounds || p.Y < miny ? p.Y : miny;
				maxx = !setbounds || p.X > maxx ? p.X : maxx;
				maxy = !setbounds || p.Y > maxy ? p.Y : maxy;
				setbounds = true;
			}

			return new Gdk.Rectangle(minx, miny, maxx - minx, maxy - miny);
		}

		public static bool HitsPolygonRectangle(Gdk.Point[] points, int x, int y) {
			var rect = Bounds(points);
			if (x >= rect.X && y >= rect.Y && x <= rect.X + rect.Width && y <= rect.Y + rect.Height)
			{
				return true;
			}

			return false;
		}
	}
}

