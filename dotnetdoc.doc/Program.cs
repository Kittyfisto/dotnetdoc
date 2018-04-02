using System.IO;
using System.Reflection;
using dotnetdoc.Creators;

namespace dotnetdoc.doc
{
	class Program
	{
		static void Main(string[] args)
		{
			var assemblyDocumentationCreator = new AssemblyDocumentationCreator(typeof(AssemblyDocumentationCreator).Assembly);

			var documentationCreator = assemblyDocumentationCreator.CreateDocumentationFor<AssemblyDocumentationCreator>();
			var example = documentationCreator.AddExample("", "This example shows you how to create documentation for a single type");
			//example.AddCodeSnippetFromMethod(typeof(Program).GetMethod(nameof(Main)));
			var snippet = example.AddCodeSnippet();
			snippet.Add(() => new AssemblyDocumentationCreator(Assembly.GetCallingAssembly()));

			assemblyDocumentationCreator.RenderTo(Directory.GetCurrentDirectory());
		}
	}
}
