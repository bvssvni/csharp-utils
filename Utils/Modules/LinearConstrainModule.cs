/*
LinearConstrainModule - Returning a position with radius within a limited range.  
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
using System.Drawing;

namespace Utils
{
	/// <summary>
	/// Linear constrain module.
	/// 
	/// Returns true if it is possible to put coordinate inside.
	/// 
	/// Computes displacement.
	/// </summary>
	public class LinearConstrainModule
	{
		public static void WithinMultiple (int offset, int count, 
		                                   float[] min, float[] max, 
		                                   float[] radius, float[] x, float[] dx, 
		                                   bool[] fit) {
			int min_dim = min.Length == 1 ? 0 : 1;
			int max_dim = max.Length == 1 ? 0 : 1;
			int radius_dim = radius.Length == 1 ? 0 : 1;
			int x_dim = x.Length == 1 ? 0 : 1;
			bool smaller, larger;
			float _min, _max, _x, _radius;

			for (int i = count - 1; i >= 0; --i) {
				_min = min[i * min_dim + offset];
				_max = max[i * max_dim + offset];
				_x = x[i * x_dim + offset];
				_radius = radius[i * radius_dim + offset];

				smaller = _x - _radius < _min;
				larger = _x + _radius > _max;
				dx[i] = 0;
				if (smaller && larger) {
					fit[i] = false;
					continue;
				}
				if (smaller) {
					dx[i] = _min + _radius - _x;
				} else if (larger) {
					dx[i] = _max - _radius - _x;
				}
				fit[i] = true;
			}
		}

		public static bool Between (float min, float max, float radius, float x, out float dx) {
			bool smaller = x - radius < min;
			bool larger = x + radius > max;
			dx = 0;
			if (smaller && larger) {
				return false;
			}
			if (smaller) {
				dx = min + radius - x;
			} else if (larger) {
				dx = max - radius - x;
			}

			return true;
		}

		public static bool Within (RectangleF rect, float radius, 
		                           float x, float y,
		                           out float dx, out float dy) {
			dx = 0;
			dy = 0;
			bool smallerX = x - radius < rect.X;
			bool largerX = x + radius > rect.X + rect.Width;
			bool smallerY = y - radius < rect.Y;
			bool largerY = y + radius > rect.Y + rect.Height;
			if (smallerX && largerX || smallerY && largerY) {
				return false;
			}
			if (smallerX) {
				dx = rect.X + radius - x;
			} else if (largerX) {
				dx = rect.X + rect.Width - radius - x;
			}
			if (smallerY) {
				dy = rect.Y + radius - y;
			} else if (largerY) {
				dy = rect.Y + rect.Height - radius - y;
			}

			return true;
		}
	}
}

