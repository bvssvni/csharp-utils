/*
LineIterationModule - Iterate pixel locations as enumerator.
BSD license.
by Sven Nilsen, 2012
http://www.cutoutpro.com
Version: 0.001 in angular degrees version notation
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
	/// <summary>
	/// Line iteration module.
	/// 
	/// 'FourConnected' iterates pixels side by side.
	/// This is a 2D algorithm.
	/// Z is set to 0.
	/// 
	/// 'EightConnected' iterates pixels through corners.
	/// This is a 2D algorithm.
	/// Z is set to 0.
	/// </summary>
	public class LineIterationModule
	{
		public struct Iteration
		{
			public int X;
			public int Y;
			public int Z;
			public int Index;
		}

		public static IEnumerable<Iteration> FourConnected (int x0, int y0, int x1, int y1) {
			int dx =  Math.Abs(x1-x0), sx = x0<x1 ? 1 : -1;
			int dy = -Math.Abs(y1-y0), sy = y0<y1 ? 1 : -1; 
			int err = dx+dy; /* error value e_xy */

			int i = 0;
			while (!(x0==x1 && y0==y1)){  /* loop */
				yield return new Iteration () {X = x0, Y = y0, Index = i++};
				if ((err<<1) >= dy) { err += dy; x0 += sx; } /* e_xy+e_x > 0 */
				else { err += dx; y0 += sy; } /* e_xy+e_y < 0 */
			}
		}

		public static IEnumerable<Iteration> EightConnected (int x0, int y0, int x1, int y1) {
			int dx = x1 - x0;
			dx = dx < 0 ? -dx : dx;
			int dy = y1 - y0;
			dy = dy < 0 ? -dy : dy;
			int sx = x0 < x1 ? 1 : -1;
			int sy = y0 < y1 ? 1 : -1;
			int err = dx - dy;
			int e2 = 2 * err;
			int i = 0;
			while (x0 != x1 || y0 != y1) {
				yield return new Iteration () { X = x0, Y = y0, Index = i++};

				e2 = 2 * err;
				if (e2 > -dy) {
					err -= dy;
					x0 += sx;
				}
				if (e2 < dx) {
					err += dx;
					y0 += sy;
				}
			}
		}

		private static void Swap<T>(ref T x, ref T y)
		{
			T tmp = y;
			y = x;
			x = tmp;
		}
		
		public static IEnumerable<Iteration> FourteenConnected(int x0, int y0, int z0, int x1, int y1, int z1)
		{
			bool steepXY = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
			if (steepXY) { Swap(ref x0, ref y0); Swap(ref x1, ref y1); }
			
			bool steepXZ = Math.Abs(z1 - z0) > Math.Abs(x1 - x0);
			if (steepXZ) { Swap(ref x0, ref z0); Swap(ref x1, ref z1); }
			
			int deltaX = Math.Abs(x1 - x0);
			int deltaY = Math.Abs(y1 - y0);
			int deltaZ = Math.Abs(z1 - z0);
			
			int errorXY = deltaX / 2, errorXZ = deltaX / 2;
			
			int stepX = (x0 > x1) ? -1 : 1;  
			int stepY = (y0 > y1) ? -1 : 1;
			int stepZ = (z0 > z1) ? -1 : 1;
			
			int y=y0, z=z0;
			
			for(int x = x0; x < x1; x += stepX) 
			{
				int xCopy=x, yCopy=y, zCopy=z;
				
				if (steepXZ) Swap(ref xCopy, ref zCopy);
				if (steepXY) Swap(ref xCopy, ref yCopy);

				yield return new Iteration () { X = xCopy, Y = yCopy, Z = zCopy, Index = x - x0 };
				
				errorXY -= deltaY;
				errorXZ -= deltaZ;
				
				if (errorXY < 0) 
				{
					y += stepY;
					errorXY += deltaX;
				}
				
				if (errorXZ < 0) 
				{
					z += stepZ;
					errorXZ += deltaX;
				}
			}
		}

	}
}

