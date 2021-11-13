using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Media.Imaging;
using dotnetdoc.Creators;
using dotnetdoc.WpfLibrary;
using dotnetdoc.Writers.Markdown;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace dotnetdoc.Test.Creators
{
	[TestFixture]
	[RequiresThread(ApartmentState.STA)]
	public sealed class ControlDocumentationCreatorTest
	{
		private const string RootDirectory = @"M:\documentation";

		private InMemoryFilesystem _filesystem;

		[SetUp]
		public void Setup()
		{
			_filesystem = new InMemoryFilesystem();
		}

		[Test]
		public void TestAddExampleImage()
		{
			var creator = Create<SomeButton>();
			var example = creator.AddExample("Stuff");
			example.Resize(32, 32);

			Render(creator);

			var screenshots = GetScreenshots();
			screenshots.Should().HaveCount(1, "because one screenshot should've been created");
			screenshots[0].Should().EndWith("Stuff.png");
		}

		[Test]
		public void TestAddImage()
		{
			var creator = Create<SomeButton>();
			var name = creator.AddImage(new BitmapImage(), "40%");
			name.Should().NotContain("%", "because special characters should've been stripped from the actual filename");
		}

		private IReadOnlyList<string> GetScreenshots()
		{
			return _filesystem.EnumerateFiles(RootDirectory, "*.png", SearchOption.AllDirectories);
		}

		private void Render<T>(ControlDocumentationCreator<T> creator) where T : FrameworkElement, new()
		{
			var reader = new Mock<IAssemblyDocumentationReader>();
			var writer = new TypeDocumentationMarkdownWriter(reader.Object, typeof(T));
			creator.RenderTo(_filesystem, RootDirectory, writer);
		}

		private ControlDocumentationCreator<T> Create<T>() where T : FrameworkElement, new()
		{
			var dispatcher = new DispatcherMock();
			var resourceDictionary = new ResourceDictionary();
			return new ControlDocumentationCreator<T>(dispatcher,
			                                          resourceDictionary);
		}
	}
}
