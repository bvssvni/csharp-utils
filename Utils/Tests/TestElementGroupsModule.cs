using System;
using NUnit.Framework;
using Play;

namespace Utils
{
	[TestFixture()]
	public class TestElementGroupsModule
	{
		[Test()]
		public void TestCase()
		{
			var a = new Group(new int[]{0, 4});
			Assert.False(a.ContainsIndex(4));

			var b = new Group(new int[]{2, 6});
			var dict = ElementGroupsModule.ElementGroups(new Group[]{a, b});

			var g0 = dict[new bool[]{true, false}];
			Assert.True(g0[0] == 0);
			Assert.True(g0[1] == 2);

			var g1 = dict[new bool[]{true, true}];
			Assert.True(g1[0] == 2);
			Assert.True(g1[1] == 4);

			var g2 = dict[new bool[]{false, true}];
			Assert.True(g2[0] == 4);
			Assert.True(g2[1] == 6);
		}
	}
}

