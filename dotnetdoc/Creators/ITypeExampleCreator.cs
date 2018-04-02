using System.Reflection;

namespace dotnetdoc.Creators
{
	public interface ITypeExampleCreator
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="method"></param>
		/// <returns></returns>
		void AddCodeSnippetFromMethod(MethodInfo method);
	}

	public interface ITypeExampleCreator<out T>
		: ITypeExampleCreator
	{
		ICodeSnippetCreator<T> AddCodeSnippet();
	}
}