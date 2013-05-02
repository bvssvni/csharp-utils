/*
GLColorModule - Generates useful data for dealing with colors in OpenGL.
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
	/// OpenGL color module.
	/// 
	/// Creates data for use with OpenGL.
	/// </summary>
	public class GLColorModule
	{
		public static byte[] ByteArrayFromColor (int vertices, Color color) {
			byte r = color.R;
			byte g = color.G;
			byte b = color.B;
			byte a = color.A;
			int m = vertices * 4;
			byte[] arr = new byte[m];
			for (int i = 0; i < vertices; ++i) {
				arr[4 * i + 0] = r;
				arr[4 * i + 1] = g;
				arr[4 * i + 2] = b;
				arr[4 * i + 3] = a;
			}
			
			return arr;
		}
		
		public static byte[] ByteArrayFromColors (params Color[] colors) {
			int m = colors.Length * 4;
			Color c;
			byte[] arr = new byte[m];
			int n = colors.Length;
			for (int i = 0; i < n; ++i) {
				c = colors[i];
				arr[4 * i + 0] = c.R;
				arr[4 * i + 1] = c.G;
				arr[4 * i + 2] = c.B;
				arr[4 * i + 3] = c.A;
			}
			
			return arr;
		}
	}
}

