using System.IO;

namespace dotnetdoc.Writers
{
	/// <summary>
	///     Responsible for writing example to a target stream.
	/// </summary>
	public interface IExampleWriter
	{
		/// <summary>
		///     Adds a new code snippet to the example being written.
		/// </summary>
		/// <param name="language"></param>
		/// <returns></returns>
		TextWriter AddCodeSnippet(string language);

		/// <summary>
		///     Embedds an image in the example.
		/// </summary>
		/// <param name="description"></param>
		/// <param name="relativePath"></param>
		void AddImage(string description, string relativePath);
	}
}