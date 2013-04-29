using System;
using System.Collections.Generic;
using Play;

namespace Utils
{
	/// <summary>
	/// Resource manager.
	/// 
	/// Makes it possible to load and unload resources by use.
	/// For example:
	/// - Textures that are shared among several objects.
	/// - Sound effects that are used by several actions.
	/// 
	/// The resources can be organized in hierarchy,
	/// such that you have one file for each scene,
	/// and each scene loads resources it needs.
	/// </summary>
	public class ResourceManager<T>
	{
		private string[] m_names;
		private int[] m_referenceCounters;
		public T[] Resources;
		private Group m_loaded = null;
		private LoadDelegate m_load;
		private UnloadDelegate m_unload;
		
		public delegate T LoadDelegate (string resource);
		public delegate void UnloadDelegate (T item);

		/// <summary>
		/// Initializes a new instance of the <see cref="Utils.ResourceManager`1"/> class.
		/// </summary>
		/// <param name='load'>
		/// Loads the resource.
		/// </param>
		/// <param name='unload'>
		/// Unload the resource.
		/// </param>
		/// <param name='sortedResources'>
		/// Sorted resources by name.
		/// </param>
		public ResourceManager (LoadDelegate load, UnloadDelegate unload, params string[] sortedNames)
		{
			m_names = sortedNames;
			m_referenceCounters = new int [m_names.Length];
			Resources = new T [m_names.Length];
			m_load = load;
			m_unload = unload;
		}
		
		public int AddReference (string resource) {
			int index = Array.BinarySearch<string> (m_names, resource);
			if (index < 0) {
				return -1;
			}
			
			m_referenceCounters [index]++;
			
			return index;
		}
		
		public void IncreaseReference (int refId) {
			m_referenceCounters [refId]++;
		}
		
		public void DecreaseReference (int refId) {
			m_referenceCounters [refId]--;
			if (m_referenceCounters [refId] < 0) {
				throw new Exception ("Released more references than added");
			}
		}
		
		/// <summary>
		/// Unloads unused resources and loads newly used ones.
		/// </summary>
		public void Refresh () {
			// Find items that are in use.
			Group.IsTrue<int> inUseFunction = (int referenceCount) => referenceCount > 0;
			var inUse = Group.Predicate<int> (inUseFunction, m_referenceCounters);
			
			// Unload items that are no longer in use.
			var toUnload = m_loaded == null ? new Group () : m_loaded - inUse;
			foreach (var i in toUnload.IndicesForward ()) {
				m_unload (Resources [i]);
				Resources [i] = default(T);
			}
			
			// Load items that are in use.
			var toLoad = m_loaded == null ? inUse : inUse - m_loaded;
			foreach (var i in toLoad.IndicesForward ()) {
				Resources [i] = m_load (m_names [i]);
			}
			
			m_loaded = inUse;
		}
	}
}

