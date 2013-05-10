/*
StateModule - Makes it easy to build and combine state machines.  
https://github.com/bvssvni/csharp-utils
BSD license.  
by Sven Nilsen, 2013  
http://www.cutoutpro.com  
Version: 0.000 in angular degrees version notation  
http://isprogrammingeasy.blogspot.no/2012/08/angular-degrees-versioning-notation.html  

Redistribution and use in source and binary forms, with or without  
modification, are permitted provided that the following conditions are met:  
1. Redistributions of source code must retain the above copyright notice, this  
list of conditions and the following disclaimer.  
2. Redistributions in binary form must reproduce the above copyright notice,  
this list of conditions and the following disclaimer in the documentation  
and/or other materials provided with the distribution.  
THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND  
ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED  
WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE  
DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR  
ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES  
(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;  
LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND  
ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT  
(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS  
SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.  
The views and conclusions contained in the software and documentation are those  
of the authors and should not be interpreted as representing official policies,  
either expressed or implied, of the FreeBSD Project.  
*/

using System;
using System.Collections.Generic;

namespace Utils
{
	/// <summary>
	/// State module.
	/// 
	/// Executes state machines written as IEnumerable objects.
	/// The iterators emit functions that has to be evaluated to true
	/// before advancing to the next state.
	/// 
	/// It makes it possible to write high level logic C# code.
	/// </summary>
	public static class StateModule<T>
	{
		// Returns true if state should be executed once more.
		public delegate bool StateDelegate (State state);

		public delegate void AddStateMachineDelegate (IEnumerable<StateDelegate> actor);
		public delegate void QuitStateMachineExecutionDelegate ();

		public class State
		{
			public T Data;
			public AddStateMachineDelegate Add;
			public QuitStateMachineExecutionDelegate Quit;
		}

		public static void Run (T data, params IEnumerable<StateDelegate>[] actors)
		{
			// Create one queue for states and their state machine.
			var stateMachines = new Queue<IEnumerator<StateDelegate>> (actors.Length);
			var states = new Queue<StateDelegate> (actors.Length);
			foreach (var actor in actors) {
				var stateMachine = actor.GetEnumerator ();
				stateMachine.MoveNext ();
				stateMachines.Enqueue (stateMachine);
				states.Enqueue (stateMachine.Current);
			}

			// Allow states to emit new state machines for execution.
			AddStateMachineDelegate addFunc = (newActor) => {
				var stateMachine = newActor.GetEnumerator ();
				stateMachines.Enqueue (stateMachine);
				states.Enqueue (stateMachine.Current);
			};

			// Use a flag to terminate execution on request by state.
			var active = true;
			QuitStateMachineExecutionDelegate quitFunc = () => active = false;
			
			var s = new State () {Data = data, Add = addFunc, Quit = quitFunc};
			while (active && states.Count > 0) {
				var state = states.Dequeue ();
				var stateMachine = stateMachines.Dequeue ();
				if (state (s)) {
					// State is not complete.
					// Put the state back in execution queue.
					states.Enqueue (state);
					stateMachines.Enqueue (stateMachine);
					continue;
				} 

				// Advance state machine to next state.
				// If it ends, stop executing it.
				stateMachine.MoveNext ();
				state = stateMachine.Current;
				if (state == null) continue;

				// Put state back in execution queue.
				states.Enqueue (state);
				stateMachines.Enqueue (stateMachine);
			}
		}
	}
}

