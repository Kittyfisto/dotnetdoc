using System;
using System.Collections.Generic;
using System.IO;

namespace dotnetdoc.Writers.Markdown
{
	/// <summary>
	///     Responsible for writing the markdown documentation for a single control.
	///     Extracts information from the XML documentation which accompanies a .NET assembly,
	///     but can be enhanced by examples, images, etc...
	/// </summary>
	internal sealed class TypeDocumentationMarkdownWriter
		: ITypeDocumentationWriter
	{
		private readonly IAssemblyDocumentationReader _assemblyDocumentationReader;
		private readonly Type _type;
		private readonly TypeDocumentation _typeDocumentation;
		private readonly List<IMarkdownWriter> _subWriters;

		public TypeDocumentationMarkdownWriter(IAssemblyDocumentationReader assemblyDocumentationReader, Type type)
		{
			if (assemblyDocumentationReader == null)
				throw new ArgumentNullException(nameof(assemblyDocumentationReader));

			_assemblyDocumentationReader = assemblyDocumentationReader;
			_type = type;
			_typeDocumentation = assemblyDocumentationReader.GetDocumentationOf(type);
			_subWriters = new List<IMarkdownWriter>();
		}

		public IExampleWriter AddExample(string name)
		{
			var writer = new ExampleMarkdownWriter(name);
			_subWriters.Add(writer);
			return writer;
		}

		public void WriteTo(TextWriter textWriter)
		{
			textWriter.WriteHeader(_type.Name);
			textWriter.WriteSummary(_typeDocumentation);
			textWriter.WriteRemarks(_typeDocumentation);

			foreach (var subWriter in _subWriters)
			{
				subWriter.WriteTo(textWriter);
			}

			if (_typeDocumentation != null)
			{
				textWriter.WriteHeader("Properties");
				foreach (var property in _typeDocumentation.Properties)
				{
					textWriter.WriteLine("**{0}**: {1}  ", property.PropertyName, property.PropertyType);
					textWriter.WriteLine(TextWriterExtensions.Format(property.Summary));
					textWriter.WriteLine();
				}
			}
		}
	}
}