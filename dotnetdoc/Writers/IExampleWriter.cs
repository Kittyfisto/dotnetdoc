using dotnetdoc.Writers.Markdown;

namespace dotnetdoc.Writers
{
	public interface IExampleWriter
	{
		ICodeSnippetWriter AddCodeSnippet(string language);
		void AddImage(string description, string relativePath);
	}
}