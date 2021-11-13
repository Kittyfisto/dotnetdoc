using System.IO;

namespace dotnetdoc.Writers
{
	/// <summary>
	///     Responsible for writing the documentation for a single type.
	/// </summary>
	public interface ITypeDocumentationWriter
	{
		/// <summary>
		///     Adds an example to the documentation for this type.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		IExampleWriter AddExample(string name);

		/// <summary>
		///     Serializes the documentation in the given writer.
		/// </summary>
		/// <param name="textWriter"></param>
		void WriteTo(TextWriter textWriter);
	}
}