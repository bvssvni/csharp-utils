using System;
using NUnit.Framework;
using ObjectNode = Utils.ComputeTreeModule.ObjectNode;
using PropertyNode = Utils.ComputeTreeModule.PropertyNode;

namespace Utils
{
	[TestFixture()]
	public class TestComputeTree
	{
		public struct Rectangle
		{
			public double X;
			public double Y;
			public double Width;
			public double Height;

			public bool IsZero () {
				return 	X == 0.0 &&
						Y == 0.0 &&
						Width == 0.0 &&
						Height == 0.0;
			}
		}

		private static void ComputeEllipse (PropertyNode[] items, int start, int end) {
			var x = (double)items [start + 0].Value;
			var y = (double)items [start + 1].Value;
			var width = (double)items [start + 2].Value;
			var height = (double)items [start + 3].Value;
			items [start + 4].Value = new Rectangle () {
				X = x, Y = y, Width = width, Height = height
			};
		}

		private static void ComputeLength (PropertyNode[] items, int start, int end) {
			var x = (double)items [start + 0].Value;
			var y = (double)items [start + 1].Value;
			var length = Math.Sqrt (x * x + y * y);
			items [start + 2].Value = length;
		}

		private static ObjectNode CreateEllipse (string name,
		                                         double x,
		                                         double y,
		                                         double width,
		                                         double height) {
			var ellipse = new ObjectNode (name, ComputeEllipse,
			                              new PropertyNode ("X", x),
			                              new PropertyNode ("Y", y),
			                              new PropertyNode ("Width", width),
			                              new PropertyNode ("Height", height),
			                              new PropertyNode ("Rectangle", new Rectangle ()));
			return ellipse;
		}

		private static ObjectNode CreateLength (string name,
		                                        double x,
		                                        double y) {
			var length = new ObjectNode (name, ComputeLength,
			                             new PropertyNode ("X", x),
			                             new PropertyNode ("Y", y),
			                             new PropertyNode ("Length", null));
			return length;
		}

		[Test()]
		public void TestEllipse()
		{
			var ellipse = CreateEllipse ("ellipse1", -100.0, -100.0, 200.0, 200.0);
			Assert.True (ellipse.GetProperty<Rectangle> ("Rectangle").IsZero ());
			ellipse.Update ();
			var rect = ellipse.GetProperty<Rectangle> ("Rectangle");
			Assert.True (rect.X == -100.0);
			Assert.True (rect.Y == -100.0);
			Assert.True (rect.Width == 200.0);
			Assert.True (rect.Height == 200.0);
			ellipse.Dispose ();
			Cheap<PropertyNode>.Defragment ();
		}

		[Test()]
		public void TestConnect ()
		{
			var ellipse = CreateEllipse ("ellipse1", -100.0, -100.0, 200.0, 200.0);
			var length = CreateLength ("length1", -100.0, -100.0);
			ellipse.SubNodes = Cheap<ObjectNode>.FromArray (length);
			Assert.True (ComputeTreeModule.Connect (ellipse, "X", length, "Length"));
			length.Update ();
			ellipse.Update ();
			var rect = ellipse.GetProperty<Rectangle> ("Rectangle");
			Assert.True (rect.X > 0.0);
			ellipse.Dispose ();
			Cheap<PropertyNode>.Defragment ();
			Cheap<ObjectNode>.Defragment ();
		}
	}
}

