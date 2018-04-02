using System.IO;

namespace dotnetdoc.Writers.Markdown
{
	internal sealed class CodeSnippetMarkdownWriter
		: StringWriter, ICodeSnippetWriter
	{
		private readonly string _language;

		public CodeSnippetMarkdownWriter(string language)
		{
			_language = language;
		}

		public void RenderTo(TextWriter textWriter)
		{
			textWriter.WriteLine("```{0}", _language);
			textWriter.Write(ToString());
			textWriter.WriteLine("```");
		}
	}
}