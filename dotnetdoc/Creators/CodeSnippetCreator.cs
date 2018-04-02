using System;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using dotnetdoc.Writers;

namespace dotnetdoc.Creators
{
	internal sealed class CodeSnippetCreator<T>
		: ICodeSnippetCreator<T>
	{
		private readonly TextWriter _writer;

		public CodeSnippetCreator()
		{
			_writer = new StringWriter();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="func"></param>
		public void Add(Expression<Func<AssemblyDocumentationCreator>> func)
		{
			_writer.WriteLine(func.ToString());
		}

		public void AddFromMethod(MethodInfo method)
		{
			throw new NotImplementedException();
		}

		public void RenderTo(IExampleWriter writer)
		{
			writer.AddCodeSnippet("C#").WriteLine(_writer);
		}
	}
}