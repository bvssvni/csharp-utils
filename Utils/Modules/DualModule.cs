using System;

namespace Utils
{
	public class DualModule
	{
		public static void Multiply (float[] a, float[] b, float[] res) {
			int count = (a.Length > b.Length ? a.Length : b.Length) >> 1;
			int a_dim = a.Length == 2 ? 0 : 2;
			int b_dim = b.Length == 2 ? 0 : 2;
			int res_dim = res.Length == 2 ? 0 : 2;
			int a_i, b_i, res_i;
			for (int i = count - 1; i >= 0; --i) {
				a_i = i * a_dim;
				b_i = i * b_dim;
				res_i = i * res_dim;
				res[res_i] += a[a_i] * b[b_i];
				res[res_i+1] += a[a_i] * b[b_i+1] + a[a_i+1] * b[b_i];
			}
		}

		public static void Divide (float[] a, float[] b, float[] res) {
			int count = (a.Length > b.Length ? a.Length : b.Length) >> 1;
			int a_dim = a.Length == 2 ? 0 : 2;
			int b_dim = b.Length == 2 ? 0 : 2;
			int res_dim = res.Length == 2 ? 0 : 2;
			int a_i, b_i, res_i;
			float c;
			for (int i = count - 1; i >= 0; --i) {
				a_i = i * a_dim;
				b_i = i * b_dim;
				res_i = i * res_dim;
				c = 1.0f / b[b_i];
				res[res_i] += a[a_i] * c;
				res[res_i+1] += (a[a_i+1] * b[b_i] - a[a_i] * b[b_i+1]) * c * c;
			}
		}

		public static void Pow (float[] a, float[] b, float[] res) {
			int count = (a.Length > b.Length ? a.Length : b.Length) >> 1;
			int a_dim = a.Length == 2 ? 0 : 2;
			int b_dim = b.Length == 2 ? 0 : 2;
			int res_dim = res.Length == 2 ? 0 : 2;
			int a_i, b_i, res_i;
			for (int i = count - 1; i >= 0; --i) {
				a_i = i * a_dim;
				b_i = i * b_dim;
				res_i = i * res_dim;
				res[res_i] += (float)(Math.Pow (a[a_i], b[b_i]));
				res[res_i+1] += (float)(a[a_i+1]*b[b_i]*Math.Pow (a[a_i], b[b_i]-1) +
				                        b[b_i+1]*Math.Pow (a[a_i], b[b_i])*Math.Log(a[a_i]));
			}
		}

		public static void Sqrt (float[] a, float[] res) {
			int count = a.Length >> 1;
			int a_dim = a.Length == 2 ? 0 : 2;
			int res_dim = res.Length == 2 ? 0 : 2;
			int a_i, res_i;
			for (int i = count - 1; i >= 0; --i) {
				a_i = i * a_dim;
				res_i = i * res_dim;
				res[res_i] += (float)(Math.Sqrt (a[a_i]));
				res[res_i+1] += (float)(a[a_i+1]*0.5/Math.Sqrt (a[a_i]));
			}
		}

		public static void Sin (float[] a, float[] res) {
			int count = a.Length >> 1;
			int a_dim = a.Length == 2 ? 0 : 2;
			int res_dim = res.Length == 2 ? 0 : 2;
			int a_i, res_i;
			for (int i = count - 1; i >= 0; --i) {
				a_i = i * a_dim;
				res_i = i * res_dim;
				res[res_i] += (float)(Math.Sin (a[a_i]));
				res[res_i+1] += (float)(a[a_i+1]*Math.Cos (a[a_i]));
			}
		}

		public static void Cos (float[] a, float[] res) {
			int count = a.Length >> 1;
			int a_dim = a.Length == 2 ? 0 : 2;
			int res_dim = res.Length == 2 ? 0 : 2;
			int a_i, res_i;
			for (int i = count - 1; i >= 0; --i) {
				a_i = i * a_dim;	
				res_i = i * res_dim;
				res[res_i] += (float)(Math.Cos (a[a_i]));
				res[res_i+1] += (float)(-a[a_i+1]*Math.Sin (a[a_i]));
			}
		}

		public static void Tan (float[] a, float[] res) {
			int count = a.Length >> 1;
			int a_dim = a.Length == 2 ? 0 : 2;
			int res_dim = res.Length == 2 ? 0 : 2;
			int a_i, res_i;
			double sec;
			for (int i = count - 1; i >= 0; --i) {
				a_i = i * a_dim;	
				res_i = i * res_dim;
				res[res_i] += (float)(Math.Tan (a[a_i]));
				sec = 2 * Math.Cos (a[a_i]) / (Math.Cos(2 * a[a_i]) + 1);
				res[res_i+1] += (float)(a[a_i+1]*sec*sec);
			}
		}

		public static void Add (float[] a, float[] b, float[] res) {
			int count = (a.Length > b.Length ? a.Length : b.Length) >> 1;
			int a_dim = a.Length == 2 ? 0 : 2;
			int b_dim = b.Length == 2 ? 0 : 2;
			int res_dim = res.Length == 2 ? 0 : 2;
			int a_i, b_i, res_i;
			for (int i = count - 1; i >= 0; --i) {
				a_i = i * a_dim;
				b_i = i * b_dim;
				res_i = i * res_dim;
				res[res_i] += a[a_i] + b[b_i];
				res[res_i+1] += a[b_i+1] + b[b_i+1];
			}
		}

		public static void Subtract (float[] a, float[] b, float[] res) {
			int count = (a.Length > b.Length ? a.Length : b.Length) >> 1;
			int a_dim = a.Length == 2 ? 0 : 2;
			int b_dim = b.Length == 2 ? 0 : 2;
			int res_dim = res.Length == 2 ? 0 : 2;
			int a_i, b_i, res_i;
			for (int i = count - 1; i >= 0; --i) {
				a_i = i * a_dim;
				b_i = i * b_dim;
				res_i = i * res_dim;
				res[res_i] += a[a_i] - b[b_i];
				res[res_i+1] += a[b_i+1] - b[b_i+1];
			}
		}

		public static bool AllEquals (float[] a, float[] b, float epsilon) {
			int count = (a.Length > b.Length ? a.Length : b.Length) >> 1;
			int a_dim = a.Length == 2 ? 0 : 2;
			int b_dim = b.Length == 2 ? 0 : 2;
			int a_i, b_i;
			for (int i = count - 1; i >= 0; --i) {
				a_i = i * a_dim;
				b_i = i * b_dim;
				if (Math.Abs(a[a_i] - b[b_i]) > epsilon || Math.Abs(a[a_i+1] - b[b_i+1]) > epsilon) {
					return false;
				}
			}

			return true;
		}

		public static bool AnyEquals (float[] a, float[] b) {
			int count = (a.Length > b.Length ? a.Length : b.Length) >> 1;
			int a_dim = a.Length == 2 ? 0 : 2;
			int b_dim = b.Length == 2 ? 0 : 2;
			int a_i, b_i;
			for (int i = count - 1; i >= 0; --i) {
				a_i = i * a_dim;
				b_i = i * b_dim;
				if (a[a_i] == b[b_i] && a[a_i+1] == b[b_i+1]) {
					return true;
				}
			}
			
			return false;
		}
	}
}

