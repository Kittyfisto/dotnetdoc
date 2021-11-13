using System.IO;
using System.Reflection;

namespace dotnetdoc.doc
{
	class Program
	{
		static void Main(string[] args)
		{
			var targetAssembly = typeof(Doc).Assembly;
			var assemblyDocumentationCreator = new Doc(targetAssembly);

			var documentationCreator = assemblyDocumentationCreator.CreateDocumentationFor<Doc>();
			var example = documentationCreator.AddExample("", "This example shows you how to create documentation for a single type");
			//example.AddCodeSnippetFromMethod(typeof(Program).GetMethod(nameof(Main)));
			var snippet = example.AddCodeSnippet();
			snippet.Add(() => new Doc(Assembly.GetCallingAssembly()));

			assemblyDocumentationCreator.RenderTo(Directory.GetCurrentDirectory());
		}
	}
}
