using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Utils
{
	[TestFixture()]
	public class TestStateModule
	{
		public class TestData {
			public int Val;
		}

		public static bool CountToTen (StateModule<TestData>.State state) {
			state.Data.Val++;
			return state.Data.Val < 10;
		}

		public static bool CountToZero (StateModule<TestData>.State state) {
			state.Data.Val--;
			return state.Data.Val >= 0;
		}

		public static IEnumerable<StateModule<TestData>.StateDelegate> TestStateMachine () {
			yield return CountToTen;
			yield return CountToZero;
			yield return CountToTen;
			yield return CountToZero;
			yield return (state) => {
				// Count up to 10, but quit at 7.
				state.Data.Val++;
				if (state.Data.Val == 7) {
					state.Quit ();
					return false;
				}

				return state.Data.Val < 10;
			};

			yield return null;
		}

		[Test()]
		public void TestCase()
		{
			var data = new TestData () {Val = 0};
			StateModule<TestData>.Run (data, TestStateMachine());
			Assert.True (data.Val == 7);
		}
	}
}

