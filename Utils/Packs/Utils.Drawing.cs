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

	public interface IMultiply<T>
	{
		T Multiply (T b);
	}

	public interface IRotate<T, G>
	{
		T Rotate (G angle);
	}

	public interface ITranslate<T, G>
	{
		T Translate (G pos);
	}

	public interface IScale<T, G>
	{
		T Scale (G info);
	}

	public interface IShear<T, G>
	{
		T Shear (G info);
	}

	public struct Matrix : 
		IMultiply<Matrix>,
		IRotate<Matrix, double>,
		ITranslate<Matrix, Point>,
		IScale<Matrix, Point>,
		IScale<Matrix, double>,
		IShear<Matrix, Point>
	{
		public double M11;
		public double M12;
		public double M13;
		public double M21;
		public double M22;
		public double M23;
		public double M31;
		public double M32;
		public double M33;

		public static Matrix Identity () {
			throw new NotImplementedException ();
		}

		public Matrix Multiply (Matrix b) {
			throw new NotImplementedException ();
		}

		public Matrix Rotate (double angleInRadians) {
			throw new NotImplementedException ();
		}

		public Matrix Translate (Point pos) {
			throw new NotImplementedException ();
		}

		public Matrix Scale (Point pos) {
			throw new NotImplementedException ();
		}

		public Matrix Scale (double scale) {
			throw new NotImplementedException ();
		}

		public Matrix Shear (Point pos) {
			throw new NotImplementedException ();
		}
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

	public static class Interpolation
	{
		public delegate object Interpolate2 (object a0, object a1, double time);
		public delegate object Interpolate4 (object a0, object a1, object a2, object a3, double time);

		public static Interpolate2 Linear (Type type)
		{
			throw new NotImplementedException ();
		}

		public static Interpolate4 Catmull (Type type)
		{
			throw new NotImplementedException ();
		}
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

	public class DeformShape : Shape
	{
		public override bool Hit(Point pos)
		{
			throw new System.NotImplementedException();
		}
	}
}

