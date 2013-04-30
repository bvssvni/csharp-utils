/*

Utils.View - Methods for mapping drawing routines in constructs.
BSD license.
by Sven Nilsen, 2013
http://www.cutoutpro.com
Version: 0.001 in angular degrees version notation
http://isprogrammingeasy.blogspot.no/2012/08/angular-degrees-versioning-notation.html

002 - Added ImageView.
001 - Added PixelPathView.

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
using Utils.Document;
using Cairo;

namespace Utils.View
{
	/// <summary>
	/// Draw layers.
	/// </summary>
	public class LayersView<T> : List<IDraw<T>>, IDraw<T>
	{
		public void Draw(T context) {
			int n = this.Count;
			for (int i = 0; i < n; i++) {
				this[i].Draw(context);
			}
		}
	}
	
	/// <summary>
	/// Offset view.
	/// 
	/// Scales and translates view correctly.
	/// The model has to implement IDraw interface for Cairo context.
	/// </summary>
	public class OffsetView : IDraw<Context>
	{
		private double m_offsetX = 0;
		private double m_offsetY = 0;
		
		private double m_scale;
		private IDraw<Context> m_model;
		
		public double OffsetX {
			get {
				return m_offsetX;
			}
			set {
				m_offsetX = value;
			}
		}
		
		public double OffsetY {
			get {
				return m_offsetY;
			}
			set {
				m_offsetY = value;
			}
		}
		
		public double Scale {
			get {
				return m_scale;
			}
			set {
				m_scale = value;
			}
		}
		
		public OffsetView(IDraw<Context> model, double scale)
		{
			m_model = model;
			m_scale = scale;
		}
		
		public void Draw(Context context) {
			context.Save();
			context.Translate(m_offsetX, m_offsetY);
			context.Scale(m_scale, m_scale);
			m_model.Draw(context);
			context.Restore();
		}
	}
	
	public class PixelPathView : IDraw<Context>
	{
		public int Width;
		public List<int> Indices;
		public Color Color;
		
		public PixelPathView(List<int> indices, int width)
		{
			Indices = indices;
			Width = width;
			Color = new Color(0, 1, 0, 1);
		}
		
		public void Draw(Context context) {
			context.Color = Color;
			for (int i = 0; i < Indices.Count; i++) {
				int p = Indices[i];
				int x = p % Width;
				int y = p / Width;
				context.NewPath();
				context.Rectangle(x, y, 1, 1);
				context.Fill();
			}
		}
	}
	
	/// <summary>
	/// Draws cairo image to context.
	/// </summary>
	public class ImageView : IDraw<Context>
	{
		private ImageSurface m_image;
		
		public ImageView(ImageSurface image)
		{
			m_image = image;
		}
		
		public void Draw(Context context) {
			m_image.Show(context, 0, 0);
		}
	}
}

