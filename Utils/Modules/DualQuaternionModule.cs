using System;

namespace Utils
{
	public static class DualQuaternionModule
	{
		public static void Multiply (float[] a, float[] b, float[] res) {
			res[0] = a[0]*b[0]-a[2]*b[2]-a[4]*b[4]-a[6]*b[6];
			res[1] = a[0]*b[1]+a[1]*b[0]-a[2]*b[3]-a[3]*b[2]-a[4]*b[5]-a[5]*b[4]-a[6]*b[7]-a[7]*b[6];
			res[2] = a[0]*b[2]+a[2]*b[0]+a[4]*b[6]-a[6]*b[4];
			res[3] = a[0]*b[3]+a[1]*b[2]+a[2]*b[1]+a[3]*b[0]+a[4]*b[7]+a[5]*b[6]-a[6]*b[5]-a[7]*b[4];
			res[4] = a[0]*b[4]-a[2]*b[6]+a[4]*b[0]+a[6]*b[2];
			res[5] = a[0]*b[5]+a[1]*b[4]-a[2]*b[7]-a[3]*b[6]+a[4]*b[1]+a[5]*b[0]+a[6]*b[3]+a[7]*b[2];
			res[6] = a[0]*b[6]+a[2]*b[4]-a[4]*b[2]+a[6]*b[0];
			res[7] = a[0]*b[7]+a[1]*b[6]+a[2]*b[5]+a[3]*b[4]-a[4]*b[3]-a[5]*b[2]+a[6]*b[1]+a[7]*b[0];
		}
	}
}

