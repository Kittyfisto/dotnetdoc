using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using dotnetdoc.Writers;

namespace dotnetdoc.Creators
{
	/// <summary>
	///     Responsible for providing the documentation for a particular control.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	internal sealed class ControlDocumentationCreator<T>
		: IControlDocumentationCreator<T>
		where T : FrameworkElement, new()
	{
		private const string DocumentationFolderName = "Documentation";
		private readonly Dispatcher _dispatcher;
		private readonly List<ITypeExampleCreator> _examples;
		private readonly ResourceDictionary _resourceDictionary;
		private readonly Dictionary<string, BitmapSource> _snapshots;

		public ControlDocumentationCreator(Dispatcher dispatcher,
			ResourceDictionary resourceDictionary)
		{
			if (dispatcher == null)
				throw new ArgumentNullException(nameof(dispatcher));
			if (resourceDictionary == null)
				throw new ArgumentNullException(nameof(resourceDictionary));

			_dispatcher = dispatcher;
			_resourceDictionary = resourceDictionary;
			_snapshots = new Dictionary<string, BitmapSource>();
			_examples = new List<ITypeExampleCreator>();
		}

		[Pure]
		public IFrameworkElementExampleCreator<T> AddExample(string name)
		{
			if (name == null)
				throw new ArgumentNullException(nameof(name));

			var exampleCreator = new FrameworkElementExampleCreator<T>(this, _dispatcher, _resourceDictionary, name);
			_examples.Add(exampleCreator);
			return exampleCreator;
		}

		public ITypeExampleCreator<T> AddExample(string name, string description)
		{
			throw new NotImplementedException();
		}

		public Type Type => typeof(T);

		public void RenderTo(IFilesystem filesystem, string path, ITypeDocumentationWriter writer)
		{
			foreach (var example in _examples)
			{
				foreach (var pair in _snapshots)
				{
					var relativeImagePath = pair.Key;
					var bitmap = pair.Value;

					var destination = Path.Combine(path, relativeImagePath);
					SaveSnapshot(bitmap, destination);
				}

				var exampleWriter = writer.AddExample(example.Name);
				example.RenderTo(exampleWriter);
			}
		}

		internal string AddImage(BitmapSource screenshot, string name)
		{
			var actualName = CoerceImageName(name);

			var relativeImagePath = string.Format("{0}.png", actualName);
			_snapshots.Add(relativeImagePath, screenshot);
			return relativeImagePath;
		}

		private static string CoerceImageName(string name)
		{
			var builder = new StringBuilder(name);
			var invalidChars = new[]
			{
				',',
				' ',
				'@',
				':',
				'?',
				'!',
				')',
				'(',
				'/',
				'\\',
				'\r',
				'\n'
			};
			foreach (var invalidCharacter in invalidChars) builder.Replace(invalidCharacter, '_');
			return builder.ToString();
		}

		private static void SaveSnapshot(BitmapSource screenshot, string destination)
		{
			var encoder = new PngBitmapEncoder();
			using (var stream = new MemoryStream())
			{
				var frame = BitmapFrame.Create(screenshot);
				encoder.Frames.Add(frame);
				encoder.Save(stream);

				stream.Position = 0;

				var destinationDirectory = Path.GetDirectoryName(destination);
				Directory.CreateDirectory(destinationDirectory);
				using (var fileStream = File.OpenWrite(destination))
				{
					stream.CopyTo(fileStream);
				}
			}
		}
	}
}