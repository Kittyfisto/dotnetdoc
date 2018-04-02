using System.IO;

namespace dotnetdoc.Writers.Markdown
{
	internal sealed class MarkdownImageWriter
		: IMarkdownWriter
	{
		private readonly string _description;
		private readonly string _relativeImagePath;

		public MarkdownImageWriter(string description, string relativeImagePath)
		{
			_description = description;
			_relativeImagePath = relativeImagePath;
		}

		public void RenderTo(TextWriter textWriter)
		{
			textWriter.WriteLine("![{0}]({1})", _description, _relativeImagePath);
		}
	}
}