using System.IO;
using System.Threading;
using dotnetdoc.Creators;
using dotnetdoc.TestTypes;
using dotnetdoc.Writers.Markdown;
using FluentAssertions;
using NUnit.Framework;

namespace dotnetdoc.Test.Creators
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

		[Test]
		public void TestDocumentTypeWithSee()
		{
			var doc = new TypeDocumentationCreator<TypeWithSee>();
			var result = Render(doc);
			result.Should().Contain("This class is similar to dotnetdoc.TestTypes.EmptyType");
			result.Should().Contain("Related to System.Double.");
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