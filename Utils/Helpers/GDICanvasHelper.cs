/*
GDICanvasHelper - A helper for handling mouse events and centered rendering.
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
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Utils
{
	/// <summary>
	/// Makes it easy to render cairo graphics and handle mouse actions on a widget.
	/// Renders everything outside the view area as black.
	/// </summary>
	public class GDICanvasHelper
	{
		private Control m_control;
		private double m_settingsWidth;
		private double m_settingsHeight;
		private MouseToolAction[] m_mouseToolActions;
		private MouseToolAction m_currentMouseToolAction;
		private MouseToolAction.MouseButton m_mouseState;
		private MouseToolAction.ModifierKey m_modifierState;
		private Matrix m_mouseDownViewTransform = null;
		private PointD m_mouseDownViewPosition;
		private PointD m_mouseDownControlPosition;
		
		public delegate void RenderDelegate (Graphics context);
		
		public PointD MouseDownViewPosition {
			get {
				return m_mouseDownViewPosition;
			}
		}
		
		public PointD MouseDownControlPosition {
			get {
				return m_mouseDownControlPosition;
			}
		}
		
		public GDICanvasHelper()
		{
		}
		
		private Rectangle CurrentControlBounds {
			get {
				return new Rectangle (0, 
				                      0, 
				                      m_control.ClientRectangle.Width, 
				                      m_control.ClientRectangle.Height);
			}
		}
		
		private RectangleD CurrentView {
			get {
				var controlBounds = this.CurrentControlBounds;
				var view = GDICanvasViewModule.ViewRectangle (controlBounds, 
				                                              m_settingsWidth, 
				                                              m_settingsHeight);
				return view;
			}
		}
		
		public Matrix CurrentViewToControlMatrix {
			get {
				var viewTransform = GDICanvasViewModule.ViewToControlMatrix (this.CurrentControlBounds, 
				                                                             this.CurrentView,
				                                                             m_settingsWidth,
				                                                             m_settingsHeight);
				return viewTransform;
			}
		}
		
		public Matrix CurrentViewToBufferMatrix {
			get {
				var viewTransform = GDICanvasViewModule.ViewToBufferMatrix (this.CurrentControlBounds, 
				                                                            this.CurrentView,
				                                                            m_settingsWidth,
				                                                            m_settingsHeight);
				return viewTransform;
			}
		}
		
		private bool m_shiftDown = false;
		private bool m_ctrlDown = false;
		
		public void Step1_SetControl (Control control) {
			
			m_control = control;
			
			var form = m_control.FindForm();
			form.KeyDown += delegate(object sender, KeyEventArgs e) { 
				if (e.KeyCode == Keys.ShiftKey)
				{
					m_shiftDown = true;
				}
				if (e.KeyCode == Keys.ControlKey)
				{
					m_ctrlDown = true;
				}
			};
			form.KeyUp += delegate(object sender, KeyEventArgs e) { 
				if (e.KeyCode == Keys.ShiftKey)
				{
					m_shiftDown = false;
				}
				if (e.KeyCode == Keys.Control)
				{
					m_ctrlDown = false;
				}
			};
			
			
			m_control.MouseDown += delegate(object o, MouseEventArgs args) {
				// Change keyboard modifier state.
				var shift = m_shiftDown;
				var ctrl = m_ctrlDown;
				if (shift) {
					m_modifierState |= MouseToolAction.ModifierKey.Shift;
				}
				if (ctrl) {
					m_modifierState |= MouseToolAction.ModifierKey.Control;
				}
				
				// Change mouse button state.
				var button = args.Button;
				switch (button) {
					case MouseButtons.Left: m_mouseState |= MouseToolAction.MouseButton.Left; break;
					case MouseButtons.Middle: m_mouseState |= MouseToolAction.MouseButton.Middle; break;
					case MouseButtons.Right: m_mouseState |= MouseToolAction.MouseButton.Right; break;
				}
				
				var viewTransform = this.CurrentViewToControlMatrix;
				m_mouseDownViewTransform = viewTransform;
				
				// Set mouse down locations.
				float px = args.X;
				float py = args.Y;
				this.m_mouseDownControlPosition = new PointD (px, py);
				var inv = viewTransform.Clone () as Matrix;
				inv.Invert ();
				var ps = new PointF[]{new PointF(px, py)};
				inv.TransformPoints(ps);
				px = ps[0].X;
				py = ps[0].Y;
				this.m_mouseDownViewPosition = new PointD (px, py);
				
				// If pressing a mouse button, complete the previous mouse tool action.
				if (this.m_currentMouseToolAction != null) {
					if (this.m_currentMouseToolAction.MouseUp != null) {
						this.m_currentMouseToolAction.MouseUp (px, py);
					}
				}
				
				// Find the mouse tool action that matches the current state.
				m_currentMouseToolAction = null;
				foreach (var mouseToolAction in m_mouseToolActions) {
					if (mouseToolAction.Button == m_mouseState &&
					    mouseToolAction.Modifier == m_modifierState) {
						this.m_currentMouseToolAction = mouseToolAction;
						if (this.m_currentMouseToolAction.MouseDown != null) {
							this.m_currentMouseToolAction.MouseDown (px, py);
						}
						
						return;
					}
				}
			};
			m_control.MouseUp += delegate(object o, MouseEventArgs args) {
				// Change keyboard modifier state.
				var shift = m_shiftDown;
				var ctrl = m_ctrlDown;
				if (shift) {
					m_modifierState &= ~MouseToolAction.ModifierKey.Shift;
				}
				if (ctrl) {
					m_modifierState &= ~MouseToolAction.ModifierKey.Control;
				}
				
				// Change mouse button state.
				var button = args.Button;
				switch (button) {
					case MouseButtons.Left: m_mouseState &= ~MouseToolAction.MouseButton.Left; break;
					case MouseButtons.Middle: m_mouseState &= ~MouseToolAction.MouseButton.Middle; break;
					case MouseButtons.Right: m_mouseState &= ~MouseToolAction.MouseButton.Right; break;
				}
				
				m_mouseDownViewTransform = this.CurrentViewToControlMatrix;
				
				float px = args.X;
				float py = args.Y;
				var inv = this.m_mouseDownViewTransform.Clone () as Matrix;
				inv.Invert ();
				var ps = new PointF[]{new PointF(px, py)};
				inv.TransformPoints(ps);
				px = ps[0].X;
				py = ps[0].Y;
				
				if (this.m_currentMouseToolAction != null) {
					if (this.m_currentMouseToolAction.MouseUp != null) {
						this.m_currentMouseToolAction.MouseUp (px, py);
					}
				}
				
				this.m_currentMouseToolAction = null;
			};
			m_control.MouseMove += delegate(object o, MouseEventArgs args) {
				if (ReferenceEquals (this.m_mouseDownViewTransform, null)) {return;}
				
				float px = args.X;
				float py = args.Y;
				var inv = this.m_mouseDownViewTransform.Clone () as Matrix;
				inv.Invert ();
				var ps = new PointF[]{new PointF(px, py)};
				inv.TransformPoints (ps);
				px = ps[0].X;
				py = ps[0].Y;
				if (this.m_currentMouseToolAction != null) {
					if (this.m_currentMouseToolAction.MouseMove != null) {
						this.m_currentMouseToolAction.MouseMove (px, py);
					}
				}
			};
			
		}
		
		public void Step2_SetTargetResolution (double targetResolutionWidth,
		                                       double targetResolutionHeight) {
			m_settingsWidth = targetResolutionWidth;
			m_settingsHeight = targetResolutionHeight;
		}
		
		public void Step3_SetMouseToolActions (MouseToolAction[] mouseToolActions) {
			m_mouseToolActions = mouseToolActions;
		}
	}
}

