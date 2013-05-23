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

		private static void ComputeEllipse (ComputeTreeModule.ObjectNode obj) {
			int start = 0;
			int end = 0;
			obj.Properties.GetRange (ref start, ref end);
			var x = (double)Cheap<PropertyNode>.Items [start + 0].Value;
			var y = (double)Cheap<PropertyNode>.Items [start + 1].Value;
			var width = (double)Cheap<PropertyNode>.Items [start + 2].Value;
			var height = (double)Cheap<PropertyNode>.Items [start + 3].Value;
			Cheap<PropertyNode>.Items [start + 4].Value = new Rectangle () {
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
			ellipse.Compute (ellipse);
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

