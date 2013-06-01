using System;
using NUnit.Framework;
using Play;

namespace Utils
{
	[TestFixture()]
	public class TestGroupManager
	{
		[Test()]
		public void TestAdd()
		{
			var manager = new GroupManager ();
			var wordsOfLengthFour = new GroupManager.Controller () {
				Filter = (object[] word) => ((string)word[0]).Length == 4,
			};

			manager.AddController (wordsOfLengthFour);
			manager.Add ("banana");
			Assert.True (Group.Size (wordsOfLengthFour.Group) == 0);
			manager.Add ("pear");
			Assert.True (Group.Size (wordsOfLengthFour.Group) == 0);
			Assert.True (manager.Count == 0);
			manager.Refresh ();
			Assert.True (Group.Size (wordsOfLengthFour.Group) == 1);
			Assert.True (manager.Count == 2);
			Assert.True (wordsOfLengthFour.Group.CompareTo (new Group (new int[] {1, 2})) == 0);
		}

		[Test()]
		public void TestRemoveAt ()
		{
			var manager = new GroupManager ();
			manager.Add ("apple");
			manager.Add ("pear");
			manager.Add ("orange");
			Assert.True (manager.Count == 0);
			manager.Refresh ();
			manager.RemoveAt (1);
			Assert.True (manager.Count == 3);
			manager.Refresh ();
			Assert.True (manager.Count == 2);
			Assert.True (((string)manager[0, 0]) == "apple");
			Assert.True (((string)manager[1, 0]) == "orange");
		}

		[Test()]
		public void TestPropertyChanged ()
		{
			var manager = new GroupManager ();
			var wordsOfLengthFour = new GroupManager.Controller () {
				Filter = (object[] word) => ((string)word[0]).Length == 4,
			};
			
			manager.AddController (wordsOfLengthFour);
			manager.Add ("banana");
			Assert.True (Group.Size (wordsOfLengthFour.Group) == 0);
			manager.Add ("pear");
			Assert.True (Group.Size (wordsOfLengthFour.Group) == 0);
			Assert.True (manager.Count == 0);
			manager.Refresh ();
			Assert.True (Group.Size (wordsOfLengthFour.Group) == 1);

			manager[1, 0] = "orange";
			manager.Refresh ();
			Assert.True (Group.Size (wordsOfLengthFour.Group) == 0);
		}

		[Test()]
		public void TestPropertyChangedNotCalledWhenRemoved ()
		{
			var manager = new GroupManager ();
			var wordsOfLengthFour = new GroupManager.Controller () {
				Filter = (object[] word) => ((string)word[0]).Length == 4,
				AddMember = (object[] word) => {
					throw new NotImplementedException ();
				}
			};
			
			manager.AddController (wordsOfLengthFour);
			manager.Add ("banana");
			manager.Refresh ();

			manager[0, 0] = "pear";
			manager.RemoveAt (0);
			manager.Refresh ();

			Assert.True (manager.Count == 0);
			Assert.True (Group.Size (wordsOfLengthFour.Group) == 0);
		}
	}
}

