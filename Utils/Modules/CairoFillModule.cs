using System;

namespace Utils
{
	public class CairoFillModule
	{
		public static void Fill(Cairo.Context context, Gtk.Widget widget, Cairo.Color color) {
			context.NewPath();
			context.Color = color;
			context.Rectangle(0, 0, widget.Allocation.Width, widget.Allocation.Height);
			context.Fill();
		}
		
		public static void Fill(Cairo.Context context, int width, int height, Cairo.Color color) {
			context.NewPath();
			context.Color = color;
			context.Rectangle(0, 0, width, height);
			context.Fill();
		}
	}
}

