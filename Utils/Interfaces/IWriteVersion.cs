using System;

namespace Utils
{
	public interface IWriteVersion<T, V>
	{
		void Save(T writer, V version);
	}
}

