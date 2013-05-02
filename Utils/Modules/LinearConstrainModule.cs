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
	public class LinearConstrainModule
	{
		/// <summary>
		/// Between the specified min, max, radius, x and newX.
		/// 
		/// Returns false if the radius is too large for the interval.
		/// </summary>
		/// <param name='min'>
		/// Minimum.
		/// </param>
		/// <param name='max'>
		/// Max.
		/// </param>
		/// <param name='radius'>
		/// Radius.
		/// </param>
		/// <param name='x'>
		/// The x coordinate.
		/// </param>
		/// <param name='newX'>
		/// New x.
		/// </param>
		public static bool Between (float min, float max, float radius, float x, out float newX) {
			bool smaller = x - radius < min;
			bool larger = x + radius > max;
			if (smaller && larger) {
				newX = x;
				return false;
			}
			if (!smaller && !larger) {
				newX = x;
				return true;
			}
			if (smaller) {
				newX = min + radius;
			} else {
				newX = max - radius;
			}

			return true;
		}

		public static bool Within (RectangleF rect, float radius, float x, float y, out float newX, out float newY) {
			newX = x;
			newY = y;
			bool smallerX = x - radius < rect.X;
			bool largerX = x + radius > rect.X + rect.Width;
			bool smallerY = y - radius < rect.Y;
			bool largerY = y + radius > rect.Y + rect.Height;
			if (smallerX && largerX || smallerY && largerY) {
				return false;
			}
			if (smallerX) {
				newX = rect.X + radius;
			} else if (largerX) {
				newX = rect.X + rect.Width - radius;
			}
			if (smallerY) {
				newY = rect.Y + radius;
			} else if (largerY) {
				newY = rect.Y + rect.Height - radius;
			}

			return true;
		}
	}
}

