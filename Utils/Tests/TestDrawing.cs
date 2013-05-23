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
		public void TestCase()
		{
			var a = new EllipseShape ();
			var b = new RectangleShape ();
			var c = new LineShape ();
			var aOp = new ShapeTree (a);
			aOp.AddChild (b).AddChild (c);
			var list = new List<ShapeBase> ();
			foreach (var item in aOp) {
				list.Add (item);
			}

			Assert.True (list[0] == b);
			Assert.True (list[1] == c);
		}
	}
}

