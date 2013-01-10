using System;
using Gdk;

namespace Utils
{
	/// <summary>
	/// Used to pass ways of drawing to Gdk.
	/// </summary>
	public class DrawingArgs
	{
		public Gdk.Window Window;
		public Gdk.GC GC;
		public Rectangle Rectangle;

		public DrawingArgs (EventExpose evt)
		{
			this.Window = evt.Window;
			this.GC = new Gdk.GC (this.Window);
			this.Rectangle = new Rectangle(0, 0, this.Window.FrameExtents.Width, this.Window.FrameExtents.Height);
		}
	}
}

