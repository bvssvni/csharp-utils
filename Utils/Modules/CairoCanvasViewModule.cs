/*
CairoCanvasViewModule - Methods for "blacking out" areas outside view.
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
using Cairo;

namespace Utils
{
	public static class CairoCanvasViewModule
	{
		/// <summary>
		/// Computes the view rectangle centered at control bounds by width and height settings.
		/// Assumes controlBounds starts at (0, 0) in upper left corner.
		/// </summary>
		/// <returns>The view rectangle.</returns>
		/// <param name="controlBounds">Control bounds.</param>
		/// <param name="settingsWidth">Settings width.</param>
		/// <param name="settingsHeight">Settings height.</param>
		public static Rectangle ViewRectangle (Rectangle controlBounds,
		                                       double settingsWidth, double settingsHeight) {
			double rControl = controlBounds.Height / controlBounds.Width;
			double rSettings = settingsHeight / settingsWidth;
			double wHalf = 0.5 * controlBounds.Width;
			double hHalf = 0.5 * controlBounds.Height;
			double xCenter = controlBounds.X + wHalf;
			double yCenter = controlBounds.Y + hHalf;
			if (rControl >= rSettings) {
				// The control is taller than settings ratio.
				double hView = controlBounds.Width * rSettings;
				double hHalfView = 0.5 * hView;
				return new Rectangle (0, yCenter - hHalfView, controlBounds.Width, hView);
			} else {
				// The control is wider than settings ratio.
				double wView = controlBounds.Height / rSettings;
				double wHalfView = 0.5 * wView;
				return new Rectangle (xCenter - wHalfView, 0, wView, controlBounds.Height);
			}
		}
		
		/// <summary>
		/// Draws black in the areas outside the view.
		/// Assumes that controlBounds is starting at (0, 0) in upper left corner.
		/// </summary>
		/// <param name="context">A context to perform drawing.</param>
		/// <param name="controlBounds">Bounds of the control.</param>
		/// <param name="view">The bounds of the view.</param>
		public static void Draw (Context context, Rectangle controlBounds, Rectangle view) {
			context.Color = new Cairo.Color (0.0, 0.0, 0.0);
			context.NewPath ();
			context.Rectangle (0.0, 0.0, view.X, controlBounds.Height);
			var wRight = controlBounds.Width - view.X - view.Width;
			context.Rectangle (view.X + view.Width, 0.0, wRight, controlBounds.Height);
			context.Rectangle (view.X, 0.0, view.Width, view.Y);
			var hBottom = controlBounds.Height - view.Y - view.Height;
			context.Rectangle (view.X, view.Y + view.Height, view.Width, hBottom);
			context.Fill ();
		}
	}
}

