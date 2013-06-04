using System;

namespace Utils.Drawing
{
	public static class DataModule
	{
		public const int TYPE_RECTANGLE = 1;
		public const int FIELD_RECTANGLE_X = 1;
		public const int FIELD_RECTANGLE_Y = 2;
		public const int FIELD_RECTANGLE_WIDTH = 3;
		public const int FIELD_RECTANGLE_HEIGHT = 4;

		public const int TYPE_POINT = 2;
		public const int FIELD_POINT_X = 1;
		public const int FIELD_POINT_Y = 2;

		public const int TYPE_LINE = 3;
		public const int FIELD_LINE_START_POINT = 1;
		public const int FIELD_LINE_END_POINT = 2;

		public static void AddRectangle (GroupManager manager, Rectangle rect) {
			manager.Add (TYPE_RECTANGLE, rect.X, rect.Y, rect.Width, rect.Height);
		}

		public static Rectangle GetRectangle (GroupManager manager, int row) {
			if ((int)manager [row, 0] != TYPE_RECTANGLE) {
				throw new Exception ("Invalid type: Expected Rectangle");
			}

			var rect = new Rectangle (
				(double)manager [row, FIELD_RECTANGLE_X],
				(double)manager [row, FIELD_RECTANGLE_Y],
				(double)manager [row, FIELD_RECTANGLE_WIDTH],
			    (double)manager [row, FIELD_RECTANGLE_HEIGHT]
				);
			return rect;
		}

		public static void UpdateRectangle (GroupManager manager, int row, Rectangle rect) {
			if ((int)manager [row, 0] != TYPE_RECTANGLE) {
				throw new Exception ("Invalid type: Expected Rectangle");
			}

			manager [row, FIELD_RECTANGLE_X] = rect.X;
			manager [row, FIELD_RECTANGLE_Y] = rect.Y;
			manager [row, FIELD_RECTANGLE_WIDTH] = rect.Width;
			manager [row, FIELD_RECTANGLE_HEIGHT] = rect.Height;
		}

		public static void AddPoint (GroupManager manager, Point point) {
			manager.Add (TYPE_POINT, point.X, point.Y);
		}

		public static Point GetPoint (GroupManager manager, int row) {
			if ((int)manager [row, 0] != TYPE_POINT) {
				throw new Exception ("Invalid type: Expected Point");
			}

			var p = new Point ((double)manager [row, FIELD_POINT_X],
			                   (double)manager [row, FIELD_POINT_Y]);
			return p;
		}

		public static void UpdatePoint (GroupManager manager, int row, Point point) {
			if ((int)manager [row, 0] != TYPE_POINT) {
				throw new Exception ("Invalid type: Expected Point");
			}

			manager [row, FIELD_POINT_X] = point.X;
			manager [row, FIELD_POINT_Y] = point.Y;
		}

		public static void AddLine (GroupManager manager, Line line) {
			manager.Add (TYPE_LINE, line.StartPoint, line.EndPoint);
		}

		public static Line GetLine (GroupManager manager, int row) {
			if ((int)manager [row, 0] != TYPE_LINE) {
				throw new Exception ("Invalid type: Expected Line");
			}

			var line = new Line ((Point)manager [row, FIELD_LINE_START_POINT],
			                     (Point)manager [row, FIELD_LINE_END_POINT]);
			return line;
		}

		public static void UpdateLine (GroupManager manager, int row, Line line) {
			if ((int)manager [row, 0] != TYPE_LINE) {
				throw new Exception ("Invalid type: Expected Line");
			}

			manager [row, FIELD_LINE_START_POINT] = line.StartPoint;
			manager [row, FIELD_LINE_END_POINT] = line.EndPoint;
		}
	}
}

