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
			var c = new LineShape (0.0, 0.0, 32.0, 32.0);
			var look = new Look (SolidBrush.Red, SolidPen.Black);
			var op = new ShapeTree (look, a);
			op.AddChild (look, b).AddChild (look, c);

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
			shape.Rectangle = new Rectangle () {
				X = 10.0, 
				Y = 10.0, 
				Width = 12.0,
				Height = 12.0
			};
			var look = new Look (SolidBrush.Red, SolidPen.Black);
			var op = new ShapeTree (look, shape);
			using (var context = new Cairo.Context (image)) {
				op.Draw (context);
			}

			image.WriteToPng ("testimages/rectangle.png");
			image.Dispose ();
		}

		[Test()]
		public void TestDrawEllipse ()
		{
			var bytes = new byte[1024 * 4];
			var format = Cairo.Format.ARGB32;
			var image = new Cairo.ImageSurface (bytes, format, 32, 32, 32 * 4); 
			var shape = new EllipseShape ();
			shape.Rectangle = new Rectangle () {
				X = 10.0, 
				Y = 4.0,
				Width = 12.0,
				Height = 18.0
			};
			var look = new Look (SolidBrush.Red, SolidPen.Black);
			var op = new ShapeTree (look, shape);
			using (var context = new Cairo.Context (image)) {
				op.Draw (context);
			}
			
			image.WriteToPng ("testimages/ellipse.png");
			image.Dispose ();
		}

		[Test()]
		public void TestDrawLine ()
		{
			var bytes = new byte[1024 * 4];
			var format = Cairo.Format.ARGB32;
			var image = new Cairo.ImageSurface (bytes, format, 32, 32, 32 * 4); 
			var shape = new LineShape (0.0, 0.0, 32.0, 32.0);
			var look = new Look (SolidBrush.Red, SolidPen.Black);
			var op = new ShapeTree (look, shape);
			using (var context = new Cairo.Context (image)) {
				op.Draw (context);
			}
			
			image.WriteToPng ("testimages/line.png");
			image.Dispose ();
		}
	}
}

