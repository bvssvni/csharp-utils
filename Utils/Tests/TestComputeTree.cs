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

		[Test()]
		public void TestEllipse()
		{
			var ellipse = new ObjectNode ("ellipse1", ComputeEllipse,
				new PropertyNode ("X", -100.0),
				new PropertyNode ("Y", -100.0),
				new PropertyNode ("Width", 200.0),
				new PropertyNode ("Height", 200.0),
				new PropertyNode ("Rectangle", new Rectangle ())
            );

			int start = 0;
			int end = 0;
			ellipse.Properties.GetRange (ref start, ref end);
			var rect = (Rectangle)Cheap<PropertyNode>.Items [start + 4].Value;
			Assert.True (rect.X == 0.0);
			Assert.True (rect.Y == 0.0);
			Assert.True (rect.Width == 0.0);
			Assert.True (rect.Height == 0.0);
			ellipse.Update ();
			rect = (Rectangle)Cheap<PropertyNode>.Items [start + 4].Value;
			Assert.True (rect.X == -100.0);
			Assert.True (rect.Y == -100.0);
			Assert.True (rect.Width == 200.0);
			Assert.True (rect.Height == 200.0);
			ellipse.Dispose ();
			Cheap<PropertyNode>.Defragment ();
		}
	}
}

