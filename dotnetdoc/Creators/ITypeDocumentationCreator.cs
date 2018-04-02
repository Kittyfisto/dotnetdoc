using System;
using System.IO;
using dotnetdoc.Writers;

namespace dotnetdoc.Creators
{
	/// <summary>
	///     Responsible for creating the documentation for a particular type.
	/// </summary>
	public interface ITypeDocumentationCreator
	{
		/// <summary>
		///     The .NET type for which documentation is being created.
		/// </summary>
		Type Type { get; }

		/// <summary>
		///     Renders the contents of this creator to the given writer.
		/// </summary>
		/// <remarks>
		///     Supplementary resources (such as images) will be placed in <paramref name="path" />.
		/// </remarks>
		/// <param name="filesystem"></param>
		/// <param name="path"></param>
		/// <param name="writer"></param>
		void RenderTo(IFilesystem filesystem, string path, ITypeDocumentationWriter writer);
	}

	/// <summary>
	///     Responsible for creating the documentation for a particular type.
	/// </summary>
	public interface ITypeDocumentationCreator<out T>
		: ITypeDocumentationCreator
	{
		/// <summary>
		/// </summary>
		/// <param name="name"></param>
		/// <param name="description"></param>
		/// <returns></returns>
		ITypeExampleCreator<T> AddExample(string name, string description);
	}
}