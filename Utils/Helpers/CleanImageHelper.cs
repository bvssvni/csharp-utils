/*
CleanImageHelper - Cleans image by looking for maximum alpha value in islands.  
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
	/// <summary>
	/// Erases all islands that have no alpha pixel above a certain limit.
	/// </summary>
	public class CleanImageHelper
	{
		private Pixbuf m_image;
		private byte m_maxAlpha;
		
		public void Step1_SetImage(Pixbuf image)
		{
			m_image = image;
		}
		
		public void Step2_SetMaxAlpha(byte alpha)
		{
			m_maxAlpha = alpha;
		}
		
		public void Step3_Clean()
		{
			Pixbuf img = m_image;
			var islands = SpriteAnalyzer.FindIslands(img);
			byte maxAlpha = m_maxAlpha;
			foreach (var island in islands) {
				byte alpha = CleanImageModule.FindMaxAlpha(m_image, island);
				if (alpha >= maxAlpha)  continue;
				
				CleanImageModule.Erase(img, island);
			}
		}
	}
}

