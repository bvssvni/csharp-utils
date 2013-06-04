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

		public static void AddTo (this Rectangle rect, GroupManager manager) {
			manager.Add (TYPE_RECTANGLE, rect.X, rect.Y, rect.Width, rect.Height);
		}

		public static void GetFrom (this Rectangle rect, GroupManager manager, int row) {
			if ((int)manager [row, 0] != TYPE_RECTANGLE) {
				throw new Exception ("Invalid type: Expected Rectangle");
			}

			rect.X = (double)manager [row, FIELD_RECTANGLE_X];
			rect.Y = (double)manager [row, FIELD_RECTANGLE_Y];
			rect.Width = (double)manager [row, FIELD_RECTANGLE_WIDTH];
			rect.Height = (double)manager [row, FIELD_RECTANGLE_HEIGHT];
		}

		public static void Update (this Rectangle rect, GroupManager manager, int row) {
			if ((int)manager [row, 0] != TYPE_RECTANGLE) {
				throw new Exception ("Invalid type: Expected Rectangle");
			}

			manager [row, FIELD_RECTANGLE_X] = rect.X;
			manager [row, FIELD_RECTANGLE_Y] = rect.Y;
			manager [row, FIELD_RECTANGLE_WIDTH] = rect.Width;
			manager [row, FIELD_RECTANGLE_HEIGHT] = rect.Height;
		}

		public static void AddTo (this Point point, GroupManager manager) {
			manager.Add (TYPE_POINT, point.X, point.Y);
		}

		public static void GetFrom (this Point point, GroupManager manager, int row) {
			if ((int)manager [row, 0] != TYPE_POINT) {
				throw new Exception ("Invalid type: Expected Point");
			}

			point.X = (double)manager [row, FIELD_POINT_X];
			point.Y = (double)manager [row, FIELD_POINT_Y];
		}

		public static void Update (this Point point, GroupManager manager, int row) {
			if ((int)manager [row, 0] != TYPE_POINT) {
				throw new Exception ("Invalid type: Expected Point");
			}

			manager [row, FIELD_POINT_X] = point.X;
			manager [row, FIELD_POINT_Y] = point.Y;
		}
	}
}

