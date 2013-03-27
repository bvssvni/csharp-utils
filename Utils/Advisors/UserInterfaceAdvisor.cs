/*
UserInterfaceAdvisor - Enum order driven actions and refreshes for user interfaces.  
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

namespace Utils
{
	/// <summary>
	/// An UserInterfaceAdvisor is an object that performs actions and refreshes interface in correct order.
	/// The order is controlled in the declaration of two enums:
	/// 
	/// 1. Action - declared within the advisor.
	/// 2. UserInteface - declared in App object.
	/// 
	/// For example, a DocumentAdvisor can control logic to open and save documents.
	/// For example, a FrameAdvisor can control logic to navigate an animation.
	/// For example, a HistoryAdvisor can control logic to undo or redo.
	/// </summary>
	public abstract class UserInterfaceAdvisor<Event, Action, UserInterface>
	{
		public abstract void DoAction(Event e, Action action);
		public abstract void Refresh(Event e, UserInterface ui);
		
		public void Do(Event e) {
			var actions = (Action[])Enum.GetValues(typeof(Action));
			for (int i = 0; i < actions.Length; i++) {
				var action = actions[i];
				DoAction(e, action);
			}
			
			var uis = (UserInterface[])Enum.GetValues(typeof(UserInterface));
			for (int i = 0; i < uis.Length; i++) {
				var ui = uis[i];
				Refresh(e, ui);
			}
		}
	}
}

