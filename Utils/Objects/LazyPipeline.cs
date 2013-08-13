using System;
using System.Collections.Generic;

namespace Utils
{
	/// <summary>
	/// A lazy pipeline performs tasks optimally through time delayed states.
	/// 
	/// 	var pipeline = new LazyPipeLine<T>();
	/// 	// Initialize states.
	/// 	pipeline.States.Add(...);
	/// 	// Initialize tasks.
	/// 	pipeline.Tasks.Add(...);
	/// 	// Loop to optimize tasks for execution.
	/// 	while (pipeline.Tick())
	/// 	{
	/// 		// Remove tasks that have completed all stages.
	/// 		pipeline.Flush();
	/// 		// Give other threads oppertunity to do some work.
	/// 		System.Threading.Thread.Sleep(0);
	/// 	}
	/// 
	/// </summary>
	public class LazyPipeline<T>
	{
		public readonly List<T> Tasks = new List<T>();
		public readonly List<State> States = new List<State>();
		private int m_done;

		public int Done
		{
			get
			{
				return m_done;
			}
		}

		public class State
		{
			public int Count;
			public StartStateDelegate Start;
			public EndStateDelegate End;
		}

		public delegate bool StartStateDelegate(T task);
		public delegate bool EndStateDelegate(T task);

		public void Flush()
		{
			var n = m_done;
			Tasks.RemoveRange(0, n);
			foreach (var state in States)
			{
				state.Count -= n;
			}
		}

		public bool Tick()
		{
			int n = States.Count;
			int t = Tasks.Count;

			// Mark all tasks that are done as done.
			var lastState = States[n-1];
			for (int i = m_done; i < lastState.Count; i++)
			{
				bool ready = lastState.End(Tasks[i]);
				if (!ready) {break;}

				m_done++;
			}

			if (m_done == t)
			{
				// All tasks are done.
				return false;
			}

			// Put pressure to start new tasks if none are ready.
			for (int i = n - 1; i > 0; i--)
			{
				State state = States[i];
				if (state.Count == t) 
				{
					// If all tasks are in this state, return.
					// We have to wait for tasks in later states to complete.
					return true;
				}

				State prevState = States[i+1];
				if (state.Count == prevState.Count)
				{
					// The previous state is waiting in line for new tasks.
					// Continue to the bottleneck state.
					continue;
				}

				bool ready = prevState.End(Tasks[state.Count]);
				if (ready)
				{
					// Start new task in this state.
					if (state.Start(Tasks[state.Count]))
					{
						state.Count++;
						return true;
					}
				}
			}

			State firstState = States[0];
			if (firstState.Count == t)
			{
				// All tasks are done in first state.
				return true;
			}

			// Start new task in first state.
			if (firstState.Start(Tasks[firstState.Count]))
			{
				firstState.Count++;
			}

			return true;
		}
	}
}

