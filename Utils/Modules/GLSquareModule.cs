 /*
GLSquareModule - Creating and drawing squares in OpenGL.  
BSD license.  
by Sven Nilsen, 2013  
http://www.cutoutpro.com  
Version: 0.002 in angular degrees version notation  
http://isprogrammingeasy.blogspot.no/2012/08/angular-degrees-versioning-notation.html  

0.002 - Added drawing of positions only.
0.001 - Added color 3 method.

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
using OpenTK.Graphics.ES20;

namespace Utils
{
	public class GLSquareModule
	{
		public static float[] CreateVertices (float x, float y, float w, float h) {
			return new float[] {
				x, y,
				x + w, y,
				x, y + h,
				x + w, y + h,
			};
		}
		
		public static void DrawTriangleStripPositionColor (int verticesAttribute, int colorAttribute, float[] vertices, byte[] colors) {
			GL.VertexAttribPointer<float> (verticesAttribute, 2, VertexAttribPointerType.Float, false, 0, vertices);
			GL.EnableVertexAttribArray (verticesAttribute);
			GL.VertexAttribPointer<byte> (colorAttribute, 4, VertexAttribPointerType.UnsignedByte, true, 0, colors);
			GL.EnableVertexAttribArray (colorAttribute);
			GL.DrawArrays (BeginMode.TriangleStrip, 0, 4);
		}
		
		public static void DrawTriangleStripPositionColor3 (int verticesAttribute, int colorAttribute, float[] vertices, byte[] colors) {
			GL.VertexAttribPointer<float> (verticesAttribute, 2, VertexAttribPointerType.Float, false, 0, vertices);
			GL.EnableVertexAttribArray (verticesAttribute);
			GL.VertexAttribPointer<byte> (colorAttribute, 3, VertexAttribPointerType.UnsignedByte, true, 0, colors);
			GL.EnableVertexAttribArray (colorAttribute);
			GL.DrawArrays (BeginMode.TriangleStrip, 0, 4);
		}
		
		public static void DrawTriangleStripPosition (int verticesAttribute, float[] vertices) {
			GL.VertexAttribPointer<float> (verticesAttribute, 2, VertexAttribPointerType.Float, false, 0, vertices);
			GL.EnableVertexAttribArray (verticesAttribute);
			GL.DrawArrays (BeginMode.TriangleStrip, 0, 4);
		}
	}
}

