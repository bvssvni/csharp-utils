using System;

namespace Utils
{
	public class DualModule
	{
		public static Cheap<DualF> Sum (Cheap<DualF> list, int stride) {
			int start_in = 0, end_in = 0;
			list.GetRange (ref start_in, ref end_in);
			int n = (end_in - start_in) / stride;
			var res = Cheap<DualF>.WithCapacity (n);
			int start_out = 0, end_out = 0;
			res.GetRange (ref start_out, ref end_out);
			var items = Cheap<DualF>.Items;
			int i, j;
			int in_i, out_i;
			for (i = n - 1; i >= 0; --i) {
				out_i = start_out + i;
				for (j = stride - 1; j >= 0; --j) {
					in_i = start_in + i * stride + j;
					items[out_i].X += items[in_i].X;
					items[out_i].Dx += items[in_i].Dx;
				}
			}

			return res;
		}
	}
}

