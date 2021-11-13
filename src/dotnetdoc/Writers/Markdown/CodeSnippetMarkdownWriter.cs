using System.IO;

namespace dotnetdoc.Writers.Markdown
{
	internal sealed class CodeSnippetMarkdownWriter
		: StringWriter
		, ICodeSnippetWriter
		, IMarkdownWriter
	{
		private readonly string _language;

		public CodeSnippetMarkdownWriter(string language)
		{
			_language = language;
		}

		public void WriteTo(TextWriter writer)
		{
			writer.WriteLine("```{0}", _language);
			writer.WriteLine(ToString());
			writer.WriteLine("```");
		}
	}
}