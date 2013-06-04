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

	public struct RectangleList
	{
		public List<Rectangle> Items;
	}

	public struct ABRectangle
	{
		public double X1;
		public double Y1;
		public double X2;
		public double Y2;

		public static ABRectangle FromRectangle (Rectangle rect) {
			throw new NotImplementedException ();
		}
	}

	public struct Point
	{
		public double X;
		public double Y;
	}

	public struct PointList
	{
		public List<Point> Items;
	}

	public struct Point3
	{
		public double X;
		public double Y;
		public double Z;
	}

	public struct Point3List
	{
		public List<Point3> Items;
	}

	public struct Point4
	{
		public double X;
		public double Y;
		public double Z;
		public double W;
	}

	public struct Point4List
	{
		public List<Point4> Items;
	}

	public struct Quaternion
	{
		public double X;
		public double Y;
		public double Z;
		public double W;
	}

	public struct QuaternionList
	{
		public List<Quaternion> Items;
	}

	public struct Wheel
	{
		public Point Position;
		public double Angle;
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

	public struct LineList
	{
		public List<Line> Items;
	}

	public static class InterpolationModule
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

		public static Interpolate4Delegate CubicBezier (Type type)
		{
			throw new NotImplementedException ();
		}
	}

	public static class ShapeFactoryModule
	{
		public static ShapeBase Default (Type type)
		{
			throw new NotImplementedException ();
		}

		public static object Zero (Type type)
		{
			// TODO: Create zero default values,
			// used for tangents in key frames.
			throw new NotImplementedException ();
		}
	}

	public static class HitCheckModule
	{
		public delegate bool HitDelegate (object a, Point pos);

		public static HitDelegate Detector (Type type)
		{
			throw new NotImplementedException ();
		}
	}

	/// <summary>
	/// Tree permission module.
	/// 
	/// Tells whether nodes of a type can be added to a parent node.
	/// This can be used to restrict 'Animation' to 'Action' nodes only.
	/// </summary>
	public static class TreePermissionModule
	{
		public static bool AllowChildNodeType (ShapeTree parent, Type type)
		{
			throw new NotImplementedException ();
		}
	}

	/// <summary>
	/// Rendering module.
	/// 
	/// Contains the logic of rendering.
	/// Directs the drawing routines per shape type.
	/// </summary>
	public static class RenderingModule
	{
		public delegate void DrawDelegate (Cairo.Context context, ShapeTree tree);

		public static void FillRectangle (Cairo.Context context, ShapeTree tree) {
			var shape = tree.Shape as RectangleShape;
			if (tree.Look.Fill is SolidBrush) {
				var solidBrush = (SolidBrush)tree.Look.Fill;
				if (solidBrush.Color.A > 0.0) {
					var c = solidBrush.Color;
					var r = shape.Rectangle;
					context.Color = new Cairo.Color (c.R, c.G, c.B, c.A);
					context.NewPath ();
					context.Rectangle (r.X, r.Y, r.Width, r.Height);
					context.Fill ();
				}
			}
		}

		public static void BorderRectangle (Cairo.Context context, ShapeTree tree) {
			var shape = tree.Shape as RectangleShape;
			if (tree.Look.Border is SolidPen) {
				var solidPen = (SolidPen)tree.Look.Border;
				if (solidPen.Color.A > 0.0 && solidPen.Width > 0.0) {
					var c = solidPen.Color;
					var w = solidPen.Width;
					var r = shape.Rectangle;
					context.Color = new Cairo.Color (c.R, c.G, c.B, c.A);
					context.NewPath ();
					context.LineWidth = w;
					context.Rectangle (r.X, r.Y, r.Width, r.Height);
					context.Stroke ();
				}
			}
		}

		public static void FillEllipse (Cairo.Context context, ShapeTree tree) {
			var shape = tree.Shape as EllipseShape;
			if (tree.Look.Fill is SolidBrush) {
				var solidBrush = (SolidBrush)tree.Look.Fill;
				if (solidBrush.Color.A > 0.0) {
					var c = solidBrush.Color;
					var r = shape.Rectangle;
					context.Color = new Cairo.Color (c.R, c.G, c.B, c.A);
					context.NewPath ();
					context.Save ();
					
					var x = r.X;
					var y = r.Y;
					var xDis = 0.5 * r.Width;
					var yDis = 0.5 * r.Height;
					var kappa = 0.5522848; // 4 * ((√(2) - 1) / 3)
					var ox = xDis * kappa;  // control point offset horizontal
					var oy = yDis * kappa;  // control point offset vertical
					var xe = x + xDis;      // x-end
					var ye = y + yDis;      // y-end
					
					context.Translate (xDis, yDis);
					context.MoveTo(x - xDis, y);
					context.CurveTo(x - xDis, y - oy, x - ox, y - yDis, x, y - yDis);
					context.CurveTo(x + ox, y - yDis, xe, y - oy, xe, y);
					context.CurveTo(xe, y + oy, x + ox, ye, x, ye);
					context.CurveTo(x - ox, ye, x - xDis, y + oy, x - xDis, y);

					context.Fill ();
					context.Restore ();
				}
			}
		}

		public static void BorderEllipse (Cairo.Context context, ShapeTree tree) {
			var shape = tree.Shape as EllipseShape;
			if (tree.Look.Border is SolidPen) {
				var solidPen = (SolidPen)tree.Look.Border;
				if (solidPen.Color.A > 0.0) {
					var c = solidPen.Color;
					var r = shape.Rectangle;
					var w = solidPen.Width;
					context.Color = new Cairo.Color (c.R, c.G, c.B, c.A);
					context.NewPath ();
					context.Save ();
					context.LineWidth = w;

					var x = r.X;
					var y = r.Y;
					var xDis = 0.5 * r.Width;
					var yDis = 0.5 * r.Height;
					var kappa = 0.5522848; // 4 * ((√(2) - 1) / 3)
					var ox = xDis * kappa;  // control point offset horizontal
					var oy = yDis * kappa;  // control point offset vertical
					var xe = x + xDis;      // x-end
					var ye = y + yDis;      // y-end

					context.Translate (xDis, yDis);
					context.MoveTo(x - xDis, y);
					context.CurveTo(x - xDis, y - oy, x - ox, y - yDis, x, y - yDis);
					context.CurveTo(x + ox, y - yDis, xe, y - oy, xe, y);
					context.CurveTo(xe, y + oy, x + ox, ye, x, ye);
					context.CurveTo(x - ox, ye, x - xDis, y + oy, x - xDis, y);

					context.Stroke ();
					context.Restore ();
				}
			}
		}

		public static void FillLine (Cairo.Context context, ShapeTree tree) {
			// Do nothing.
		}

		public static void BorderLine (Cairo.Context context, ShapeTree tree) {
			var shape = tree.Shape as LineShape;
			if (tree.Look.Border is SolidPen) {
				var solidPen = (SolidPen)tree.Look.Border;
				if (solidPen.Color.A > 0.0 && solidPen.Width > 0.0) {
					var c = solidPen.Color;
					var w = solidPen.Width;
					var p1 = shape.Line.StartPoint;
					var p2 = shape.Line.EndPoint;
					context.Color = new Cairo.Color (c.R, c.G, c.B, c.A);
					context.NewPath ();
					context.LineWidth = w;
					context.MoveTo (p1.X, p1.Y);
					context.LineTo (p2.X, p2.Y);
					context.Stroke ();
				}
			}
		}

		public static DrawDelegate FillRoutine (Type type)
		{
			if (type == typeof (RectangleShape)) return FillRectangle;
			if (type == typeof (EllipseShape)) return FillEllipse;
			if (type == typeof (LineShape)) return FillLine;

			throw new NotImplementedException ("Fill type not supported: " + type.ToString ());
		}

		public static DrawDelegate BorderRoutine (Type type)
		{
			if (type == typeof (RectangleShape)) return BorderRectangle;
			if (type == typeof (EllipseShape)) return BorderEllipse;
			if (type == typeof (LineShape)) return BorderLine;

			throw new NotImplementedException ("Border type not supprted: " + type.ToString ());
		}

		public static Rectangle Bounds (ShapeBase shape)
		{
			// TODO: Compute bounds of shape, used for marking regions as dirty.
			throw new NotImplementedException ();
		}
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

		public Look (Brush fill, Pen border) {
			this.Fill = fill;
			this.Border = border;
		}
	}

	public struct Color
	{
		public double R;
		public double G;
		public double B;
		public double A;
	}

	public struct ColorList
	{
		public List<Color> Items;
	}

	public class SolidPen : Pen
	{
		public Color Color;
		public double Width;

		public static SolidPen Red = new SolidPen (1.0, 1.0, 0.0, 0.0, 1.0);
		public static SolidPen Green = new SolidPen (1.0, 0.0, 1.0, 0.0, 1.0);
		public static SolidPen Blue = new SolidPen (1.0, 0.0, 0.0, 1.0, 1.0);
		public static SolidPen Black = new SolidPen (1.0, 0.0, 0.0, 0.0, 1.0);
		public static SolidPen White = new SolidPen (1.0, 1.0, 1.0, 1.0, 1.0);

		public SolidPen (double width, Color color) {
			this.Width = width;
			this.Color = color;
		}

		public SolidPen (double width, double r, double g, double b, double a) {
			this.Width = width;
			this.Color = new Color () {
				R = r, 
				G = g, 
				B = b, 
				A = a
			};
		}
	}

	public class SolidBrush : Brush
	{
		public Color Color;

		public static SolidBrush Red = new SolidBrush (1.0, 0.0, 0.0, 1.0);
		public static SolidBrush Green = new SolidBrush (0.0, 1.0, 0.0, 1.0);
		public static SolidBrush Blue = new SolidBrush (0.0, 0.0, 1.0, 1.0);
		public static SolidBrush Black = new SolidBrush (0.0, 0.0, 0.0, 1.0);
		public static SolidBrush White = new SolidBrush (1.0, 1.0, 1.0, 1.0);

		public SolidBrush (Color color)
		{
			this.Color = color;
		}

		public SolidBrush (double r, double g, double b, double a)
		{
			Color = new Color () {R = r, G = g, B = b, A = a};
		}
	}

	public class NavigationBase
	{

	}

	public class UpNavigation : NavigationBase
	{

	}

	public class RootNavigation : NavigationBase
	{

	}

	public class ChildNavigation : NavigationBase
	{
		public string ChildName;
	}

	/// <summary>
	/// Represents a relative location in shape tree.
	/// If a group of items are copied and inserted in same parent node,
	/// they should be able to maintain correct relationship.
	/// </summary>
	public class Address : List<NavigationBase>
	{
		public ShapeTree Target;
		public bool Valid;
	}

	public abstract class ConstraintBase
	{

	}

	public class ParentConstraint : ConstraintBase
	{
		public Address Parent;
	}

	public class SinglePointConstraint : ConstraintBase
	{
		public Address Point;
	}

	public class BoneConstraint : ConstraintBase
	{
		public Address Point1;
		public Address Point2;
	}

	public class FeedbackConstraint : ConstraintBase
	{
		public Address Point1;
		public Address Point2;
		public Address RelativeLayer;
		public double MinAngle;
		public double MaxAngle;
	}

	public abstract class ShapeBase : Utils.Document.ICopyTo<ShapeBase>
	{
		public string Name;
		public bool Visible;
		public string[] Properties;

		public abstract object this[int indexer]
		{
			get;
			set;
		}

		public abstract ShapeBase CopyTo (ShapeBase shape);
	}

	public class ShapeTree : 
		IEnumerable<ShapeBase>, 
		Utils.Document.IDraw<Cairo.Context>
	{
		public Look Look;
		public ShapeBase Shape;
		public ShapeTree Parent;
		public List<ShapeTree> Children;
		public RenderingModule.DrawDelegate Fill;
		public RenderingModule.DrawDelegate Border;

		public ShapeTree ()
		{
		}

		public ShapeTree (Look look, ShapeBase shape)
		{
			this.Look = look;
			this.Shape = shape;
			Children = new List<ShapeTree> ();
			var type = shape.GetType ();
			Fill = RenderingModule.FillRoutine (type);
			Border = RenderingModule.BorderRoutine (type);
		}

		public void Draw (Cairo.Context context)
		{
			Fill (context, this);


			Border (context, this);
		}

		public ShapeTree AddChild (Look look, ShapeBase shape)
		{
			var node = new ShapeTree (look, shape);
			node.Parent = this;
			Children.Add (node);
			return node;
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

	public class KeyFrameShape : ShapeTree
	{
		public double Time;
		public ShapeBase State;
		public ShapeBase TangentIn;
		public ShapeBase TangentOut;

		public void SetState (ShapeBase shape) {
			throw new NotImplementedException ();
		}
	}

	/// <summary>
	/// Added to shape tree as nodes to simplify data structure.
	/// </summary>
	public class AnimationShape : ShapeTree
	{
		public Address Address;
		public string Property;
	}

	/// <summary>
	/// Multiple animations put together into an action.
	/// </summary>
	public class ActionShape : ShapeTree
	{
		public List<AnimationShape> KeyFrameCollections;
		public List<double> Offset;
	}
	
	public enum Anchor {
		Right = 1,
		Bottom = 2,
		Left = 4,
		Top = 8
	}

	public class View
	{
		public Anchor Anchor;
		public Rectangle FitInside;
	}

	public class Document : 
		ShapeTree, 
		Utils.Document.IRead<string>,
		Utils.Document.IWrite<string>,
		Utils.Document.IRead<Obf.OpenBinaryFormat>,
		Utils.Document.IWriteVersion<Obf.OpenBinaryFormat, int>
	{	
		void Utils.Document.IRead<string>.Read(string file)
		{
			throw new NotImplementedException();
		}

		void Utils.Document.IWrite<string>.Save(string file)
		{
			throw new NotImplementedException();
		}

		void Utils.Document.IRead<Obf.OpenBinaryFormat>.Read(Obf.OpenBinaryFormat r)
		{
			throw new NotImplementedException();
		}

		void Utils.Document.IWriteVersion<Obf.OpenBinaryFormat, int>.Save(
			Obf.OpenBinaryFormat writer, int version)
		{
			throw new NotImplementedException();
		}

		public ShapeTree Find (ShapeTree source, Address address) {
			throw new NotImplementedException ();
		}

		public Address CreateAddress (ShapeTree source, string path) {
			throw new NotImplementedException ();
		}
	}

	public class ControlPointShape : ShapeBase
	{
		public Point Position;

		public override object this[int indexer] {
			get {
				throw new System.NotImplementedException();
			}
			set {
				throw new System.NotImplementedException();
			}
		}

		public override ShapeBase CopyTo(ShapeBase shape)
		{
			throw new System.NotImplementedException();
		}
	}

	public class RectangleShapeBase : ShapeBase
	{
		public Rectangle Rectangle;

		public override object this[int indexer] {
			get {
				throw new System.NotImplementedException();
			}
			set {
				throw new System.NotImplementedException();
			}
		}

		public override ShapeBase CopyTo(ShapeBase shape)
		{
			throw new System.NotImplementedException();
		}
	}

	public class EllipseShape : RectangleShapeBase
	{

	}

	public class RectangleShape : RectangleShapeBase
	{

	}

	public class RoundRectangleShape : RectangleShapeBase
	{

	}

	public class LineShape : ShapeBase
	{
		public Line Line;

		public LineShape (Line line) {
			this.Line = line;
		}

		public LineShape (Point p1, Point p2) {
			this.Line = new Line () {
				StartPoint = p1,
				EndPoint = p2
			};
		}

		public LineShape (double x1, double y1, double x2, double y2) {
			this.Line = new Line () {
				StartPoint = new Point () {
					X = x1, 
					Y = y1
				}, 
				EndPoint = new Point () {
					X = x2, 
					Y = y2
				}
			};
		}

		public override object this[int indexer] {
			get {
				throw new System.NotImplementedException();
			}
			set {
				throw new System.NotImplementedException();
			}
		}

		public override ShapeBase CopyTo(ShapeBase shape)
		{
			throw new System.NotImplementedException();
		}
	}

	public abstract class PolygonShapeBase : ShapeBase
	{
		public List<Point> Points;

		public override object this[int indexer] {
			get {
				throw new System.NotImplementedException();
			}
			set {
				throw new System.NotImplementedException();
			}
		}

		public override ShapeBase CopyTo(ShapeBase shape)
		{
			throw new System.NotImplementedException();
		}
	}

	public class CatmullCurveShape : PolygonShapeBase
	{

	}

	public class OpenCatmullCurveShape : PolygonShapeBase
	{

	}

	public class QuadraticBezierShape : PolygonShapeBase
	{

	}

	public class OpenQuadraticBezierShape : PolygonShapeBase
	{

	}

	public class ImageShape : ShapeBase
	{
		public Cairo.ImageSurface Surface;

		public override object this[int indexer] {
			get {
				throw new System.NotImplementedException();
			}
			set {
				throw new System.NotImplementedException();
			}
		}

		public override ShapeBase CopyTo(ShapeBase shape)
		{
			throw new System.NotImplementedException();
		}
	}

	public class SpriteSheetShape : ShapeBase
	{
		public Cairo.ImageSurface Surface;
		public List<Rectangle> SourceRectangles;
		public List<Rectangle> TargetRectangles;

		public override object this[int indexer] {
			get {
				throw new System.NotImplementedException();
			}
			set {
				throw new System.NotImplementedException();
			}
		}

		public override ShapeBase CopyTo(ShapeBase shape)
		{
			throw new System.NotImplementedException();
		}
	}

	public class DeformShape : ShapeBase
	{
		public override object this[int indexer] {
			get {
				throw new System.NotImplementedException();
			}
			set {
				throw new System.NotImplementedException();
			}
		}

		public override ShapeBase CopyTo(ShapeBase shape)
		{
			throw new System.NotImplementedException();
		}
	}

	public class AbsoluteShape : ShapeBase
	{
		public override object this[int indexer] {
			get {
				throw new System.NotImplementedException();
			}
			set {
				throw new System.NotImplementedException();
			}
		}

		public override ShapeBase CopyTo(ShapeBase shape)
		{
			throw new System.NotImplementedException();
		}
	}

	public class RepeatShape : ShapeBase
	{
		public int RepeatX;
		public int RepeatY;

		public override object this[int indexer] {
			get {
				throw new System.NotImplementedException();
			}
			set {
				throw new System.NotImplementedException();
			}
		}

		public override ShapeBase CopyTo(ShapeBase shape)
		{
			throw new System.NotImplementedException();
		}
	}

	public class RevolveShape : ShapeBase
	{
		public int Resolution;

		public override object this[int indexer] {
			get {
				throw new System.NotImplementedException();
			}
			set {
				throw new System.NotImplementedException();
			}
		}

		public override ShapeBase CopyTo(ShapeBase shape)
		{
			throw new System.NotImplementedException();
		}
	}

	public class EventShape : ShapeBase
	{
		public override object this[int indexer] {
			get {
				throw new System.NotImplementedException();
			}
			set {
				throw new System.NotImplementedException();
			}
		}

		public override ShapeBase CopyTo(ShapeBase shape)
		{
			throw new System.NotImplementedException();
		}
	}

	public class LayerShape : ShapeBase
	{
		public ConstraintBase Attachment;
		public Matrix ModelTransform;
		public Matrix RestTransform;

		public override object this[int indexer] {
			get {
				throw new System.NotImplementedException();
			}
			set {
				throw new System.NotImplementedException();
			}
		}

		public override ShapeBase CopyTo(ShapeBase shape)
		{
			throw new System.NotImplementedException();
		}
	}

	/// <summary>
	/// State switch shape.
	/// 
	/// Copies values from old selected to new selected child.
	/// Makes it possible to program duplicates of same structure
	/// and still make them appear as one.
	/// </summary>
	public class StateSwitchShape : ShapeBase
	{
		public int SelectedChild;

		public override object this[int indexer] {
			get {
				throw new System.NotImplementedException();
			}
			set {
				throw new System.NotImplementedException();
			}
		}

		public override ShapeBase CopyTo(ShapeBase shape)
		{
			throw new System.NotImplementedException();
		}
	}
}

