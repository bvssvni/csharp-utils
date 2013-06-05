using System;
using System.Collections.Generic;
using NUnit.Framework;
using Utils.Drawing;

namespace Utils
{
	[TestFixture()]
	public class TestDrawingDataModule
	{
		[Test()]
		public void TestRectangle()
		{
			var manager = new GroupManager ();
			var rect = new Rectangle () {
				X = 0, 
				Y = 1, 
				Width = 2, 
				Height = 3
			};
			DataModule.AddRectangle (manager, rect);
			manager.Commit ();

			rect.X = 10;

			rect = DataModule.GetRectangle (manager, 0);
			Assert.True (rect.X == 0);
			Assert.True (rect.Y == 1);
			Assert.True (rect.Width == 2);
			Assert.True (rect.Height == 3);

			rect.X = 10;
			DataModule.UpdateRectangle (manager, 0, rect);
			manager.Commit ();

			rect = DataModule.GetRectangle (manager, 0);
			Assert.True (rect.X == 10);
		}

		[Test()]
		public void TestRectangleList ()
		{
			var manager = new GroupManager ();
			var rectangleList = new RectangleList () {
				Items = new List<Rectangle> ()
			};
			DataModule.AddRectangeList (manager, rectangleList);
			manager.Commit ();
			rectangleList = DataModule.GetRectangleList (manager, 0);
			DataModule.UpdateRectangleList (manager, 0, rectangleList);
			manager.Commit ();
			rectangleList = DataModule.GetRectangleList (manager, 0);
		}

		[Test()]
		public void TestRectangleTree ()
		{
			var manager = new GroupManager ();
			var rectangleTree = new RectangleTree ();
			DataModule.AddRectangleTree (manager, rectangleTree);
			manager.Commit ();

			rectangleTree = DataModule.GetRectangleTree (manager, 0);
			DataModule.UpdateRectangleTree (manager, 0, rectangleTree);
			manager.Commit ();

			rectangleTree = DataModule.GetRectangleTree (manager, 0);
		}

		[Test()]
		public void TestPoint ()
		{
			var manager = new GroupManager ();
			var point = new Point () {
				X = 0, 
				Y = 1
			};
			DataModule.AddPoint (manager, point);
			manager.Commit ();

			point.X = 10;

			point = DataModule.GetPoint (manager, 0);
			Assert.True (point.X == 0);
			Assert.True (point.Y == 1);

			point.X = 10;
			DataModule.UpdatePoint (manager, 0, point);
			manager.Commit ();

			point = DataModule.GetPoint (manager, 0);
			Assert.True (point.X == 10);
		}

		[Test()]
		public void TestPointList ()
		{
			var manager = new GroupManager ();
			var pointList = new PointList ();
			DataModule.AddPointList (manager, pointList);
			manager.Commit ();
			pointList = DataModule.GetPointList (manager, 0);
			DataModule.UpdatePointList (manager, 0, pointList);
			manager.Commit ();
			pointList = DataModule.GetPointList (manager, 0);
		}

		[Test()]
		public void TestLine ()
		{
			var manager = new GroupManager ();
			var line = new Line () {
				StartPoint = new Point () {
					X = 0, 
					Y = 1
				}, 
				EndPoint = new Point () {
					X = 2, 
					Y = 3
				}
			};
			DataModule.AddLine (manager, line);
			manager.Commit ();

			line.StartPoint = new Point () {
				X = 10, 
				Y = 11
			};

			line = DataModule.GetLine (manager, 0);
			Assert.True (line.StartPoint.X == 0);
			Assert.True (line.StartPoint.Y == 1);
			Assert.True (line.EndPoint.X == 2);
			Assert.True (line.EndPoint.Y == 3);

			line.StartPoint = new Point () {
				X = 10, 
				Y = 11
			};
			DataModule.UpdateLine (manager, 0, line);
			manager.Commit ();

			line = DataModule.GetLine (manager, 0);
			Assert.True (line.StartPoint.X == 10);
		}

		[Test()]
		public void TestLineList ()
		{
			var manager = new GroupManager ();
			var lineList = new LineList ();
			DataModule.AddLineList (manager, lineList);
			manager.Commit ();
			lineList = DataModule.GetLineList (manager, 0);
			DataModule.UpdateLineList (manager, 0, lineList);
			manager.Commit ();
			lineList = DataModule.GetLineList (manager, 0);
		}

		public void TestColor ()
		{
			var manager = new GroupManager ();
			var color = new Color () {
				R = 0.0, 
				G = 0.1,
				B = 0.2, 
				A = 0.3
			};
			DataModule.AddColor (manager, color);
			manager.Commit ();

			color.R = 1.0;

			color = DataModule.GetColor (manager, 0);
			Assert.True (color.R == 0);
			Assert.True (color.G == 0.1);
			Assert.True (color.B == 0.2);
			Assert.True (color.A == 0.3);

			color.R = 1.0;
			DataModule.UpdateColor (manager, 0, color);

			color = DataModule.GetColor (manager, 0);
			Assert.True (color.R == 1);
		}

		[Test()]
		public void TestColorList ()
		{
			var manager = new GroupManager ();
			var colorList = new ColorList ();
			DataModule.AddColorList (manager, colorList);
			manager.Commit ();
			colorList = DataModule.GetColorList (manager, 0);
			DataModule.UpdateColorList (manager, 0, colorList);
			manager.Commit ();
			colorList = DataModule.GetColorList (manager, 0);
		}

		[Test()]
		public void TestPoint3 ()
		{
			var manager = new GroupManager ();
			var point3 = new Point3 () {
				X = 0,
				Y = 1,
				Z = 2
			};
			DataModule.AddPoint3 (manager, point3);
			manager.Commit ();

			point3.X = 1;

			point3 = DataModule.GetPoint3 (manager, 0);
			Assert.True (point3.X == 0);
			Assert.True (point3.Y == 1);
			Assert.True (point3.Z == 2);

			point3.X = 10;
			DataModule.UpdatePoint3 (manager, 0, point3);
			manager.Commit ();
			point3 = DataModule.GetPoint3 (manager, 0);
			Assert.True (point3.X == 10);
		}

		[Test()]
		public void TestPoint3List ()
		{
			var manager = new GroupManager ();
			var point3List = new Point3List ();
			DataModule.AddPoint3List (manager, point3List);
			manager.Commit ();
			point3List = DataModule.GetPoint3List (manager, 0);
			DataModule.UpdatePoint3List (manager, 0, point3List);
			point3List = DataModule.GetPoint3List (manager, 0);
		}

		[Test()]
		public void TestPoint4 ()
		{
			var manager = new GroupManager ();
			var point4 = new Point4 () {
				X = 0,
				Y = 1,
				Z = 2,
				W = 3
			};
			DataModule.AddPoint4 (manager, point4);
			manager.Commit ();

			point4 = DataModule.GetPoint4 (manager, 0);
			Assert.True (point4.X == 0);
			Assert.True (point4.Y == 1);
			Assert.True (point4.Z == 2);
			Assert.True (point4.W == 3);
			point4.X = 10;
			DataModule.UpdatePoint4 (manager, 0, point4);
			manager.Commit ();

			point4 = DataModule.GetPoint4 (manager, 0);
			Assert.True (point4.X == 10);
		}

		[Test()]
		public void TestPoint4List ()
		{
			var manager = new GroupManager ();
			var point4List = new Point4List ();
			DataModule.AddPoint4List (manager, point4List);
			manager.Commit ();

			point4List = DataModule.GetPoint4List (manager, 0);
			DataModule.UpdatePoint4List (manager, 0, point4List);
			manager.Commit ();

			point4List = DataModule.GetPoint4List (manager, 0);
		}

		[Test()]
		public void TestQuaternion ()
		{
			var manager = new GroupManager ();
			var quaternion = new Quaternion () {
				X = 0,
				Y = 1,
				Z = 2,
				W = 3
			};
			DataModule.AddQuaternion (manager, quaternion);
			manager.Commit ();

			quaternion = DataModule.GetQuaternion (manager, 0);
			Assert.True (quaternion.X == 0);
			Assert.True (quaternion.Y == 1);
			Assert.True (quaternion.Z == 2);
			Assert.True (quaternion.W == 3);

			quaternion.X = 10;
			DataModule.UpdateQuaternion (manager, 0, quaternion);
			manager.Commit ();

			quaternion = DataModule.GetQuaternion (manager, 0);
			Assert.True (quaternion.X == 10);
		}

		[Test()]
		public void TestQuaternionList ()
		{
			var manager = new GroupManager ();
			var quaternionList = new QuaternionList ();
			DataModule.AddQuaternionList (manager, quaternionList);
			manager.Commit ();

			quaternionList = DataModule.GetQuaternionList (manager, 0);
			DataModule.UpdateQuaternionList (manager, 0, quaternionList);
			manager.Commit ();

			quaternionList = DataModule.GetQuaternionList (manager, 0);
		}

		[Test()]
		public void TestWheel ()
		{
			var manager = new GroupManager ();
			var wheel = new Wheel () {
				Position = new Point () {
					X = 0,
					Y = 1
				},
				Angle = 2
			};
			DataModule.AddWheel (manager, wheel);
			manager.Commit ();

			wheel = DataModule.GetWheel (manager, 0);
			Assert.True (wheel.Position.X == 0);
			Assert.True (wheel.Position.Y == 1);
			Assert.True (wheel.Angle == 2);

			wheel.Position.X = 10;
			DataModule.UpdateWheel (manager, 0, wheel);
			manager.Commit ();

			wheel = DataModule.GetWheel (manager, 0);
			Assert.True (wheel.Position.X == 10);
		}

		[Test()]
		public void TestWheelList ()
		{
			var manager = new GroupManager ();
			var wheelList = new WheelList ();
			DataModule.AddWheelList (manager, wheelList);
			manager.Commit ();

			wheelList = DataModule.GetWheelList (manager, 0);
			DataModule.UpdateWheelList (manager, 0, wheelList);
			manager.Commit ();

			wheelList = DataModule.GetWheelList (manager, 0);
		}
	}
}

