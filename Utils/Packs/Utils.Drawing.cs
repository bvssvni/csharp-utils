using System;
using System.Collections.Generic;

namespace Utils.Drawing
{
	public struct Rectangle
	{
		public double X;
		public double Y;
		public double Width;
		public double Height;
	}

	public struct ABRectangle
	{
		public double X1;
		public double Y1;
		public double X2;
		public double Y2;
	}

	public struct Point
	{
		public double X;
		public double Y;
	}

	public struct Line
	{
		public Point StartPoint;
		public Point EndPoint;
	}

	public interface IHit<T>
	{
		bool Hit (T pos);
	}

	public class Brush
	{
		public void Draw (Cairo.Context context, Shape shape) {

		}
	}

	public class Pen
	{

	}

	public struct Look
	{
		public Brush Fill;
		public Pen Border;
	}

	public struct Color
	{
		public double R;
		public double G;
		public double B;
		public double A;
	}

	public class SolidPen : Pen
	{
		public Color Color;
		public double Width;
	}

	public class SolidBrush : Brush
	{
		public Color Color;
	}

	public abstract class Shape : IHit<Point>
	{
		public string Name;
		public bool Visible;

		public abstract bool Hit(Point pos);
	}

	public class ShapeTree
	{
		public Shape Shape;
		public List<ShapeTree> Children;
	}

	public class EllipseShape : Shape
	{
		public Brush Brush;
		public Pen Border;
		public Rectangle Rectangle;

		public override bool Hit (Point pos) {
			throw new NotImplementedException ();
		}
	}

	public class RectangleShape : Shape
	{
		public Look Look;
		public Rectangle Rectangle;

		public override bool Hit (Point pos) {
			throw new NotImplementedException ();
		}
	}

	public class LineShape : Shape
	{
		public Pen Border;
		public Line Line;

		public override bool Hit (Point pos) {
			throw new NotImplementedException ();
		}
	}

	public abstract class PolygonShape : Shape
	{
		public Look Look;
		public List<Point> Points;
	}

	public class CatmullCurveShape : PolygonShape
	{
		public override bool Hit(Point pos)
		{
			throw new System.NotImplementedException();
		}
	}

	public class QuadraticBezierShape : PolygonShape
	{
		public override bool Hit(Point pos)
		{
			throw new System.NotImplementedException();
		}
	}
}

