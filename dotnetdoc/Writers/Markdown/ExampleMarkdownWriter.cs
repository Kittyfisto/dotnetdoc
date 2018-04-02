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
		private readonly List<IMarkdownWriter> _subWriters;

		public ExampleMarkdownWriter(string name)
		{
			_name = name;
			_subWriters = new List<IMarkdownWriter>();
		}

		public TextWriter AddCodeSnippet(string language)
		{
			var writer =  new CodeSnippetMarkdownWriter(language);
			_subWriters.Add(writer);
			return writer;
		}

		public void AddImage(string description, string relativePath)
		{
			_subWriters.Add(new MarkdownImageWriter(description, relativePath));
		}

		public void WriteTo(TextWriter textWriter)
		{
			textWriter.WriteLine("### {0}", _name);
			textWriter.WriteLine();

			foreach (var subWriter in _subWriters)
			{
				subWriter.WriteTo(textWriter);
			}

			if (_subWriters.Any())
				textWriter.WriteLine();
		}
	}
}