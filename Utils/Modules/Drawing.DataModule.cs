using System;
using System.Collections.Generic;

namespace Utils.Drawing
{
	/// <summary>
	/// Data module.
	/// 
	/// Supported types:
	/// 
	/// 	TYPE			LIST
	/// 	Rectangle		RectangleList
	/// 	Point			PointList
	/// 	Line
	/// 	Color
	/// </summary>
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

		public const int TYPE_COLOR = 4;
		public const int FIELD_COLOR_R = 1;
		public const int FIELD_COLOR_G = 2;
		public const int FIELD_COLOR_B = 3;
		public const int FIELD_COLOR_A = 4;

		public const int TYPE_RECTANGLE_LIST = 101;
		public const int FIELD_RECTANGLE_LIST_ITEMS = 1;

		public const int TYPE_POINT_LIST = 202;
		public const int FIELD_POINT_LIST_ITEMS = 1;

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

		public static void AddColor (GroupManager manager, Color color) {
			manager.Add (TYPE_COLOR, color.R, color.G, color.B, color.A);
		}

		public static Color GetColor (GroupManager manager, int row) {
			if ((int)manager [row, 0] != TYPE_COLOR) {
				throw new Exception ("Invalid type: Expected Color");
			}

			var color = new Color ((double)manager [row, FIELD_COLOR_R],
			                       (double)manager [row, FIELD_COLOR_G],
			                       (double)manager [row, FIELD_COLOR_B],
			                       (double)manager [row, FIELD_COLOR_A]);
			return color;
		}

		public static void UpdateColor (GroupManager manager, int row, Color color) {
			if ((int)manager [row, 0] != TYPE_COLOR) {
				throw new Exception ("Invalid type: Expected Color");
			}

			manager [row, FIELD_COLOR_R] = color.R;
			manager [row, FIELD_COLOR_G] = color.G;
			manager [row, FIELD_COLOR_B] = color.B;
			manager [row, FIELD_COLOR_A] = color.A;
		}

		public static void AddRectangeList (GroupManager manager, RectangleList rectangleList) {
			manager.Add (TYPE_RECTANGLE_LIST, rectangleList.Items);
		}

		public static RectangleList GetRectangleList (GroupManager manager, int row) {
			if ((int)manager [row, 0] != TYPE_RECTANGLE_LIST) {
				throw new Exception ("Invalid type: Expected RectangleList");
			}

			var rectangleList = new RectangleList () {
				Items = (List<Rectangle>)manager [row, FIELD_RECTANGLE_LIST_ITEMS]
			};
			return rectangleList;
		}

		public static void UpdateRectangleList (GroupManager manager, int row, RectangleList rectangleList) {
			if ((int)manager [row, 0] != TYPE_RECTANGLE_LIST) {
				throw new Exception ("Invalid type: Expected RectangleList");
			}

			manager [row, 0] = rectangleList.Items;
		}

		public static void AddPointList (GroupManager manager, PointList pointList) {
			manager.Add (TYPE_POINT_LIST, pointList.Items);
		}

		public static PointList GetPointList (GroupManager manager, int row) {
			if ((int)manager [row, 0] != TYPE_POINT_LIST) {
				throw new Exception ("Invalid type: Expected PointList");
			}

			var pointList = new PointList () {
				Items = (List<Point>)manager [row, FIELD_POINT_LIST_ITEMS]
			};
			return pointList;
		}

		public static void UpdatePointList (GroupManager manager, int row, PointList pointList) {
			if ((int)manager [row, 0] != TYPE_POINT_LIST) {
				throw new Exception ("Invalid type: Expected PointList");
			}

			manager [row, 0] = pointList.Items;
		}
	}
}

