using System;
using System.Collections.Generic;

namespace Utils
{
	/// <summary>
	/// This is a generic tree type.
	/// </summary>
	public class Tree<T>
	{
		public List<T> Items;
		public List<Tree<T>> Nodes;
	}
}

