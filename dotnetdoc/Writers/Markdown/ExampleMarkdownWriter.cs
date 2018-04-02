using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace dotnetdoc.Writers.Markdown
{
	internal sealed class ExampleMarkdownWriter
		: IExampleWriter
		, IMarkdownWriter
	{
		private readonly string _name;
		private readonly List<ICodeSnippetWriter> _subWriters;

		public ExampleMarkdownWriter(string name)
		{
			_name = name;
			_subWriters = new List<ICodeSnippetWriter>();
		}

		public ICodeSnippetWriter AddCodeSnippet(string language)
		{
			var writer =  new CodeSnippetMarkdownWriter(language);
			_subWriters.Add(writer);
			return writer;
		}

		public void AddImage(string description, string relativePath)
		{
			_subWriters.Add(new MarkdownImageWriter(description, relativePath));
		}

		public void RenderTo(TextWriter textWriter)
		{
			textWriter.WriteLine("### {0}", _name);
			textWriter.WriteLine();

			foreach (var subWriter in _subWriters)
			{
				subWriter.RenderTo(textWriter);
			}

			if (_subWriters.Any())
				textWriter.WriteLine();
		}
	}
}