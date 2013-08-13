using System;
using NUnit.Framework;

namespace Utils
{
	[TestFixture()]
	public class TestLazyPipeline
	{
		private class TestTask
		{
			public DateTime Start;
			public TimeSpan Wait;
		}

		[Test()]
		public void TestCase()
		{
			var pipeline = new LazyPipeline<TestTask>();
			// Create a thousand tasks of waiting one second.
			for (int i = 0; i < 1000; i++)
			{
				pipeline.Tasks.Add(new TestTask(){Wait = TimeSpan.FromSeconds(1.0)});
			}

			int k = 0;
			var waitState = new LazyPipeline<TestTask>.State()
			{
				Start = (t) => {
					t.Start = DateTime.Now;
					return true;
				},
				End = (t) => {
					var res = DateTime.Now.CompareTo(t.Start.Add(t.Wait)) > 0;
					if (res)
					{
						// Make sure the tasks are ended sequentially.
						Assert.True(t == pipeline.Tasks[k++]);
					}

					return res;
				}
			};
			pipeline.States.Add(waitState);

			var start = DateTime.Now;
			while (pipeline.Tick())
			{
				pipeline.Flush();
			}

			var end = DateTime.Now;
			var totalSeconds = end.Subtract(start).TotalSeconds;
			Assert.True(totalSeconds < 2.0);
		}
	}
}

