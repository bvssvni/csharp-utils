/*
GLShaderModule - Low level methods for compiling and linking OpenGL shaders.  
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
using OpenTK.Graphics.ES20;

namespace Utils
{
	/// <summary>
	/// GL shader module.
	/// 
	/// This contains low level functions to deal with shaders.
	/// </summary>
	public class GLShaderModule
	{
		
		public static bool CompileShader (ShaderType type, string file, out int shader)
		{
			string src = System.IO.File.ReadAllText (file);
			shader = GL.CreateShader (type);
			GL.ShaderSource (shader, 1, new string[] { src }, (int[])null);
			GL.CompileShader (shader);
			
#if DEBUG
			int logLength = 0;
			GL.GetShader (shader, ShaderParameter.InfoLogLength, out logLength);
			if (logLength > 0) {
				var infoLog = new System.Text.StringBuilder ();
				GL.GetShaderInfoLog (shader, logLength, out logLength, infoLog);
				Console.WriteLine ("Shader compile log:\n{0}", infoLog);
			}
#endif
			int status = 0;
			GL.GetShader (shader, ShaderParameter.CompileStatus, out status);
			if (status == 0) {
				GL.DeleteShader (shader);
				return false;
			}
			
			return true;
		}
		
		public static bool LinkProgram (int prog)
		{
			GL.LinkProgram (prog);
			
#if DEBUG
			int logLength = 0;
			GL.GetProgram (prog, ProgramParameter.InfoLogLength, out logLength);
			if (logLength > 0) {
				var infoLog = new System.Text.StringBuilder ();
				GL.GetProgramInfoLog (prog, logLength, out logLength, infoLog);
				Console.WriteLine ("Program link log:\n{0}", infoLog);
			}
#endif
			int status = 0;
			GL.GetProgram (prog, ProgramParameter.LinkStatus, out status);
			if (status == 0)
				return false;
			
			return true;
		}
		
		/// <summary>
		/// Validates the program.
		/// 
		/// This can be done in debug mode in the render loop.
		/// </summary>
		/// <returns><c>true</c>, if program was validated, <c>false</c> otherwise.</returns>
		/// <param name="prog">Prog.</param>
		public static bool ValidateProgram (int prog)
		{
			GL.ValidateProgram (prog);
			
			int logLength = 0;
			GL.GetProgram (prog, ProgramParameter.InfoLogLength, out logLength);
			if (logLength > 0) {
				var infoLog = new System.Text.StringBuilder ();
				GL.GetProgramInfoLog (prog, logLength, out logLength, infoLog);
				Console.WriteLine ("Program validate log:\n{0}", infoLog);
			}
			
			int status = 0;
			GL.GetProgram (prog, ProgramParameter.LinkStatus, out status);
			if (status == 0)
				return false;
			
			return true;
		}
		
	}
}

