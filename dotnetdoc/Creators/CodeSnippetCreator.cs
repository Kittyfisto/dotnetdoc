using System;
using System.IO;
using System.Linq.Expressions;
using dotnetdoc.Writers.Markdown;

namespace dotnetdoc.Creators
{
	internal sealed class CodeSnippetCreator<T>
		: ICodeSnippetCreator<T>
	{
		private readonly CodeSnippetMarkdownWriter _writer;

		public CodeSnippetCreator()
		{
			_writer = new CodeSnippetMarkdownWriter("C#");
		}

		public void AddFromMethod(Type type, string mainName)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="func"></param>
		public void Add(Expression<Func<AssemblyDocumentationCreator>> func)
		{
			_writer.WriteLine(func.ToString());
		}

		public void RenderTo(TextWriter writer)
		{
			_writer.RenderTo(writer);
		}
	}
}