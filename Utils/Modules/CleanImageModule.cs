/*
CleanImageModule - Contains useful methods for cleaning up artifacts in image.  
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
using Gdk;

namespace Utils
{
	public class CleanImageModule
	{
		/// <summary>
		/// Finds the maximum alpa within an island.
		/// </summary>
		/// <returns>
		/// The max alpha value found in island.
		/// </returns>
		/// <param name='buf'>
		/// The image to containing the island.
		/// </param>
		/// <param name='rect'>
		/// A region to check for maximum alpha.
		/// </param>
		public static byte FindMaxAlpha(Pixbuf buf, SpriteIsland rect)
		{
			// Make sure not to read and write outside the image.
			var clip = SpriteIsland.Clip(rect, new SpriteIsland(0, 0, buf.Width, buf.Height));
			byte max = 0;
			unsafe {
				byte* start = (byte*)buf.Pixels;
				int stride = buf.Rowstride;
				int startX = clip.X;
				int startY = clip.Y;
				int endX = clip.X + clip.Width;
				int endY = clip.Y + clip.Height;
				for (int y = startY; y < endY; y++) {
					for (int x = startX; x < endX; x++) {
						byte* current = start + 4 * x + y * stride;
						byte alpha = current[3];
						if (alpha > max) max = alpha;
					}
				}
			}
			return max;
		}

		
		/// <summary>
		/// Erase an island from image.
		/// </summary>
		/// <param name='buf'>
		/// The image to erase the island from.
		/// </param>
		/// <param name='rect'>
		/// An island to erase all pixels.
		/// </param>
		public static byte Erase(Pixbuf buf, SpriteIsland rect)
		{
			// Make sure not to read and write outside the image.
			var clip = SpriteIsland.Clip(rect, new SpriteIsland(0, 0, buf.Width, buf.Height));
			byte max = 0;
			unsafe {
				byte* start = (byte*)buf.Pixels;
				int stride = buf.Rowstride;
				int startX = clip.X;
				int startY = clip.Y;
				int endX = clip.X + clip.Width;
				int endY = clip.Y + clip.Height;
				for (int y = startY; y < endY; y++) {
					for (int x = startX; x < endX; x++) {
						byte* current = start + 4 * x + y * stride;
						current[0] = current[1] = current[2] = current[3] = 0;
					}
				}
			}
			return max;
		}
	}
}

