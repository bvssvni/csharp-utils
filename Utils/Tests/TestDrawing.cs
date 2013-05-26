using System;
using System.Collections.Generic;
using NUnit.Framework;
using Utils.Drawing;

namespace Utils
{
	[TestFixture()]
	public class TestDrawing
	{
		[Test()]
		public void TestBranchIteration()
		{
			var a = new EllipseShape ();
			var b = new RectangleShape ();
			var c = new LineShape ();
			var op = new ShapeTree (a);
			op.AddChild (b).AddChild (c);

			// Iterate through each member of the branch.
			var list = new List<ShapeBase> ();
			foreach (var item in op) {
				list.Add (item);
			}

			Assert.True (list[0] == b);
			Assert.True (list[1] == c);
		}

		[Test()]
		public void TestDrawRectangle ()
		{
			var bytes = new byte[1024 * 4];
			var format = Cairo.Format.ARGB32;
			var image = new Cairo.ImageSurface (bytes, format, 32, 32, 32 * 4); 
			var shape = new RectangleShape ();
			shape.Look.Fill = new SolidBrush (1.0, 0.0, 0.0, 1.0);
			shape.Look.Border = new SolidPen (2.0, 0.0, 0.0, 0.0, 1.0);
			shape.Rectangle = new Rectangle (10.0, 10.0, 12.0, 12.0);
			var op = new ShapeTree (shape);
			using (var context = new Cairo.Context (image)) {
				op.Draw (context);
			}

			image.WriteToPng ("testimages/rectangle.png");
			image.Dispose ();
		}
	}
}

