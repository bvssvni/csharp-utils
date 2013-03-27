using System;

namespace Utils
{
	public interface IWrite<T>
	{
		void Save(T writer);
	}
}

