/*
Cairo CanvasHelper - Makes it easy to render cairo graphics and handle mouse actions.
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
using Gtk;
using Gdk;

namespace Utils
{
	/// <summary>
	/// Makes it easy to render cairo graphics and handle mouse actions on a widget.
	/// Renders everything outside the view area as black.
	/// </summary>
	public class CairoCanvasHelper
	{
		private Widget m_control;
		private RenderDelegate m_renderDelegate;
		private double m_settingsWidth;
		private double m_settingsHeight;
		private MouseToolAction[] m_mouseToolActions;
		private MouseToolAction m_currentMouseToolAction;
		private MouseToolAction.MouseButton m_mouseState;
		private MouseToolAction.ModifierKey m_modifierState;
		private Matrix m_mouseDownViewTransform = null;
		private Cairo.PointD m_mouseDownViewPosition;
		private Cairo.PointD m_mouseDownControlPosition;
		
		public delegate void RenderDelegate (Context context);
		
		public Cairo.PointD MouseDownViewPosition {
			get {
				return m_mouseDownViewPosition;
			}
		}
		
		public Cairo.PointD MouseDownControlPosition {
			get {
				return m_mouseDownControlPosition;
			}
		}
		
		public CairoCanvasHelper()
		{
		}
		
		private Cairo.Rectangle CurrentControlBounds {
			get {
				return new Cairo.Rectangle (0.0, 
				                            0.0, 
				                            m_control.Allocation.Width, 
				                            m_control.Allocation.Height);
			}
		}
		
		private Cairo.Rectangle CurrentView {
			get {
				var controlBounds = this.CurrentControlBounds;
				var view = CairoCanvasViewModule.ViewRectangle (controlBounds, 
				                                                m_settingsWidth, 
				                                                m_settingsHeight);
				return view;
			}
		}
		
		private Matrix CurrentViewTransform {
			get {
				var viewTransform = CairoCanvasViewModule.ViewToControlMatrix (this.CurrentControlBounds, 
				                                                               this.CurrentView,
				                                                               m_settingsWidth,
				                                                               m_settingsHeight);
				return viewTransform;
			}
		}
		
		public void Step1_SetControl (Widget control) {
			
			m_control = control;
			m_control.ExposeEvent += (o, args) => {
				using (var context = Gdk.CairoHelper.Create (args.Event.Window)) {
					CairoFillModule.Fill(context, m_control, new Cairo.Color(1, 1, 1));
					context.Antialias = Cairo.Antialias.Subpixel;
					context.Save ();
					context.Transform (this.CurrentViewTransform);
					if (m_renderDelegate != null) {
						m_renderDelegate (context);
					}
					context.Restore ();
					CairoCanvasViewModule.Draw (context, 
					                            this.CurrentControlBounds,
					                            this.CurrentView);
				}
			};
			
			m_control.ButtonPressEvent += delegate(object o, Gtk.ButtonPressEventArgs args) {
				// Change keyboard modifier state.
				var shift = (args.Event.State & ModifierType.ShiftMask) == ModifierType.ShiftMask;
				var ctrl = (args.Event.State & ModifierType.ControlMask) == ModifierType.ControlMask;
				if (shift) {
					m_modifierState |= MouseToolAction.ModifierKey.Shift;
				}
				if (ctrl) {
					m_modifierState |= MouseToolAction.ModifierKey.Control;
				}
				
				// Change mouse button state.
				var button = args.Event.Button;
				switch (button) {
					case 1: m_mouseState |= MouseToolAction.MouseButton.Left; break;
					case 2: m_mouseState |= MouseToolAction.MouseButton.Middle; break;
					case 3: m_mouseState |= MouseToolAction.MouseButton.Right; break;
				}
				
				var viewTransform = this.CurrentViewTransform;
				m_mouseDownViewTransform = viewTransform;
				
				// Set mouse down locations.
				var px = args.Event.X;
				var py = args.Event.Y;
				this.m_mouseDownControlPosition = new Cairo.PointD (px, py);
				var inv = viewTransform.Clone () as Matrix;
				inv.Invert ();
				inv.TransformPoint (ref px, ref py);
				this.m_mouseDownViewPosition = new Cairo.PointD (px, py);
				
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
			m_control.ButtonReleaseEvent += delegate(object o, Gtk.ButtonReleaseEventArgs args) {
				// Change keyboard modifier state.
				var shift = (args.Event.State & ModifierType.ShiftMask) == ModifierType.ShiftMask;
				var ctrl = (args.Event.State & ModifierType.ControlMask) == ModifierType.ControlMask;
				if (shift) {
					m_modifierState &= ~MouseToolAction.ModifierKey.Shift;
				}
				if (ctrl) {
					m_modifierState &= ~MouseToolAction.ModifierKey.Control;
				}
				
				// Change mouse button state.
				var button = args.Event.Button;
				switch (button) {
					case 1: m_mouseState &= ~MouseToolAction.MouseButton.Left; break;
					case 2: m_mouseState &= ~MouseToolAction.MouseButton.Middle; break;
					case 3: m_mouseState &= ~MouseToolAction.MouseButton.Right; break;
				}
				
				var px = args.Event.X;
				var py = args.Event.Y;
				var inv = this.m_mouseDownViewTransform.Clone () as Matrix;
				inv.Invert ();
				inv.TransformPoint (ref px, ref py);
				
				if (this.m_currentMouseToolAction != null) {
					if (this.m_currentMouseToolAction.MouseUp != null) {
						this.m_currentMouseToolAction.MouseUp (px, py);
					}
				}
				
				this.m_currentMouseToolAction = null;
			};
			m_control.MotionNotifyEvent += delegate(object o, Gtk.MotionNotifyEventArgs args) {
				if (ReferenceEquals (this.m_mouseDownViewTransform, null)) {return;}
				
				var px = args.Event.X;
				var py = args.Event.Y;
				var inv = this.m_mouseDownViewTransform.Clone () as Matrix;
				inv.Invert ();
				inv.TransformPoint (ref px, ref py);
				if (this.m_currentMouseToolAction != null) {
					if (this.m_currentMouseToolAction.MouseMove != null) {
						this.m_currentMouseToolAction.MouseMove (px, py);
					}
				}
			};
			
			m_control.AddEvents ((int)(
				EventMask.ButtonPressMask 
				| EventMask.ButtonReleaseMask 
				| EventMask.PointerMotionMask));
		}
		
		public void Step2_SetTargetResolution (double targetResolutionWidth,
		                                       double targetResolutionHeight) {
			m_settingsWidth = targetResolutionWidth;
			m_settingsHeight = targetResolutionHeight;
		}
		
		public void Step3_SetRenderDelegate (RenderDelegate renderDelegate) {
			m_renderDelegate = renderDelegate;
		}
		
		public void Step4_SetMouseToolActions (MouseToolAction[] mouseToolActions) {
			m_mouseToolActions = mouseToolActions;
		}
	}
}

