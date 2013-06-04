using System;
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
			rect.AddTo (manager);
			manager.Commit ();

			rect.X = 10;

			rect.GetFrom (manager, 0);
			Assert.True (rect.X == 0);
			Assert.True (rect.Y == 1);
			Assert.True (rect.Width == 2);
			Assert.True (rect.Height == 3);

			rect.X = 10;
			rect.Update (manager, 0);
			manager.Commit ();

			rect.GetFrom (manager, 0);
			Assert.True (rect.X == 10);
		}

		[Test()]
		public void TestPoint ()
		{
			var manager = new GroupManager ();
			var point = new Point (0, 1);
			point.AddTo (manager);
			manager.Commit ();

			point.X = 10;

			point.GetFrom (manager, 0);
			Assert.True (point.X == 0);
			Assert.True (point.Y == 1);

			point.X = 10;
			point.Update (manager, 0);
			manager.Commit ();

			point.GetFrom (manager, 0);
			Assert.True (point.X == 10);
		}
	}
}

