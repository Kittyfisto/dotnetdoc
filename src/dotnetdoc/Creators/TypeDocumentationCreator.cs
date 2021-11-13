using System;
using System.Collections.Generic;
using System.IO;
using dotnetdoc.Writers;

namespace dotnetdoc.Creators
{
	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="T"></typeparam>
	internal sealed class TypeDocumentationCreator<T>
		: ITypeDocumentationCreator<T>
	{
		private readonly List<ITypeExampleCreator> _creators;

		public TypeDocumentationCreator()
		{
			_creators = new List<ITypeExampleCreator>();
		}

		public ITypeExampleCreator<T> AddExample(string name, string description)
		{
			var creator = new TypeExampleCreator<T>(name, description);
			_creators.Add(creator);
			return creator;
		}

		public Type Type => typeof(T);

		public void RenderTo(IFilesystem filesystem, string path, ITypeDocumentationWriter writer)
		{
			foreach (var creator in _creators)
			{
				var exampleWriter = writer.AddExample(creator.Name);
				creator.RenderTo(exampleWriter);
			}
		}
	}
}