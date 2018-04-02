using System;
using System.IO;
using System.Linq.Expressions;

namespace dotnetdoc.Creators
{
	/// <summary>
	/// 
	/// </summary>
	public interface ICodeSnippetCreator
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="func"></param>
		void Add(Expression<Func<AssemblyDocumentationCreator>> func);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="writer"></param>
		void RenderTo(TextWriter writer);
	}

	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface ICodeSnippetCreator<out T>
		: ICodeSnippetCreator
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="type"></param>
		/// <param name="mainName"></param>
		void AddFromMethod(Type type, string mainName);
	}
}