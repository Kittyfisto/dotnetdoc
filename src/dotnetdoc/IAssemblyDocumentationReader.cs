using System;
using System.Diagnostics.Contracts;

namespace dotnetdoc
{
	/// <summary>
	///     Provides access to documentation.
	/// </summary>
	public interface IAssemblyDocumentationReader
	{
		/// <summary>
		///     Retrieves the documentation for the given <paramref name="type" />.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		[Pure]
		TypeDocumentation GetDocumentationOf(Type type);

		/// <summary>
		///     Retrieves the documentation for the given <typeparamref name="T" />.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		TypeDocumentation GetDocumentationOf<T>();
	}
}