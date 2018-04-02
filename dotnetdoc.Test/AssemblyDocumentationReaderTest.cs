using FluentAssertions;
using NUnit.Framework;

namespace dotnetdoc.Test
{
	/// <summary>
	/// 
	/// </summary>
	[TestFixture]
	public sealed class AssemblyDocumentationReaderTest
	{
		/// <summary>
		/// 
		/// </summary>
		[Test]
		public void TestParseOneType()
		{
			var reader = new AssemblyDocumentationReader(typeof(AssemblyDocumentationReader).Assembly);
			var doc = reader.GetDocumentationOf<AssemblyDocumentationReader>();
			doc.Should().NotBeNull();
			doc.FullTypeName.Should().Be("dotnetdoc.AssemblyDocumentationReader");
			doc.Type.Should().Be<AssemblyDocumentationReader>();
			doc.Summary.Should().Be("Responsible for reading the documentation of a .NET assembly (which resides in a similarly named xml file,\n                usually).");
		}
	}
}