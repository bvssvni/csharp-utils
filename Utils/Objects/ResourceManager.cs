/*
ResourceManager - Manages resources with reference counters and 'Refresh' for automatic load and unload.  
BSD license.  
by Sven Nilsen, 2012  
http://www.cutoutpro.com  
Version: 0.000 in angular degrees version notation  
http://isprogrammingeasy.blogspot.no/2012/08/angular-degrees-versioning-notation.html  

Redistribution and use in source and binary forms, with or without  
modification, are permitted provided that the following conditions are met:  
1. Redistributions of source code must retain the above copyright notice, this  
list of conditions and the following disclaimer.  
2. Redistributions in binary form must reproduce the above copyright notice,  
this list of conditions and the following disclaimer in the documentation  
and/or other materials provided with the distribution.  
THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND  
ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED  
WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE  
DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR  
ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES  
(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;  
LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND  
ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT  
(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS  
SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.  
The views and conclusions contained in the software and documentation are those  
of the authors and should not be interpreted as representing official policies,  
either expressed or implied, of the FreeBSD Project.  
*/

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
	public class ResourceManager<S, T> where S : IComparable<S>
	{
		protected S[] Keys;
		protected int[] ReferenceCounters;
		public T[] Resources;
		private Group m_loaded = null;
		public LoadDelegate Load;
		public UnloadDelegate Unload;
		
		public delegate T LoadDelegate (S key);
		public delegate void UnloadDelegate (S key, T item);
		
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
		public ResourceManager (params S[] keys)
		{
			Array.Sort<S> (keys);
			Keys = keys;
			ReferenceCounters = new int [Keys.Length];
			Resources = new T [Keys.Length];
		}
		
		public int AddReference (S key) {
			int index = Array.BinarySearch<S> (Keys, key);
			if (index < 0) {
				return -1;
			}
			
			ReferenceCounters [index]++;
			
			return index;
		}
		
		public void IncreaseReference (int refId) {
			ReferenceCounters [refId]++;
		}
		
		public virtual void DecreaseReference (int refId) {
			ReferenceCounters [refId]--;
			if (ReferenceCounters [refId] < 0) {
				throw new Exception ("Released more references than added");
			}
		}
		
		/// <summary>
		/// Unloads unused resources and loads newly used ones.
		/// </summary>
		public virtual void Refresh () {
			// Find items that are in use.
			Group.IsTrue<int> inUseFunction = (int referenceCount) => referenceCount > 0;
			var inUse = Group.Predicate<int> (inUseFunction, ReferenceCounters);
			
			// Unload items that are no longer in use.
			var toUnload = m_loaded == null ? new Group () : m_loaded - inUse;
			foreach (var i in toUnload.IndicesForward ()) {
				Unload (Keys [i], Resources [i]);
				Resources [i] = default(T);
			}
			
			// Load items that are in use.
			var toLoad = m_loaded == null ? inUse : inUse - m_loaded;
			foreach (var i in toLoad.IndicesForward ()) {
				Resources [i] = Load (Keys [i]);
			}
			
			m_loaded = inUse;
		}
		
		public void ResetReferenceCounters () {
			Array.Clear (ReferenceCounters, 0, ReferenceCounters.Length);
		}
	}
}

