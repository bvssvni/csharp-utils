using System;

namespace Utils
{
	/// <summary>
	/// RPG arena module.
	/// 
	/// The idea is to give each attribute a value between 0 and 1000.
	/// 1000 is the maximum value of any ability.
	/// 100 is the starting point for the player.
	/// </summary>
	public class RPGArenaModule
	{
		public static float[] AttributesFromClasses (Random rnd, int[] cs) {
			float[] res = new float [cs.Length];
			for (int i = 0; i < cs.Length; i++) {
				res [i] = (float) (rnd.Next(100) + cs[i] * 100);
			}

			return res;
		}

		public static float[][] ArenaTeam (Random rnd, int[][] classes, params int[] team) {
			float[][] res = new float[team.Length][];
			for (int i = 0; i < team.Length; i++) {
				res [i] = AttributesFromClasses (rnd, classes [team [i]]);
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

