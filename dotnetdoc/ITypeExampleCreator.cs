using System.Reflection;
using dotnetdoc.Writers;

namespace dotnetdoc
{
	/// <summary>
	///     Responsible for creating examples on how to use a particular type.
	/// </summary>
	/// <remarks>
	///     An example should cover one scenario only and consist of only a few code snippets (ideally just one).
	/// </remarks>
	public interface ITypeExampleCreator
	{
		/// <summary>
		///     The name of the example, will be displayed to the user.
		/// </summary>
		string Name { get; }

		/// <summary>
		/// </summary>
		/// <param name="method"></param>
		/// <returns></returns>
		void AddCodeSnippetFromMethod(MethodInfo method);

		/// <summary>
		///     Renders the contents of this creator to the given writer.
		/// </summary>
		/// <param name="writer"></param>
		void RenderTo(IExampleWriter writer);
	}

	/// <summary>
	///     Responsible for creating examples on how to use the type <typeparamref name="T" />.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface ITypeExampleCreator<T>
		: ITypeExampleCreator
	{
		/// <summary>
		///     Adds a new code snippet to this example.
		/// </summary>
		/// <returns></returns>
		ICodeSnippetCreator<T> AddCodeSnippet();
	}
}