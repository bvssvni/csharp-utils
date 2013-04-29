/*
GLShaderProgram - Used in combination with GLShaderProgramManager.  
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

namespace Utils
{
	/// <summary>
	/// OpenGL shader program.
	/// 
	/// Used in combination with GLShaderProgramManager.
	/// 
	/// This class is used to store key information about program.
	/// The same shaders can be shared with multiple programs.
	/// The attribute locations are stored in the 'AttributeLocations' array.
	/// These are retrieved when linking the program.
	/// </summary>
	public class GLShaderProgram : IComparable<GLShaderProgram> {
		public string VertexShaderFile;
		public int VertexShaderId;
		public string FragmentShaderFile;
		public int FragmentShaderId;
		public string[] Attributes;
		public int[] AttributeLocations;
		
		public GLShaderProgram (string vertexShaderFile, string fragmentShaderFile, params string[] attributes) {
			// Check that file exists.
			if (!System.IO.File.Exists (vertexShaderFile)) throw new Exception ("Vertex shader file does not exists: " + vertexShaderFile);
			if (!System.IO.File.Exists (fragmentShaderFile)) throw new Exception ("Fragment shader file does not exists: " + fragmentShaderFile);
			
			this.VertexShaderFile = vertexShaderFile;
			this.FragmentShaderFile = fragmentShaderFile;
			this.Attributes = attributes;
			this.AttributeLocations = new int[this.Attributes.Length];
		}
		
		public int GetAttributeLocation (string attribute) {
			for (int i = 0; i < Attributes.Length; i++) {
				if (Attributes [i] == attribute) {
					return AttributeLocations [i];
				}
			}
			
			return -1;
		}
		
		#region IComparable implementation
		public int CompareTo (GLShaderProgram other)
		{
			int res = this.VertexShaderFile.CompareTo (other.VertexShaderFile);
			if (res != 0) {
				return res;
			}
			
			return this.FragmentShaderFile.CompareTo (other.FragmentShaderFile);
		}
#endregion
	}
}

