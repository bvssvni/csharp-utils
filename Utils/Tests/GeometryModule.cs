using System;

namespace Utils
{
	/// <summary>
	/// Geometry module.
	/// 
	/// Computes different geometry problems.
	/// 
	/// 					radius		diameter	circumference
	/// 	radius			-			v			v
	/// 	diameter		v			-			v
	/// 	circumference	v			v			-
	/// 
	/// 					radians		degrees
	/// 	radians			-			v
	/// 	degrees			v			-
	/// 
	/// </summary>
	public static class GeometryModule
	{
		public const double RadiusToCircumference = 2 * Math.PI;
		public const double CircumferenceToRadius = 0.5 / Math.PI;
		public const double DiameterToCircumference = Math.PI;
		public const double CircumferenceToDiameter = 1.0 / Math.PI;

		public const double RadiusToDiameter = 2;
		public const double DiameterToRadius = 0.5;

		public const double DegreesToRadians = Math.PI / 180.0;
		public const double RadiansToDegrees = 180.0 / Math.PI;

		public static double AreaOfCircleByRadius (double rad) {
			return Math.PI * rad * rad;
		}

		/// <summary>
		/// Area of a filled half circle from center by factor.
		/// 
		/// Imagine a tunnel where the road is a straight line through a circle.
		/// If we fill the tunnel with water, we start at the road
		/// and then fill toward the round edge.
		/// </summary>
		/// <returns>The area.</returns>
		/// <param name="fact">Fact.</param>
		public static double AreaOfFilledHalfCircleFromCenterByFactor (double fact) {
			return Math.Sqrt (1 - fact * fact) * fact + Math.Asin (fact);
		}

		/// <summary>
		/// Area of a filled circle from edge by factor.
		/// 
		/// Imagine a round pipe filled with water,
		/// the factor is relative to the diameter of the pipe,
		/// and describes the maximum depth of water.
		/// </summary>
		/// <returns>The area.</returns>
		/// <param name="fact">Fact.</param>
		public static double AreaOfFilledCircleFromEdgeByFactor (double fact) {
			if (fact < 0.5) {
				return 0.5 * Math.PI - AreaOfFilledHalfCircleFromCenterByFactor (1 - 2 * fact);
			} else {
				return 0.5 * Math.PI + AreaOfFilledHalfCircleFromCenterByFactor (2 * fact - 1);
			}
		}

		/// <summary>
		/// Area of a triangle where vertices are on the edge of a unit circle.
		/// </summary>
		/// <returns>The of triangle in unit circle from edge by radians.</returns>
		/// <param name="a">The alpha component.</param>
		/// <param name="b">The blue component.</param>
		/// <param name="c">C.</param>
		public static double AreaOfTriangleInUnitCircleFromEdgeByRadians (double a, double b, double c) {
			return 2 * Math.Sin (a) * Math.Sin (b) * Math.Sin (b);
		}

		public static bool IsTriangleByRadians (double a, double b, double c) {
			return (a + b + c) == Math.PI;
		}
	}
}

