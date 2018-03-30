using System;

namespace dotnetdoc
{
	/// <summary>
	///     Responsible for creating the documentation for a particular type.
	/// </summary>
	public interface ITypeDocumentationCreator
		: IDisposable
	{
	}

	/// <summary>
	///     Responsible for creating the documentation for a particular type.
	/// </summary>
	public interface ITypeDocumentationCreator<out T>
		: ITypeDocumentationCreator
	{
	}
}