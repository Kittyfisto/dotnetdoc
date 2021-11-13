using System.IO;

namespace dotnetdoc.Writers.Markdown
{
	internal interface IMarkdownWriter
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="writer"></param>
		void WriteTo(TextWriter writer);
	}
}