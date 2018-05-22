using dotnetdoc.TestTypes;
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
			doc.Summary.Should().Be("Responsible for reading the documentation of a .NET assembly (which resides in a similarly named xml file,\r\n                usually).");
		}

		[Test]
		public void TestParseWithXmlAttributes()
		{
			var reader = new AssemblyDocumentationReader(typeof(TypeWithSee).Assembly);
			var doc = reader.GetDocumentationOf<TypeWithSee>();
			doc.Summary.Should().Be("This class is similar to <see cref=\"T:dotnetdoc.TestTypes.EmptyType\" />.");
			doc.Remarks.Should().HaveCount(1);
			doc.Remarks[0].Should().Be("Related to <see cref=\"T:System.Double\" />.");
		}
	}
}