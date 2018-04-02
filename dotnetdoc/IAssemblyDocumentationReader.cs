using System;
using System.Diagnostics.Contracts;

namespace dotnetdoc
{
	/// <summary>
	///     Provides access to documentation.
	/// </summary>
	public interface IAssemblyDocumentationReader
	{
		[Pure]
		TypeDocumentation GetDocumentationOf(Type type);

		[Pure]
		TypeDocumentation GetDocumentationOf<T>();
	}
}