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

	public enum Component
	{
		Visibility = 1
	}

	public interface HitCheck<T>
	{
		bool Hit (T pos);
	}

	public class Brush
	{

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

	public class ComponentBase
	{

	}

	public class Shape
	{
		public string Name;
		public Dictionary<Component, ComponentBase> Components;
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
	}

	public class RectangleShape : Shape
	{
		public Brush Brush;
		public Pen Border;
		public Rectangle Rectangle;
	}

	public class LineShape : Shape
	{
		public Pen Border;
		public Line Line;
	}

	public class PolygonShape : Shape
	{
		public Brush Brush;
		public Pen Border;
		public List<Point> Points;
	}

	public class CatmullCurveShape : PolygonShape
	{

	}

	public class QuadraticBezierShape : PolygonShape
	{

	}
}

