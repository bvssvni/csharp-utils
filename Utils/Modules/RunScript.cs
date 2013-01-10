using System;
using Mono.CSharp;

namespace Utils
{
	/// <summary>
	/// Allows compiling C# and execute it.
	/// </summary>
	public class RunScript
	{
		public static void Run(string source)
		{
			Evaluator.Init(new string[]{});
			Evaluator.ReferenceAssembly(System.Reflection.Assembly.GetEntryAssembly());
			Evaluator.Run (source);
		}

		public static T Evaluate<T>(string source)
		{
			Evaluator.Init(new string[]{});
			Evaluator.ReferenceAssembly(System.Reflection.Assembly.GetEntryAssembly());
			return (T)Convert.ChangeType(Evaluator.Evaluate(source), typeof(T));
		}
	}
}

