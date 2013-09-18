using System;

namespace Utils
{
	/// <summary>
	/// A structure that generalizes numbers in a tree hierarchy.
	/// </summary>
	public struct Number
	{
		public object X;
		public object dX;

		public static object Add(object a, object b)
		{
			bool aNum = a is Number;
			bool bNum = b is Number;
			if (!aNum && !bNum)
			{
				return (double)a + (double)b;
			}
			else if (aNum && bNum)
			{
				return new Number(){X = Add(((Number)a).X, ((Number)b).X), 
					dX = Add(((Number)a).dX, ((Number)b).dX)};
			}
			else if (aNum && !bNum)
			{
				return new Number(){X = Add(((Number)a).X, b), dX = ((Number)a).dX};
			}
			else
			{
				return new Number(){X = Add(a, ((Number)b).X), dX = ((Number)b).dX};
			}
		}

		public static object Subtract(object a, object b)
		{
			bool aNum = a is Number;
			bool bNum = b is Number;
			if (!aNum && !bNum)
			{
				return (double)a - (double)b;
			}
			else if (aNum && bNum)
			{
				return new Number(){X = Subtract(((Number)a).X, ((Number)b).X), 
					dX = Subtract(((Number)a).dX, ((Number)b).dX)};
			}
			else if (aNum && !bNum)
			{
				return new Number(){X = Subtract(((Number)a).X, b), dX = ((Number)a).dX};
			}
			else
			{
				return new Number(){X = Subtract(a, ((Number)b).X), dX = ((Number)b).dX};
			}
		}

		public static object Conjugate(object a)
		{
			bool aNum = a is Number;
			if (!aNum) 
			{
				return a;
			}
			else 
			{
				return new Number(){
					X = Conjugate(((Number)a).X),
					dX = Subtract(0.0, Conjugate(((Number)a).dX))
				};
			}
		}

		public static object Multiply(object a, object b)
		{
			bool aNum = a is Number;
			bool bNum = b is Number;
			object result = null;
			if (!aNum && !bNum)
			{
				result = (double)a * (double)b;
			}
			else if (aNum && bNum)
			{
				Number aNumber = (Number)a;
				Number bNumber = (Number)b;
				var ab = Multiply(aNumber.X, bNumber.X);
				bool aNumDx = aNumber.dX is Number;
				bool bNumDx = bNumber.dX is Number;
				if (aNumDx && bNumDx)
				{
					Number aNumberDx = (Number)aNumber.dX;
					Number bNumberDx = (Number)bNumber.dX;
					ab = Subtract(ab, Multiply(aNumberDx.dX, bNumberDx.dX));
				}

				result = new Number()
				{

					X = ab,
					dX = Add(
						Multiply(((Number)a).X, ((Number)b).dX),
						Multiply(((Number)a).dX, Conjugate(((Number)b).X))
						)
				};
			}
			else if (!aNum && bNum)
			{
				result = new Number()
				{
					X = Multiply(a, ((Number)b).X),
					dX = Multiply(a, ((Number)b).dX)
				};
			}
			else
			{
				result = new Number()
				{
					X = Multiply(((Number)a).X, b),
					dX = Multiply(((Number)a).dX, b)
				};
			}

			if (result is Number)
			{
				var numResult = (Number)result;
				if (!(numResult.dX is Number) && !(numResult.X is Number) &&
				    (double)numResult.dX == 0.0)
				{
					return numResult.X;
				}
			}

			return result;
		}

		public static bool Equal(object a, object b)
		{
			bool aNum = a is Number;
			bool bNum = b is Number;
			if (!aNum && !bNum)
			{
				return (double)a == (double)b;
			}
			else if (aNum && bNum)
			{
				return Equal(((Number)a).X, ((Number)b).X) &&
					Equal(((Number)a).dX, ((Number)b).dX);
			}
			else if (!aNum && bNum)
			{
				return Equal(a, ((Number)b).X) &&
					Equal(0.0, ((Number)b).dX);
			}
			else
			{
				return Equal(((Number)a).X, b) &&
					Equal(((Number)a).dX, 0.0);
			}
		}

		public override bool Equals(object obj)
		{
			return Equal(this, obj);
		}

		public static bool operator == (Number a, Number b)
		{
			return Equal(a, b);
		}

		public static bool operator != (Number a, Number b)
		{
			return !Equal(a, b);
		}

		public static bool operator == (Number a, double b)
		{
			return Equal(a, b);
		}
		
		public static bool operator != (Number a, double b)
		{
			return !Equal(a, b);
		}

		public static bool operator == (double a, Number b)
		{
			return Equal(a, b);
		}
		
		public static bool operator != (double a, Number b)
		{
			return !Equal(a, b);
		}

		public static Number Complex(double x, double y)
		{
			return new Number()
			{
				X = x,
				dX = new Number()
				{
					X = 0.0,
					dX = y
				}
			};
		}

		public static Number Quaternion(double w, double x, double y, double z)
		{
			return new Number()
			{
				X = new Number()
				{
					X = w,
					dX = new Number()
					{
						X = 0.0,
						dX = x
					}
				},
				dX = new Number()
				{
					X = 0.0,
					dX = new Number()
					{
						X = y,
						dX = new Number()
						{
							X = 0.0,
							dX = z
						}
					}
				}
			};
		}

		public object Img
		{
			get
			{
				return ((Number)dX).dX;
			}
			set
			{
				var b = (Number)dX;
				b.dX = value;
			}
		}

		public object QW
		{
			get
			{
				return ((Number)X).X;
			}
		}

		public object QI
		{
			get
			{
				return ((Number)X).Img;
			}
		}

		public object QJ
		{
			get
			{
				return ((Number)Img).X;
			}
		}

		public object QK
		{
			get
			{
				return ((Number)Img).Img;
			}
		}

		public static Number operator + (Number a, Number b)
		{
			return new Number(){X = Add(a.X, b.X), dX = Add(a.dX, b.dX)};
		}

		public static Number operator - (Number a, Number b)
		{
			return new Number(){X = Subtract(a.X, b.X), dX = Subtract(a.dX, b.dX)};
		}

		public static Number operator * (Number a, Number b)
		{
			var res = Multiply(a, b);
			return res is Number ? (Number)res : new Number(){X = res, dX = 0.0};
		}

		public static Number operator + (Number a, double b)
		{
			return new Number(){X = Add(a.X, b), dX = a.dX};
		}

		public static Number operator - (Number a, double b)
		{
			return new Number(){X = Subtract(a.X, b), dX = a.dX};
		}

		public static Number operator * (Number a, double b)
		{
			return new Number()
			{
				X = Multiply(a.X, b),
				dX = Multiply(a.dX, b)
			};
		}

		public static Number operator + (double a, Number b)
		{
			return new Number(){X = Add(a, b.X), dX = b.dX};
		}

		public static Number operator - (double a, Number b)
		{
			return new Number(){X = Subtract(a, b.X), dX = Subtract(0.0, b.dX)};
		}

		public static Number operator * (double a, Number b)
		{
			return new Number()
			{
				X = Multiply(a, b.X),
				dX = Multiply(a, b.dX)
			};
		}

		public override string ToString()
		{
			return "{" + X.ToString() + "," + dX.ToString() + "}";
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hash = 13;
				hash = (hash * 7) + X.GetHashCode();
				hash = (hash * 7) + dX.GetHashCode();
				return hash;
			}
		}
	}
}

