/*
GLShaderProgramManager - Manages shader programs from a directory.  
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
using OpenTK.Graphics.ES20;

namespace Utils
{
	/// <summary>
	/// OpenGL shader program manager.
	/// 
	/// This class stores the shader programs internally.
	/// Each shader program has a vertex and a fragment shader.
	/// 
	/// It allows you to reuse compiled shader programs between multiple scenes.
	/// 
	/// To clean up, call
	/// 
	/// 	manager.ResetReferenceCounters ();
	/// 	manager.Refresh ();
	/// 
	/// </summary>
	public class GLShaderProgramManager : Utils.ResourceManager<GLShaderProgram, int>
	{
		// Contains shader files with gl id.
		private Utils.ResourceManager<string, int> m_shaders;
		
		public GLShaderProgramManager (string directory, params GLShaderProgram[] programs) : base (programs)
		{
			// Use files that ends with 'vsh' and 'fsh' extensions.
			m_shaders = new Utils.ResourceManager<string, int> (Utils.FindFilesModule.FindFiles (directory, true, "*.vsh", "*.fsh"));
			m_shaders.Load = LoadShader;
			m_shaders.Unload = UnloadShader;
			base.Load = LoadProgram;
			base.Unload = UnloadProgram;
		}
		
		private static int LoadShader (string file) {
			// Detect shader type from the file extension.
			var shaderType = file.EndsWith (".vsh") ? ShaderType.VertexShader : ShaderType.FragmentShader;
			int shader;
			if (!Utils.GLShaderModule.CompileShader (shaderType, file, out shader)) {
				Console.WriteLine ("Failed to compile vertex shader {0}", file);
				return -1;
			}
			
			return shader;
		}
		
		private static void UnloadShader (string file, int shader) {
			GL.DeleteShader (shader);
		}
		
		private int LoadProgram (GLShaderProgram programSettings) {
			int vertShader = m_shaders.Resources [programSettings.VertexShaderId];
			int fragShader = m_shaders.Resources [programSettings.FragmentShaderId];
			
			int program = GL.CreateProgram ();
			
			GL.AttachShader (program, vertShader);
			GL.AttachShader (program, fragShader);
			
			// Link program.
			if (!Utils.GLShaderModule.LinkProgram (program)) {
				Console.WriteLine ("Failed to link program: {0:x}", program);
				
				if (vertShader != 0)
					GL.DeleteShader (vertShader);
				
				if (fragShader != 0)
					GL.DeleteShader (fragShader);
				
				if (program != 0) {
					GL.DeleteProgram (program);
					program = 0;
				}
				return -1;
			}
			
			// Get the attribute locations.
			var attributes = programSettings.Attributes;
			for (int i = 0; i < attributes.Length; ++i) {
				programSettings.AttributeLocations[i] = GL.GetAttribLocation (program, attributes [i]);
			}
			
			// Release vertex and fragment shaders.
			if (vertShader != 0) {
				GL.DetachShader (program, vertShader);
				m_shaders.DecreaseReference (programSettings.VertexShaderId);
			}
			
			if (fragShader != 0) {
				GL.DetachShader (program, fragShader);
				m_shaders.DecreaseReference (programSettings.FragmentShaderId);
			}
			
			return program;
		}
		
		/// <summary>
		/// Unloads the shader program.
		/// </summary>
		/// <param name="program">Program.</param>
		/// <param name="id">Identifier.</param>
		private void UnloadProgram (GLShaderProgram program, int id) {
			GL.DeleteProgram (id);
		}
		
		public override void Refresh () {
			// Reset the shader references since no shader is used outside a program.
			m_shaders.ResetReferenceCounters ();
			int n = Resources.Length;
			for (int i = 0; i < n; i++) {
				var key = Keys[i];
				key.VertexShaderId = m_shaders.AddReference (key.VertexShaderFile);
				key.FragmentShaderId = m_shaders.AddReference (key.FragmentShaderFile);
			}
			
			m_shaders.Refresh ();
			base.Refresh ();
		}
	}
}

