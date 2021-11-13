using System.Collections.Generic;
using System.Reflection;
using dotnetdoc.Writers;

namespace dotnetdoc.Creators
{
	internal sealed class TypeExampleCreator<T>
		: ITypeExampleCreator<T>
	{
		private readonly string _name;
		private readonly List<ICodeSnippetCreator> _creators;

		public TypeExampleCreator(string name, string description)
		{
			_name = name;
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

		public void RenderTo(IExampleWriter writer)
		{
			foreach (var creator in _creators)
			{
				creator.RenderTo(writer);
			}
		}

		public string Name => _name;
	}
}