using System;
using System.Collections.Generic;
using Utils.Drawing;

namespace Utils
{
	/// <summary>
	/// Data module.
	/// 
	/// Supported types:
	/// 
	/// 	TYPE			LIST
	/// 	Color			ColorList
	/// 	Line			LineList
	/// 	Point			PointList
	/// 	Point3			Point3List
	/// 	Point4			Point4List
	/// 	Quaternion		QuaternionList
	/// 	Rectangle		RectangleList
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

		public const int TYPE_POINT3 = 5;
		public const int FIELD_POINT3_X = 1;
		public const int FIELD_POINT3_Y = 2;
		public const int FIELD_POINT3_Z = 3;

		public const int TYPE_POINT4 = 6;
		public const int FIELD_POINT4_X = 1;
		public const int FIELD_POINT4_Y = 2;
		public const int FIELD_POINT4_Z = 3;
		public const int FIELD_POINT4_W = 4;

		public const int TYPE_QUATERNION = 7;
		public const int FIELD_QUATERNION_X = 1;
		public const int FIELD_QUATERNION_Y = 2;
		public const int FIELD_QUATERNION_Z = 3;
		public const int FIELD_QUATERNION_W = 4;

		public const int TYPE_RECTANGLE_LIST = 1001;
		public const int FIELD_RECTANGLE_LIST_ITEMS = 1;

		public const int TYPE_POINT_LIST = 1002;
		public const int FIELD_POINT_LIST_ITEMS = 1;

		public const int TYPE_LINE_LIST = 1003;
		public const int FIELD_LINE_LIST_ITEMS = 1;

		public const int TYPE_COLOR_LIST = 1004;
		public const int FIELD_COLOR_LIST_ITEMS = 1;

		public const int TYPE_POINT3_LIST = 1005;
		public const int FIELD_POINT3_LIST_ITEMS = 1;

		public const int TYPE_POINT4_LIST = 1006;
		public const int FIELD_POINT4_LIST_ITEMS = 1;

		public const int TYPE_QUATERNION_LIST = 1007;
		public const int FIELD_QUATERNION_LIST_ITEM = 1;

		public static void AddRectangle (GroupManager manager, Rectangle rect) {
			manager.Add (TYPE_RECTANGLE, rect.X, rect.Y, rect.Width, rect.Height);
		}

		public static Rectangle GetRectangle (GroupManager manager, int row) {
			if ((int)manager [row, 0] != TYPE_RECTANGLE) {
				throw new Exception ("Invalid type: Expected Rectangle");
			}

			var rect = new Rectangle () {
				X = (double)manager [row, FIELD_RECTANGLE_X],
				Y = (double)manager [row, FIELD_RECTANGLE_Y],
				Width = (double)manager [row, FIELD_RECTANGLE_WIDTH],
			    Height = (double)manager [row, FIELD_RECTANGLE_HEIGHT]
			};
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

			var p = new Point () {
				X = (double)manager [row, FIELD_POINT_X],
			    Y = (double)manager [row, FIELD_POINT_Y]
			};
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

			var line = new Line () {
				StartPoint = (Point)manager [row, FIELD_LINE_START_POINT],
			    EndPoint = (Point)manager [row, FIELD_LINE_END_POINT]
			};
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

			var color = new Color () {
				R = (double)manager [row, FIELD_COLOR_R],
			    G = (double)manager [row, FIELD_COLOR_G],
			    B = (double)manager [row, FIELD_COLOR_B],
			    A = (double)manager [row, FIELD_COLOR_A]
			};
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

			manager [row, FIELD_RECTANGLE_LIST_ITEMS] = rectangleList.Items;
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

			manager [row, FIELD_POINT_LIST_ITEMS] = pointList.Items;
		}

		public static void AddLineList (GroupManager manager, LineList lineList) {
			manager.Add (TYPE_LINE_LIST, lineList.Items);
		}

		public static LineList GetLineList (GroupManager manager, int row) {
			if ((int)manager [row, 0] != TYPE_LINE_LIST) {
				throw new Exception ("Invalid type: Expected LineList");
			}

			var lineList = new LineList () {
				Items = (List<Line>)manager [row, FIELD_LINE_LIST_ITEMS]
			};
			return lineList;
		}

		public static void UpdateLineList (GroupManager manager, int row, LineList list) {
			if ((int)manager [row, 0] != TYPE_LINE_LIST) {
				throw new Exception ("Invalid type: Expected LineList");
			}

			manager [row, FIELD_LINE_LIST_ITEMS] = list.Items;
		}

		public static void AddColorList (GroupManager manager, ColorList colorList) {
			manager.Add (TYPE_COLOR_LIST, colorList.Items);
		}

		public static ColorList GetColorList (GroupManager manager, int row) {
			if ((int)manager [row, 0] != TYPE_COLOR_LIST) {
				throw new Exception ("Invalid type: Expected ColorList");
			}

			var colorList = new ColorList () {
				Items = (List<Color>)manager [row, FIELD_COLOR_LIST_ITEMS]
			};
			return colorList;
		}

		public static void UpdateColorList (GroupManager manager, int row, ColorList colorList) {
			if ((int)manager [row, 0] != TYPE_COLOR_LIST) {
				throw new Exception ("Invalid type: Expected ColorList");
			}

			manager [row, FIELD_COLOR_LIST_ITEMS] = colorList.Items;
		}

		public static void AddPoint3 (GroupManager manager, Point3 point3) {
			manager.Add (TYPE_POINT3, point3.X, point3.Y, point3.Z);
		}

		public static Point3 GetPoint3 (GroupManager manager, int row) {
			if ((int)manager [row, 0] != TYPE_POINT3) {
				throw new Exception ("Invalid type: Expected Point3");
			}

			var point3 = new Point3 () {
				X = (double)manager [row, FIELD_POINT3_X],
				Y = (double)manager [row, FIELD_POINT3_Y],
				Z = (double)manager [row, FIELD_POINT3_Z]
			};
			return point3;
		}

		public static void UpdatePoint3 (GroupManager manager, int row, Point3 point3) {
			if ((int)manager [row, 0] != TYPE_POINT3) {
				throw new Exception ("Invalid type: Expected Point3");
			}

			manager [row, FIELD_POINT3_X] = point3.X;
			manager [row, FIELD_POINT3_Y] = point3.Y;
			manager [row, FIELD_POINT3_Z] = point3.Z;
		}

		public static void AddPoint3List (GroupManager manager, Point3List point3List) {
			manager.Add (TYPE_POINT3_LIST, point3List.Items);
		}

		public static Point3List GetPoint3List (GroupManager manager, int row) {
			if ((int)manager [row, 0] != TYPE_POINT3_LIST) {
				throw new Exception ("Invalid type: Expected Point3List");
			}

			var point3List = new Point3List () {
				Items = (List<Point3>)manager [row, FIELD_POINT3_LIST_ITEMS]
			};
			return point3List;
		}

		public static void UpdatePoint3List (GroupManager manager, int row, Point3List point3List) {
			if ((int)manager [row, 0] != TYPE_POINT3_LIST) {
				throw new Exception ("Invalid type: Expected Point3List");
			}

			manager [row, FIELD_POINT3_LIST_ITEMS] = point3List.Items;
		}

		public static void AddPoint4 (GroupManager manager, Point4 point4) {
			manager.Add (TYPE_POINT4, point4.X, point4.Y, point4.Z, point4.W);
		}

		public static Point4 GetPoint4 (GroupManager manager, int row) {
			if ((int)manager [row, 0] != TYPE_POINT4) {
				throw new Exception ("Invalid type: Expected Point4");
			}

			var point4 = new Point4 () {
				X = (double)manager [row, FIELD_POINT4_X],
				Y = (double)manager [row, FIELD_POINT4_Y],
				Z = (double)manager [row, FIELD_POINT4_Z],
				W = (double)manager [row, FIELD_POINT4_W]
			};
			return point4;
		}

		public static void UpdatePoint4 (GroupManager manager, int row, Point4 point4) {
			if ((int)manager [row, 0] != TYPE_POINT4) {
				throw new Exception ("Invalid type: Expected Point4");
			}

			manager [row, FIELD_POINT4_X] = point4.X;
			manager [row, FIELD_POINT4_Y] = point4.Y;
			manager [row, FIELD_POINT4_Z] = point4.Z;
			manager [row, FIELD_POINT4_W] = point4.W;
		}

		public static void AddPoint4List (GroupManager manager, Point4List point4List) {
			manager.Add (TYPE_POINT4_LIST, point4List.Items);
		}

		public static Point4List GetPoint4List (GroupManager manager, int row) {
			if ((int)manager [row, 0] != TYPE_POINT4_LIST) {
				throw new Exception ("Invalid type: Expected Point4List");
			}

			var point4List = new Point4List () {
				Items = (List<Point4>)manager [row, FIELD_POINT4_LIST_ITEMS]
			};
			return point4List;
		}

		public static void UpdatePoint4List (GroupManager manager, int row, Point4List point4List) {
			if ((int)manager [row, 0] != TYPE_POINT4_LIST) {
				throw new Exception ("Invalid type: Expceted Point4List");
			}

			manager [row, FIELD_POINT4_LIST_ITEMS] = point4List.Items;
		}

		public static void AddQuaternion (GroupManager manager, Quaternion quaternion) {
			manager.Add (TYPE_QUATERNION, quaternion.X, quaternion.Y, quaternion.Z, quaternion.W);
		}

		public static Quaternion GetQuaternion (GroupManager manager, int row) {
			if ((int)manager [row, 0] != TYPE_QUATERNION) {
				throw new Exception ("Invalid type: Expected Quaternion");
			}

			var quaternion = new Quaternion () {
				X = (double)manager [row, FIELD_QUATERNION_X],
				Y = (double)manager [row, FIELD_QUATERNION_Y],
				Z = (double)manager [row, FIELD_QUATERNION_Z],
				W = (double)manager [row, FIELD_QUATERNION_W]
			};
			return quaternion;
		}

		public static void UpdateQuaternion (GroupManager manager, int row, Quaternion quaternion) {
			if ((int)manager [row, 0] != TYPE_QUATERNION) {
				throw new Exception ("Invalid type: Expted Quaternion");
			}

			manager [row, FIELD_QUATERNION_X] = quaternion.X;
			manager [row, FIELD_QUATERNION_Y] = quaternion.Y;
			manager [row, FIELD_QUATERNION_Z] = quaternion.X;
			manager [row, FIELD_QUATERNION_W] = quaternion.W;
		}

		public static void AddQuaternionList (GroupManager manager, QuaternionList quaternionList) {
			manager.Add (TYPE_QUATERNION_LIST, quaternionList.Items);
		}

		public static QuaternionList GetQuaternionList (GroupManager manager, int row) {
			if ((int)manager [row, 0] != TYPE_QUATERNION_LIST) {
				throw new Exception ("Invalid type: Expected QuaternionList");
			}

			var quaternionList = new QuaternionList () {
				Items = (List<Quaternion>)manager [row, FIELD_QUATERNION_LIST_ITEM]
			};
			return quaternionList;
		}

		public static void UpdateQuaternionList (GroupManager manager, int row, QuaternionList quaternionList) {
			if ((int)manager [row, 0] != TYPE_QUATERNION_LIST) {
				throw new Exception ("Invalid type: Expected QuaternionList");
			}

			manager [row, FIELD_QUATERNION_LIST_ITEM] = quaternionList.Items;
		}
	}
}

