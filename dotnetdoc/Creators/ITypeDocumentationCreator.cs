using System;
using dotnetdoc.Writers;
using dotnetdoc.Writers.Markdown;

namespace dotnetdoc.Creators
{
	/// <summary>
	///     Responsible for creating the documentation for a particular type.
	/// </summary>
	public interface ITypeDocumentationCreator
	{
		Type Type { get; }

		void RenderTo(ITypeDocumentationWriter writer);
	}

	/// <summary>
	///     Responsible for creating the documentation for a particular type.
	/// </summary>
	public interface ITypeDocumentationCreator<out T>
		: ITypeDocumentationCreator
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <param name="description"></param>
		/// <returns></returns>
		ITypeExampleCreator<T> AddExample(string name, string description);
	}
}