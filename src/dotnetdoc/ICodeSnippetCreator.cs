using System;
using System.Linq.Expressions;
using System.Reflection;
using dotnetdoc.Creators;
using dotnetdoc.Writers;

namespace dotnetdoc
{
	/// <summary>
	/// 
	/// </summary>
	public interface ICodeSnippetCreator
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="method"></param>
		void AddFromMethod(MethodInfo method);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="writer"></param>
		void RenderTo(IExampleWriter writer);
	}

	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface ICodeSnippetCreator<T>
		: ICodeSnippetCreator
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="func"></param>
		void Add(Expression<Func<T>> func);
	}
}