﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using dotnetdoc.Writers;
using log4net;

namespace dotnetdoc.Creators
{
	/// <summary>
	///     Responsible for providing the documentation for a particular control.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	internal sealed class ControlDocumentationCreator<T>
		: IInternalControlDocumentationCreator<T>
		where T : FrameworkElement, new()
	{
		private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		private readonly IDispatcher _dispatcher;
		private readonly List<ITypeExampleCreator> _examples;
		private readonly ResourceDictionary _resourceDictionary;
		private readonly Dictionary<string, BitmapSource> _snapshots;

		public ControlDocumentationCreator(IDispatcher dispatcher,
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
			Log.InfoFormat("Rendering '{0}'...", typeof(T).FullName);

			foreach (var example in _examples)
			{
				var exampleWriter = writer.AddExample(example.Name);
				example.RenderTo(exampleWriter);
			}

			foreach (var pair in _snapshots)
			{
				var relativeImagePath = pair.Key;
				var bitmap = pair.Value;

				var destination = Path.Combine(path, relativeImagePath);
				SaveSnapshotAsync(filesystem, bitmap, destination);
			}
		}

		public string AddImage(BitmapSource screenshot, string name)
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
				'\n',
				'%'
			};
			foreach (var invalidCharacter in invalidChars) builder.Replace(invalidCharacter, '_');
			return builder.ToString();
		}

		private static void SaveSnapshotAsync(IFilesystem filesystem, BitmapSource screenshot, string destination)
		{
			var encoder = new PngBitmapEncoder();
			using (var stream = new MemoryStream())
			{
				var frame = BitmapFrame.Create(screenshot);
				encoder.Frames.Add(frame);
				encoder.Save(stream);

				stream.Position = 0;

				var destinationDirectory = Path.GetDirectoryName(destination);
				filesystem.CreateDirectory(destinationDirectory);
				using (var fileStream = filesystem.OpenWrite(destination))
				{
					stream.CopyTo(fileStream);
				}
			}
		}
	}
}