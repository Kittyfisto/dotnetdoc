using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
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
			textWriter.WriteLine("**Namespace**: {0}  ", _type.Namespace);
			textWriter.WriteLine("**Assembly**: {0} (in {0}.dll)  ", _type.Assembly.GetName().Name);
			textWriter.WriteLine();

			CreateTypeDefinition(_type).WriteTo(textWriter);

			textWriter.WriteLine();

			textWriter.WriteLine("Inheritance {0}", string.Join(" -> ", GetInheritance(_type)));

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

		[Pure]
		private static CodeSnippetMarkdownWriter CreateTypeDefinition(Type type)
		{
			var typeDefinitionWriter = new CodeSnippetMarkdownWriter("C#");
			var attributes = type.GetCustomAttributes(false);

			foreach (var attribute in attributes)
			{
				var attributeType = attribute.GetType();
				var attributeName = attributeType.FullName;
				if (attributeName.EndsWith("Attribute"))
					attributeName = attributeName.Substring(0, attributeName.Length - 9);
				typeDefinitionWriter.WriteLine("[{0}]", attributeName);
			}

			if (type.IsPublic)
				typeDefinitionWriter.Write("public ");

			if (type.IsSealed && !type.IsEnum)
				typeDefinitionWriter.Write("sealed ");
			else if (type.IsAbstract)
				typeDefinitionWriter.Write("abstract ");

			if (type.IsClass)
				typeDefinitionWriter.Write("class ");
			else if (type.IsEnum)
				typeDefinitionWriter.Write("enum ");
			else
				typeDefinitionWriter.Write("struct ");

			typeDefinitionWriter.Write("{0} ", type.Name);

			var @base = type.BaseType;
			if (type != typeof(Object))
				typeDefinitionWriter.Write(": {0}", @base);
			return typeDefinitionWriter;
		}

		[Pure]
		private static IEnumerable<string> GetInheritance(Type type)
		{
			var inheritance = new List<string>();
			while (type != typeof(Object) && type != null)
			{
				inheritance.Add(type.Name);
				type = type.BaseType;
			}

			if (type != null)
				inheritance.Add(type.Name);

			inheritance.Reverse();
			return inheritance;
		}
	}
}