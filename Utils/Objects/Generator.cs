using System;
using System.Collections.Generic;

namespace Play
{
	/// <summary>
	/// A generator is the equivelant of an algorithmic group.
	/// Like groups, they can be combined with Boolean algebra.
	/// Generators are good at problems where the next index is easy to find.
	/// 
	/// A generator behaves like a lazy evaluated group.
	/// 
	/// 
	/// Groups and generators complement each other,
	/// because a generator is slow to iterate a group,
	/// and a group takes lot of memory to represent a generator.
	/// 
	/// This representation uses functions for internal behavior.
	/// It allows for a wide class of generators, without using object inheritance.
	/// 
	/// There is only one restriction, which is that the next counter
	/// can be extracted from the previous value that might be outside the values generated.
	/// This happens when two generators are combined, and they are passed the same number
	/// because no state is maintained internally.
	/// 
	/// However, this is only a problem when a generator is allowed to take multiple steps.
	/// 
	/// All generators should continue to produce higher values even they gets outside the bounds.
	/// This is because the condition for continuing is checked after getting next value.
	/// 
	/// </summary>
	public class Generator <T> where T : IComparable 
	{
		public delegate T ResetDelegate ();

		public ResetDelegate Reset;

		public delegate T NextDelegate (T i);

		private NextDelegate m_next;

		public delegate bool ShouldDelegate (T i);

		private ShouldDelegate m_should;

		public delegate int CompareDelegate (T a, T b);

		public Generator (ResetDelegate reset, NextDelegate next, ShouldDelegate should)
		{
			this.Reset = reset;
			this.m_next = next;
			this.m_should = should;
		}

		public bool Next(ref T i)
		{
			i = m_next(i);

			if (!m_should(i)) return false;

			return true;
		}

		public static Generator<T> Or (Generator<T> a, Generator<T> b)
		{
			ResetDelegate reset = delegate {
				T ai = a.Reset ();
				T bi = b.Reset ();
				return ai.CompareTo(bi) < 0 ? ai : bi;
			};
			NextDelegate next = delegate (T i) {
				T ai = a.m_next (i);
				T bi = a.m_next (i);
				return ai.CompareTo(bi) < 0 ? ai : bi;
			};
			ShouldDelegate should = x => a.m_should (x) || b.m_should (x);
			return new Generator<T> (reset, next, should);
		}

		public static Generator<T> And (Generator<T> a, Generator<T> b)
		{
			ResetDelegate reset = delegate {
				T ai = a.Reset ();
				T bi = b.Reset ();
				return ai.CompareTo(bi) > 0 ? ai : bi;
			};
			NextDelegate next = delegate (T i) {
				T ai = a.m_next (i);
				T bi = b.m_next (i);
				return ai.CompareTo(bi) > 0 ? ai : bi;
			};
			ShouldDelegate should = x => a.m_should (x) && b.m_should (x);
			return new Generator<T> (reset, next, should);
		}

		public static Generator<T> Except (Generator<T> a, Generator<T> b)
		{
			ResetDelegate reset = delegate {
				T ai = a.Reset ();
				T bi = b.Reset ();
				while (bi.CompareTo(ai) <= 0) {
					if (bi.CompareTo(ai) < 0) {
						bi = b.m_next (bi);
					}
					if (ai.CompareTo(bi) == 0) {
						if (!b.m_should (bi)) break;
						
						ai = a.m_next (ai);
					}
				}
				return ai;
			};
			NextDelegate next = delegate (T i) {
				T ai = a.m_next (i);
				T bi = i;
				while (bi.CompareTo(ai) <= 0) {
					if (bi.CompareTo(ai) < 0) {
						bi = b.m_next (bi);
					}
					if (ai.CompareTo(bi) == 0) {
						if (!b.m_should (bi)) break;

						ai = a.m_next (ai);
					}
				}
				return ai;
			};
			ShouldDelegate should = x => a.m_should (x) && !b.m_should (x);
			return new Generator<T> (reset, next, should);
		}
	}
}

