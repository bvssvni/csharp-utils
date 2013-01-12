using System;
using NUnit.Framework;

namespace Utils
{
	[TestFixture()]
	public class Test
	{
		[Test()]
		public void TestIntersect1()
		{
			Group a = new Group();
			a.Add(0);
			a.Add(10);

			Group b = new Group();
			b.Add(5);
			b.Add(15);

			Group c = a * b;
			Assert.AreEqual(2, c.Count);
			Assert.AreEqual(5, c [0]);
			Assert.AreEqual(10, c [1]);
		}

		[Test()]
		public void TestIntersect2()
		{
			Group a = new Group();
			a.Add(0);
			a.Add(10);
			
			Group b = new Group();
			
			Group c = a * b;
			Assert.AreEqual(true, Group.IsEmpty(c));
		}

		[Test()]
		public void TestUnion1()
		{
			Group a = new Group();
			a.Add(0);
			a.Add(10);

			Group b = new Group();
			b.Add(9);
			b.Add(12);

			Group c = a + b;
			Assert.AreEqual(2, c.Count);
			Assert.AreEqual(0, c [0]);
			Assert.AreEqual(12, c [1]);
		}

		[Test()]
		public void TestUnion2()
		{
			Group a = new Group();
			a.Add(0);
			a.Add(10);
			
			Group b = new Group();
			
			Group c = a + b;
			Assert.True(a.CompareTo(c) == 0);
		}

		[Test()]
		public void TestSubtract1()
		{
			Group a = new Group();
			a.Add(0);
			a.Add(10);
			
			Group b = new Group();
			b.Add(9);
			b.Add(12);
			
			Group c = a - b;
			Assert.AreEqual(2, c.Count);
			Assert.AreEqual(0, c [0]);
			Assert.AreEqual(9, c [1]);
		}

		[Test()]
		public void TestAdd1()
		{
			Group a = new Group();
			Assert.AreEqual(0, a.Count);
			a += 0;
			Assert.AreEqual(2, a.Count);
			a += 0;
			Assert.AreEqual(1, a [1]);
			a += 1;
			Assert.AreEqual(2, a [1]);
		}

		[Test()]
		public void TestIsEmpty1()
		{
			Group a = new Group();
			Assert.AreEqual(true, Group.IsEmpty(a));
			a += 5;
			Assert.AreEqual(false, Group.IsEmpty(a));
		}

		[Test()]
		public void TestSize()
		{
			Group a = new Group();
			a.Add(0);
			a.Add(10);
			Assert.AreEqual(10, Group.Size(a));
			Assert.True(a.CompareTo(5) > 0);
			Assert.True(a.CompareTo(12) < 0);
			Assert.True(a.CompareTo(10) <= 0);
			Assert.False(a.CompareTo(10) < 0);
			Assert.True(a.CompareTo(10) >= 0);
		}

		[Test()]
		public void TestForward()
		{
			var arr = new int[] {0,1,2,3,4,5,6};
			var g = Group.Slice(2, 4);
			int j = 0;
			foreach (int i in g.Forward<int>(arr)) {
				Console.WriteLine(i.ToString());
				Assert.True(i == arr [j + 2]);
				j++;
			}
		}

		[Test()]
		public void TestBackward()
		{
			var arr = new int[] {0,1,2,3,4,5,6};
			var g = Group.Slice(2, 4);
			int j = 4;
			foreach (int i in g.Backward<int>(arr)) {
				Console.WriteLine(i.ToString());
				Assert.True(i == arr [j]);
				j--;
			}
		}

		[Test()]
		public void TestFilter()
		{
			var g = new Group(new int[]{1,100});
			g *= delegate (int i) {
				return i % 10 == 0;
			};
			Assert.True(g [0] == 10);
			Assert.True(g [1] == 11);
			Assert.True(g [2] == 20);
			Assert.True(g [3] == 21);
			Assert.True(g [4] == 30);
			Assert.True(g [5] == 31);
			Assert.True(g [6] == 40);
			Assert.True(g [7] == 41);
			Assert.True(g [8] == 50);
			Assert.True(g [9] == 51);
			Assert.True(g [10] == 60);
			Assert.True(g [11] == 61);
			Assert.True(g [12] == 70);
			Assert.True(g [13] == 71);
			Assert.True(g [14] == 80);
			Assert.True(g [15] == 81);
			Assert.True(g [16] == 90);
			Assert.True(g [17] == 91);
			Assert.True(g.CompareTo(9) == 0);
		}

		[Test()]
		public void TestCompareGroups()
		{
			var a = new Group(new int[]{13, 15});
			var b = new Group(new int[]{13, 15});
			var c = new Group(new int[]{13, 14});

			Assert.True(a.CompareTo(b) == 0);
			Assert.True(a.CompareTo(c) > 0);
			Assert.True(b.CompareTo(c) > 0);
			Assert.True(a.CompareTo(c) >= 0);
			Assert.True(b.CompareTo(c) != 0);
		}

		[Test()]
		public void TestMostSimilar()
		{
			var a = new Group(new int[]{5, 6});
			var b = new Group(new int[]{4, 6});
			var c = new Group(new int[]{5, 7});
			var d = new Group(new int[]{0, 10});
			var gr = new Group[]{b, c, d};
			var all = Group.Slice(0, gr.Length-1);

			var ind = a.MostSimilar(gr, all);
			Assert.True(ind == 0);

			gr = new Group[]{c, d, b};
			ind = a.MostSimilar(gr, all);
			Assert.True(ind == 2);

			gr = new Group[]{d, b, c};
			ind = a.MostSimilar(gr, all);
			Assert.True(ind == 1);

			gr = new Group[]{a, b, c, d};
			ind = a.MostSimilar(gr, all);
			Assert.True(ind == 0);
		}

		[Test()]
		public void TestBoolSamples()
		{
			var a = new bool[]{true, true, false, false, true, true};
			var b = Group.FromBoolSamples(a);
			var c = new Group(new int[]{0, 2, 4, 6});

			Assert.True(b.CompareTo(c) == 0);
		}

		[Test()]
		public void TestMaxMinInterval()
		{
			var a = new Group(new int[]{0, 3, 4, 12});
			var b = a.MaxInterval();
			var c = new Group(new int[]{4, 12});
			var d = a.MinInterval();
			var e = new Group(new int[]{0, 3});

			Assert.True(b.CompareTo(c) == 0);
			Assert.True(d.CompareTo(e) == 0);
		}

		[Test()]
		public void TestMaxMinLeap()
		{
			var a = new Group(new int[]{7, 9, 10, 20});
			var b = a.MaxLeap();
			var c = new Group(new int[]{0, 7});
			var d = a.MinLeap();
			var e = new Group(new int[]{9, 10});

			Assert.True(b.CompareTo(c) == 0);
			Assert.True(d.CompareTo(e) == 0);
		}

		[Test()]
		public void TestExternalInternal()
		{
			var a = new Group(new int[]{4, 5, 8, 10});
			Assert.True(a.External(0) == 4);
			Assert.True(a.External(1) == 8);
			Assert.True(a.External(2) == 9);
			Assert.True(a.External(3) == -1);
			Assert.True(a.External(4) == -1);

			Assert.True(a.Internal(0) == -1);
			Assert.True(a.Internal(1) == -1);
			Assert.True(a.Internal(2) == -1);
			Assert.True(a.Internal(3) == -1);
			Assert.True(a.Internal(4) == 0);
			Assert.True(a.Internal(5) == -1);
			Assert.True(a.Internal(6) == -1);
			Assert.True(a.Internal(7) == -1);
			Assert.True(a.Internal(8) == 1);
			Assert.True(a.Internal(9) == 2);
			Assert.True(a.Internal(10) == -1);
			Assert.True(a.Internal(11) == -1);
		}

		[Test()]
		public void TestExternal()
		{
			var a = Group.Slice (3, 3);
			Assert.True(a.External(0) == 3);
		}

		[Test()]
		public void TestContainsIndex()
		{
			var a = new Group(new int[]{4, 5, 8, 10});
			Assert.True(a.ContainsIndex(4));
			Assert.False(a.ContainsIndex(5));
			Assert.False(a.ContainsIndex(7));
			Assert.True(a.ContainsIndex(8));
			Assert.True(a.ContainsIndex(9));
			Assert.False(a.ContainsIndex(10));
		}

		[Test()]
		public void TestFirst()
		{
			var gr = new Group[]{Group.Slice (0, 2), Group.Slice(3, 3), Group.Slice (4, 6)};
			Assert.True(Group.First(gr, 0) == 0);
			Assert.True(Group.First(gr, 3) == 1);
			Assert.True(Group.First(gr, 4) == 2);
		}
	}
}

