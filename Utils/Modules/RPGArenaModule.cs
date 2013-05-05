using System;

namespace Utils
{
	/// <summary>
	/// RPG arena module.
	/// 
	/// The idea is to give each attribute a value between 0 and 1000.
	/// </summary>
	public class RPGArenaModule
	{
		public static float AttributeFromClass (Random rnd, int c) {
			return (float) (rnd.Next(100) + c * 100);
		}

		public static float[] AttributesFromClasses (Random rnd, int[] cs) {
			float[] res = new float [cs.Length];
			for (int i = 0; i < cs.Length; i++) {
				res [i] = cs [i];
			}

			return res;
		}

		public static float[][] Arena (Random rnd, int[][] classes, params int[] enemies) {
			float[][] res = new float[enemies.Length][];
			for (int i = 0; i < enemies.Length; i++) {
				res [i] = AttributesFromClasses (rnd, classes [enemies [i]]);
			}

			return res;
		}

		public static float Damage (float attack, float resistance) {
			// A negative resistance means the entity is immune.
			if (resistance < 0) {
				return 0;
			}

			// Increase the damange with the square of attack strength.
			return attack * attack / resistance;
		}

		public static bool IsDead (float life) {
			return life <= 0;
		}
	}
}

