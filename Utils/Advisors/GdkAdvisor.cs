using System;

namespace Utils
{
	public class GdkAdvisor
	{
		public static bool ShouldNotDraw(Gdk.Window window) {
			return window.FrameExtents.Width <= 0
				|| window.FrameExtents.Height <= 0;
		}
	}
}

