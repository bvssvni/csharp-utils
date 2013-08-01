/*
CairoCanvasViewModule - Methods for "blacking out" areas outside view.
BSD license.  
by Sven Nilsen, 2013
http://www.cutoutpro.com  
Version: 0.001 in angular degrees version notation  
http://isprogrammingeasy.blogspot.no/2012/08/angular-degrees-versioning-notation.html  

0.001 - Added 'ViewToBufferMatrix'.

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
using System.Drawing.Drawing2D;

namespace Utils
{
	public static class GDICanvasViewModule
	{
		/// <summary>
		/// Computes the view rectangle centered at control bounds by width and height settings.
		/// Assumes controlBounds starts at (0, 0) in upper left corner.
		/// </summary>
		/// <returns>The view rectangle.</returns>
		/// <param name="controlBounds">Control bounds.</param>
		/// <param name="settingsWidth">Settings width.</param>
		/// <param name="settingsHeight">Settings height.</param>
		public static RectangleD ViewRectangle (Rectangle controlBounds,
		                                        double settingsWidth, double settingsHeight) {
			double cx = controlBounds.X;
			double cy = controlBounds.Y;
			double cw = controlBounds.Width;
			double ch = controlBounds.Height;
			double rControl = ch / cw;
			double rSettings = settingsHeight / settingsWidth;
			double wHalf = 0.5 * cw;
			double hHalf = 0.5 * ch;
			double xCenter = cx + wHalf;
			double yCenter =cy + hHalf;
			if (rControl >= rSettings) {
				// The control is taller than settings ratio.
				double hView = cw * rSettings;
				double hHalfView = 0.5 * hView;
				return new RectangleD (0, yCenter - hHalfView, cw, hView);
			} else {
				// The control is wider than settings ratio.
				double wView = ch / rSettings;
				double wHalfView = 0.5 * wView;
				return new RectangleD (xCenter - wHalfView, 0, wView, ch);
			}
		}
		
		/// <summary>
		/// Draws black in the areas outside the view.
		/// Assumes that controlBounds is starting at (0, 0) in upper left corner.
		/// </summary>
		/// <param name="context">A context to perform drawing.</param>
		/// <param name="controlBounds">Bounds of the control.</param>
		/// <param name="view">The bounds of the view.</param>
		public static void Draw (Graphics context, Rectangle controlBounds, RectangleD view) {
			context.Clear(Color.Black);
			context.FillRectangle(Brushes.White, (float)view.X, (float)view.Y, (float)view.Width, (float)view.Height);
		}
		
		public static Matrix ViewToControlMatrix (Rectangle controlBounds,
		                                          RectangleD view,
		                                          double settingsWidth,
		                                          double settingsHeight) {
			
			var m = new Matrix ();
			m.Translate ((float)(view.X - controlBounds.X), (float)(view.Y - controlBounds.Y));
			m.Scale ((float)(view.Width / settingsWidth), (float)(view.Height / settingsHeight));
			return m;
		}
		
		public static Matrix ViewToBufferMatrix (Rectangle controlBounds,
		                                         RectangleD view,
		                                         double settingsWidth,
		                                         double settingsHeight) {
			
			var m = new Matrix ();
			m.Scale ((float)(view.Width / settingsWidth), (float)(view.Height / settingsHeight));
			return m;
		}
	}
}

