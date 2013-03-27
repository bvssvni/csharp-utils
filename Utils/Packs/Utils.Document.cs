/*

History - A undo/redo manager.
BSD license.
by Sven Nilsen, 2013
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

namespace Utils.Document
{

	/// <summary>
	/// When implementing this interface,
	/// remember to return the object copied to.
	/// </summary>
	public interface ICopyTo<Argument>
	{
		Argument CopyTo(Argument obj);
	}
	
	public interface IDraw<T>
	{
		void Draw(T context);
	}
	
	public interface IRead<T>
	{
		void Read(T r);
	}
	
	public interface IWrite<T>
	{
		void Save(T writer);
	}
	
	public interface IWriteVersion<T, V>
	{
		void Save(T writer, V version);
	}
	
	public interface IUndoAction<App> :
		IDisposable
	{
		void Undo(App app);
		void Redo(App app);
	}

	public class History<App>
	{
		private List<IUndoAction<App>> m_data;
		private int m_cursor = 0;
		
		public int Cursor
		{
			get {return m_cursor;}
		}
		
		public int Count
		{
			get {
				return m_data.Count;
			}
		}
		
		public History ()
		{
			m_data = new List<IUndoAction<App>>();
		}
		
		public void Undo(App app) {
			if (m_cursor <= 0) return;
			
			m_cursor--;
			m_data[m_cursor].Undo(app);
		}
		
		public void Redo(App app) {
			if (m_cursor >= m_data.Count) return;
			
			m_data[m_cursor].Redo(app);
			m_cursor++;
		}
		
		public void Clear() {
			for (int i = 0; i < m_data.Count; i++) {
				m_data[i].Dispose();
			}
			
			m_data.Clear();
		}
		
		public void AddUndoAction(IUndoAction<App> action) {
			// Clean up unmanaged resources used by the undo actions.
			for (int i = m_cursor; i < m_data.Count - m_cursor; i++) {
				m_data[i].Dispose();
			}
			
			// Remove the history up to the current point.
			m_data.RemoveRange(m_cursor, m_data.Count - m_cursor);
			// Add action to history.
			m_data.Add(action);
			m_cursor++;
		}
	}
	
}

