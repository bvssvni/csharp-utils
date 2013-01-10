
using System;
using NUnit.Framework;

namespace Utils
{
	[TestFixture()]
	public class TestRunScript
	{
		[Test()]
		public void TestCase ()
		{
			// Run script directly.
			Assert.True (RunScript.Evaluate<int>("1+1;") == 2);

			// Compile script and then evaluate it.
			var method = RunScript.Compile("1+1;");
			var obj = new object();
			method(ref obj);
			var val = (int)Convert.ChangeType(obj, typeof(int));
			Assert.True(val == 2);

			Assert.True(RunScript.Execute<int>(method) == 2);
		}
	}
}

