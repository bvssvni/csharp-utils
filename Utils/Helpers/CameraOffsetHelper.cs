using System;

namespace Utils
{
	/// <summary>
	/// Camera offset helper.
	/// 
	/// To use this, enable mouse pointer events + button press + button release.
	/// 
	/// The offset x and y is get and set through properties.
	/// </summary>
	public class CameraOffsetHelper
	{
		private double m_offsetX = 0;
		private double m_offsetY = 0;
		private bool m_enabled = false;
		private bool m_mouseDown = false;
		private double m_mouseDownX = 0;
		private double m_mouseDownY = 0;
		private double m_offsetDownX = 0;
		private double m_offsetDownY = 0;
		private bool m_enablePopupMenu = false;
		
		private event EventHandler<CameraOffsetChangedEventArgs> m_cameraOffsetChanged;
		
		public class CameraOffsetChangedEventArgs : EventArgs {
			private double m_oldCameraOffsetX;
			private double m_oldCameraOffsetY;
			private double m_newCameraOffsetX;
			private double m_newCameraOffsetY;
			
			public double OldCameraOffsetX {
				get {
					return m_oldCameraOffsetX;
				}
			}
			
			public double OldCameraOffsetY {
				get {
					return m_oldCameraOffsetY;
				}
			}
			
			public double NewCameraOffsetX {
				get {
					return m_newCameraOffsetX;
				}
			}
			
			public double NewCameraOffsetY {
				get {
					return m_newCameraOffsetY;
				}
			}
			
			public CameraOffsetChangedEventArgs(double oldX, double oldY, double newX, double newY) {
				m_oldCameraOffsetX = oldX;
				m_oldCameraOffsetY = oldY;
				m_newCameraOffsetX = newX;
				m_newCameraOffsetY = newY;
			}
		}
		
		public double OffsetX
		{
			get {
				return m_offsetX;
			}
		}
		
		public double OffsetY
		{
			get {
				return m_offsetY;
			}
		}
		
		public bool Enabled
		{
			get {
				return m_enabled;
			}
		}
		
		public CameraOffsetHelper()
		{
		}
		
		private void SetCameraOffset(double x, double y) {
			double oldX = m_offsetX;
			double oldY = m_offsetY;
			
			m_offsetX = x;
			m_offsetY = y;
			
			var args = new CameraOffsetChangedEventArgs(oldX, oldY, x, y);
			if (m_cameraOffsetChanged != null) m_cameraOffsetChanged(this, args);
		}
		
		public void Step1_SetWidget(Gtk.Widget widget) {
			// Add right-click context menu to control.
			var menu = new Gtk.Menu();
			var resetMenuItem = new Gtk.MenuItem("Reset Camera Offset");
			resetMenuItem.Activated += delegate(object sender, EventArgs e) {
				SetCameraOffset(0, 0);
			};
			menu.Add(resetMenuItem);
			
			// Handle mouse move events.
			widget.MotionNotifyEvent += delegate(object o, Gtk.MotionNotifyEventArgs args) {
				if (!m_enabled) return;
				if (!m_mouseDown) return;
				
				double x = args.Event.X;
				double y = args.Event.Y;
				double dx = x - m_mouseDownX;
				double dy = y - m_mouseDownY;
				
				SetCameraOffset(m_offsetDownX + dx, m_offsetDownY + dy);
			};
			widget.ButtonPressEvent += delegate(object o, Gtk.ButtonPressEventArgs args) {
				if (m_enablePopupMenu && (args.Event.Button & 2) == 2) {
					menu.ShowAll();
					menu.Popup();
					return;
				}
				
				m_mouseDown = true;
				m_mouseDownX = args.Event.X;
				m_mouseDownY = args.Event.Y;
				m_offsetDownX = m_offsetX;
				m_offsetDownY = m_offsetY;
			};
			widget.ButtonReleaseEvent += delegate(object o, Gtk.ButtonReleaseEventArgs args) {
				m_mouseDown = false;
			};
		}
		
		public void Step2_SetCameraOffsetChanged(EventHandler<CameraOffsetChangedEventArgs> handler) {
			m_cameraOffsetChanged += handler;
		}
		
		public void Step3_SetEnablePopupMenu(bool val) {
			m_enablePopupMenu = val;
		}
		
		public void Step4_SetEnabled(bool val) {
			m_enabled = !m_enabled;
		}
		
	}
}

