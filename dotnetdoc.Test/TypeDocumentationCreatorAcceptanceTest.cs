using System.IO;
using System.Reflection;
using System.Threading;
using dotnetdoc.Creators;
using dotnetdoc.Test.TestTypes;
using dotnetdoc.Writers.Markdown;
using FluentAssertions;
using NUnit.Framework;

namespace dotnetdoc.Test
{
	[TestFixture]
	public sealed class TypeDocumentationCreatorAcceptanceTest
	{
		private InMemoryFilesystem _filesystem;
		private ImmediateTaskScheduler _scheduler;
		private AssemblyDocumentationReader _documentationReader;

		[OneTimeSetUp]
		public void OneTimeSetUp()
		{
			_documentationReader = new AssemblyDocumentationReader(typeof(EmptyType).Assembly);
		}

		[SetUp]
		public void Setup()
		{
			_scheduler = new ImmediateTaskScheduler();
			_filesystem = new InMemoryFilesystem(_scheduler);
		}

		[TearDown]
		public void TearDown()
		{
			_scheduler.Dispose();
		}

		[Test]
		public void TestDocumentEmptyType()
		{
			var doc = new TypeDocumentationCreator<EmptyType>();
			Render(doc).Should().Be("# EmptyType\r\n\r\n");
		}

		private string Render<T>(ITypeDocumentationCreator<T> creator)
		{
			var writer = new TypeDocumentationMarkdownWriter(_documentationReader, typeof(T));
			var folder = typeof(T).Name;
			creator.RenderTo(_filesystem, folder, writer);
			var stringWriter = new StringWriter();
			writer.WriteTo(stringWriter);
			return stringWriter.ToString();
		}
	}
}