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

	public static class Interpolation
	{
		public delegate object Interpolate2Delegate (object a0, object a1, double time);
		public delegate object Interpolate4Delegate (object a0, object a1, object a2, object a3, double time);

		public static Interpolate2Delegate Linear (Type type)
		{
			throw new NotImplementedException ();
		}

		public static Interpolate4Delegate Catmull (Type type)
		{
			throw new NotImplementedException ();
		}
	}

	public static class HitCheck
	{
		public delegate bool HitDelegate (object a, Point pos);

		public static HitDelegate Detector (Type type)
		{
			throw new NotImplementedException ();
		}
	}

	public static class Rendering
	{
		public delegate void DrawDelegate (Cairo.Context context);

		public static DrawDelegate DrawingRoutine (Type type)
		{
			throw new NotImplementedException ();
		}
	}

	public class Brush
	{
		public void Draw (Cairo.Context context, ShapeBase shape) {

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

	public abstract class ShapeBase
	{
		public string Name;
		public bool Visible;
	}

	public class ShapeTree : IEnumerable<ShapeBase>
	{
		public ShapeBase Shape;
		public List<ShapeTree> Children;

		public ShapeTree (ShapeBase shape)
		{
			this.Shape = shape;
			Children = new List<ShapeTree> ();
		}

		public ShapeTree AddChild (ShapeBase shape)
		{
			Children.Add (new ShapeTree (shape));
			return Children [Children.Count - 1];
		}

		public IEnumerator<ShapeBase> GetEnumerator()
		{
			foreach (var child in Children) {
				yield return child.Shape;
				foreach (var item in child) {
					yield return item;
				}
			}
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return (System.Collections.IEnumerator)GetEnumerator ();
		}
	}

	public class EllipseShape : ShapeBase
	{
		public Brush Brush;
		public Pen Border;
		public Rectangle Rectangle;
	}

	public class RectangleShape : ShapeBase
	{
		public Look Look;
		public Rectangle Rectangle;
	}

	public class LineShape : ShapeBase
	{
		public Pen Border;
		public Line Line;
	}

	public abstract class PolygonShapeBase : ShapeBase
	{
		public Look Look;
		public List<Point> Points;
	}

	public class CatmullCurveShape : PolygonShapeBase
	{

	}

	public class QuadraticBezierShape : PolygonShapeBase
	{

	}

	public class DeformShape : ShapeBase
	{

	}
}

