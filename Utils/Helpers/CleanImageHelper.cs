using System;
using Gdk;
using Utils.SpriteSheetAnalyzer;

namespace Utils
{
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
			foreach (var a in islands) {
				byte alpha = CleanImage.FindMaxAlpha(m_image, a);
				if (alpha >= maxAlpha)  continue;

				CleanImage.Erase(img, a);
			}
		}
	}
}

