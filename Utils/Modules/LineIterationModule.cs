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
			int dx = x1 - x0;
			dx = dx < 0 ? -dx : dx;
			int dy = y1 - y0;
			dy = dy < 0 ? -dy : dy;
			int ix = x0 < x1 ? 1 : -1;
			int iy = y0 < y1 ? 1 : -1;
			int e = 0;
			int e1 = 0;
			int e2 = 0;
			int n = dx + dy;
			for (int i = 0; i < n; ++i) {
				yield return new Iteration () { X = x0, Y = y0, Index = i };
				
				e1 = e + dy;
				e2 = e - dx;
				if ( (e1 < 0 ? -e1 : e1) < (e2 < 0 ? -e2 : e2) ) {
					x0 += ix;
					e = e1;
				} else {
					y0 += iy;
					e = e2;
				}
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

	}
}

