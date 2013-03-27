/*
SpriteIsland - GAnalyzes a horizontal sprite sheet sequence and finds offset + width.  
https://github.com/bvssvni/csharp-utils
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

namespace Utils
{
	public class SpriteIsland : IComparable
	{
		public int X;
		public int Y;
		public int Width;
		public int Height;

		public SpriteIsland(int x, int y, int w, int h)
		{
			this.X = x;
			this.Y = y;
			this.Width = w;
			this.Height = h;
		}

		public static SpriteIsland Clip(SpriteIsland a, SpriteIsland b)
		{
			int nx = Math.Max(a.X, b.X);
			int ny = Math.Max(a.Y, b.Y);
			int nw = Math.Min(a.X + a.Width, b.X + b.Width) - nx;
			int nh = Math.Min(a.Y + a.Height, b.Y + b.Height) - ny;
			return new SpriteIsland(nx, ny, nw, nh);
		}

		#region IComparable implementation

		public int CompareTo(object obj)
		{
			var island = obj as SpriteIsland;
			// if (island == null) return -1;

			if (this.X < island.X) return -1;
			if (this.X > island.X) return 1;

			return 0;
		}

		#endregion

		public static bool Intersects(SpriteIsland r, SpriteIsland s) {
			return r.X <= s.X + s.Width && r.Y <= s.Y + s.Height &&
				r.X + r.Width >= s.X && r.Y + r.Height >= s.Y;
		}

		public static void Join(List<SpriteIsland> islands, int i, int j) {
			if (i == j) return;

			var r = islands[i];
			var s = islands[j];
			int nx, ny, nw, nh;
			nx = Math.Min(r.X, s.X);
			ny = Math.Min(r.Y, s.Y);
			nw = Math.Max(r.X + r.Width, s.X + s.Width) - nx;
			nh = Math.Max(r.Y + r.Height, s.Y + s.Height) - ny;
			islands[i] = new SpriteIsland(nx, ny, nw, nh);
			islands.RemoveAt(j);
		}

		
		/// <summary>
		/// Joins the intersecting rectangles.
		/// If two rectangles are joined, it starts over to check for new intersections.
		/// </summary>
		/// <param name='list'>
		/// A list of rectangles which may or may not intersect with each other.
		/// </param>
		public static void JoinOverlaps(List<SpriteIsland> list)
		{
			START_OVER:
			{
				// Construct a new list of joined rectangles.
				int listCount = list.Count;
				for (int i = 0; i < listCount; i++) {
					var r = list [i];
					for (int j = i + 1; j < listCount; j++) {
						var s = list [j];
					
						// Check for intersection between rectangles.
						if (SpriteIsland.Intersects(r, s)) {
							SpriteIsland.Join(list, i, j);
							goto START_OVER;
						}
					}
				}
			}
		}

		public static int HitIndex(List<SpriteIsland> islands, int x, int y) {
			int n = islands.Count;
			for (int i = 0; i < n; i++) {
				var island = islands[i];
				if (x >= island.X && y >= island.Y && x < island.X + island.Width && y < island.Y + island.Height) {
					return i;
				}
			}
			return -1;
		}
	}
}

