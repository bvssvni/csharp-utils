using System;
using System.Collections.Generic;

namespace Utils
{
	/// <summary>
	/// My approach to classification is to have many systems and different sets of knowledge,
	/// and pick the one that "understands" the context best.
	/// It is assumed that data needs to be understood in term of the context.
	/// </summary>
	public static class ClassifyModule
	{
		/// <summary>
		/// Puts the id on the list if the word exists in dictionary.
		/// </summary>
		/// <param name="id">Identifier.</param>
		/// <param name="dictionary">Dictionary.</param>
		/// <param name="word">The word.</param>
		/// <param name="list">List.</param>
		public static void Classify(int id, List<int> dictionary, int word, List<int> list)
		{
			int ind = IndexingModule.BinarySearch(dictionary, word);
			if (ind >= 0) 
			{
				list.Add(id);
			}
		}

		/// <summary>
		/// Returns a list of indices refering to dictionaries that contains a specific word.
		/// This can be used to detect interference cases.
		/// </summary>
		/// <returns>The indices.</returns>
		/// <param name="dictionaries">Dictionaries.</param>
		/// <param name="word">Word.</param>
		public static List<int> DictionaryIndices(List<List<int>> dictionaries, int word)
		{
			var list = new List<int>();
			int n = dictionaries.Count;
			for (int i = 0; i < n; i++)
			{
				Classify(i, dictionaries[i], word, list);
			}

			return list;
		}
	}
}

