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
			var rect = new Rectangle (0, 1, 2, 3);
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
		public void TestPoint ()
		{
			var manager = new GroupManager ();
			var point = new Point (0, 1);
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
			var line = new Line (0, 1, 2, 3);
			DataModule.AddLine (manager, line);
			manager.Commit ();

			line.StartPoint = new Point (10, 11);

			line = DataModule.GetLine (manager, 0);
			Assert.True (line.StartPoint.X == 0);
			Assert.True (line.StartPoint.Y == 1);
			Assert.True (line.EndPoint.X == 2);
			Assert.True (line.EndPoint.Y == 3);

			line.StartPoint = new Point (10, 11);
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
			var color = new Color (0, 0.1, 0.2, 0.3);
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
	}
}

