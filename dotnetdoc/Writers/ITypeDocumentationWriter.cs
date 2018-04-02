using System.IO;
using System.Windows;

namespace dotnetdoc.Writers
{
	/// <summary>
	///     Responsible for writing the documentation for a single type.
	/// </summary>
	public interface ITypeDocumentationWriter
	{
		IExampleWriter AddExample(string name);

		void WriteTo(TextWriter textWriter);
	}
}