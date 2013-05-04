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

		public static bool AllEquals (float[] a, float[] b) {
			int count = (a.Length > b.Length ? a.Length : b.Length) >> 1;
			int a_dim = a.Length == 2 ? 0 : 2;
			int b_dim = b.Length == 2 ? 0 : 2;
			int a_i, b_i;
			for (int i = count - 1; i >= 0; --i) {
				a_i = i * a_dim;
				b_i = i * b_dim;
				if (a[a_i] != b[b_i] || a[a_i+1] != b[b_i+1]) {
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

