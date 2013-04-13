using System;
using System.Collections.Generic;
using Utils.Document;

namespace Utils
{
	/// <summary>
	/// Draw layers.
	/// </summary>
	public class Layers<T> : List<IDraw<T>>, IDraw<T>
	{
		public void Draw(T context) {
			int n = this.Count;
			for (int i = 0; i < n; i++) {
				this[i].Draw(context);
			}
		}
	}
}

