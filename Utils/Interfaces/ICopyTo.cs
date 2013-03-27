using System;

namespace Utils
{
	/// <summary>
	/// When implementing this interface,
	/// remember to return the object copied to.
	/// </summary>
	public interface ICopyTo<Argument>
	{
		Argument CopyTo(Argument obj);
	}
}

