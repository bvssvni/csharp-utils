using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Utils
{
	[TestFixture()]
	public class TestResourceManager
	{
		private class ResourceItem {
			public string Name;

			public ResourceItem (string name) {
				this.Name = name;
			}
		}

		[Test()]
		public void TestLoadUnload()
		{
			ResourceManager<string, ResourceItem>.LoadDelegate load = (string resource) => new ResourceItem (resource);
			ResourceManager<string, ResourceItem>.UnloadDelegate unload = (string resource, ResourceItem item) => item = null;
			var manager = new ResourceManager<string, ResourceItem> ("hello");
			manager.Load = load;
			manager.Unload = unload;

			Assert.True (manager.Resources [0] == null);
			var helloId = manager.AddReference ("hello");
			Assert.True (manager.Resources [0] == null);
			manager.Refresh ();
			Assert.True (manager.Resources [0].Name == "hello");
			manager.DecreaseReference (helloId);
			Assert.True (manager.Resources [0].Name == "hello");
			manager.Refresh ();
			Assert.True (manager.Resources [0] == null);
		}

		[Test()]
		public void TestNoInitialResources()
		{
			var manager = new ResourceManager<string, ResourceItem> ();
			Assert.True (manager.Resources != null);
		}
	}
}

