using System.Collections.Generic;
using System.Reflection;

namespace dotnetdoc.Creators
{
	internal sealed class TypeExampleCreator<T>
		: ITypeExampleCreator<T>
	{
		private readonly List<ICodeSnippetCreator> _creators;

		public TypeExampleCreator(string name, string description)
		{
			_creators = new List<ICodeSnippetCreator>();
		}

		/// <summary>
		/// </summary>
		/// <returns></returns>
		public ICodeSnippetCreator<T> AddCodeSnippet()
		{
			var creator = new CodeSnippetCreator<T>();
			_creators.Add(creator);
			return creator;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="method"></param>
		public void AddCodeSnippetFromMethod(MethodInfo method)
		{
			
		}

		public void RenderTo(string getCurrentDirectory)
		{
			foreach (var creator in _creators) creator.RenderTo(getCurrentDirectory);
		}
	}
}