using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using dotnetdoc.Creators;
using dotnetdoc.WpfLibrary;
using FluentAssertions;
using NUnit.Framework;

namespace dotnetdoc.Test.Creators
{
	[TestFixture]
	public sealed class ControlDocumentationCreatorTest
	{
		private const string RootDirectory = @"M:\documentation";

		private Doc _doc;
		private InMemoryFilesystem _filesystem;

		[SetUp]
		public void Setup()
		{
			_filesystem = new InMemoryFilesystem();
			_doc = new Doc(typeof(SomeButton).Assembly, "/dotnetdoc.WpfLibrary;component/Themes/Generic.xaml");
		}

		[TearDown]
		public void Teardown()
		{
			_doc.Dispose();
		}

		[Test]
		public void TestAddExampleImage()
		{
			var creator = Create<SomeButton>();
			var example = creator.AddExample("Stuff");
			example.Resize(32, 32);

			Render();

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
			return _filesystem.EnumerateFiles(RootDirectory, "*.png", SearchOption.AllDirectories).Result;
		}

		private void Render()
		{
			_doc.RenderTo(_filesystem, RootDirectory);
		}

		private ControlDocumentationCreator<T> Create<T>() where T : FrameworkElement, new()
		{
			return (ControlDocumentationCreator<T>) _doc.CreateDocumentationForFrameworkElement<T>();
		}
	}
}
