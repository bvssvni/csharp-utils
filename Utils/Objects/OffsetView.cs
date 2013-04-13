using System;
using Utils.Document;

namespace Utils
{
	/// <summary>
	/// Offset view.
	/// 
	/// Scales and translates view correctly.
	/// The model has to implement IDraw interface for Cairo context.
	/// </summary>
	public class OffsetView : IDraw<Cairo.Context>
	{
		private double m_offsetX = 0;
		private double m_offsetY = 0;
		
		private double m_scale;
		private IDraw<Cairo.Context> m_model;
		
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
		
		public OffsetView(IDraw<Cairo.Context> model, double scale)
		{
			m_model = model;
			m_scale = scale;
		}
		
		public void Draw(Cairo.Context context) {
			context.Save();
			context.Translate(m_offsetX, m_offsetY);
			context.Scale(m_scale, m_scale);
			m_model.Draw(context);
			context.Restore();
		}
	}
}

