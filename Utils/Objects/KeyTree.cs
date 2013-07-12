using System;
using System.Collections.Generic;

namespace Utils
{
	public class KeyTree<K, V> where K : IComparable<K>
	{
		public K Key;
		public V Value;
		public KeyTree<K, V>[] SubNodes;

		public KeyTree(int n, K key, V value)
		{
			this.Value = value;
			this.SubNodes = new KeyTree<K, V>[n];
		}

		public void Insert (K key, V value) {

		}
	}

}

