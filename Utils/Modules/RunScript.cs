using System;
using System.Reflection;
using Mono.CSharp;

namespace Utils
{
	/// <summary>
	/// Allows compiling C# and execute it.
	/// 
	/// Running sources directly are slow because they have to be compiled first.
	/// If you compile the source first, you can boost performance with 2^14 times.
	/// </summary>
	public class RunScript
	{
		/// <summary>
		/// cl 12.
		/// </summary>
		/// <param name="source">Source.</param>
		public static void Run (string source)
		{
			Evaluator.Init(new string[]{});
			var entryAssembly = Assembly.GetEntryAssembly();
			if (entryAssembly != null) Evaluator.ReferenceAssembly(entryAssembly);
			Evaluator.Run (source);
		}

		/// <summary>
		/// cl 12.
		/// </summary>
		/// <param name="source">Source.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static T Evaluate<T>(string source)
		{
			Evaluator.Init(new string[]{});
			var entryAssembly = Assembly.GetEntryAssembly();
			if (entryAssembly != null) Evaluator.ReferenceAssembly(entryAssembly);
			return (T)Convert.ChangeType(Evaluator.Evaluate(source), typeof(T));
		}

		/// <summary>
		/// cl 12.
		/// </summary>
		/// <param name="source">Source.</param>
		public static CompiledMethod Compile(string source)
		{
			Evaluator.Init(new string[]{});
			var entryAssembly = Assembly.GetEntryAssembly();
			if (entryAssembly != null) Evaluator.ReferenceAssembly(entryAssembly);
			return Evaluator.Compile(source);
		}

		public static T Execute<T>(CompiledMethod method)
		{
			var obj = new object();
			method(ref obj);
			return (T)Convert.ChangeType(obj, typeof(T));
		}
	}
}

