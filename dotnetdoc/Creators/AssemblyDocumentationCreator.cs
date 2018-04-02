using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using dotnetdoc.Writers.Markdown;
using log4net;

namespace dotnetdoc.Creators
{
	/// <summary>
	///     Responsible for creating documentation for a library.
	/// </summary>
	public sealed class AssemblyDocumentationCreator
	{
		private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private readonly IAssemblyDocumentationReader _assemblyDocumentationReader;

		private readonly Dispatcher _dispatcher;
		private readonly ResourceDictionary _resourceDictionary;
		private readonly List<ITypeDocumentationCreator> _types;

		/// <summary>
		/// Initializes this class.
		/// </summary>
		/// <param name="assembly"></param>
		public AssemblyDocumentationCreator(Assembly assembly)
			: this(assembly, null, null)
		{
		}

		/// <summary>
		/// Initializes this class.
		/// </summary>
		/// <param name="assembly"></param>
		/// <param name="dispatcher"></param>
		/// <param name="resourceDictionary"></param>
		public AssemblyDocumentationCreator(Assembly assembly, Dispatcher dispatcher, ResourceDictionary resourceDictionary)
		{
			if (assembly == null)
				throw new ArgumentNullException(nameof(assembly));

			_dispatcher = dispatcher;
			_resourceDictionary = resourceDictionary;
			_types = new List<ITypeDocumentationCreator>();
			_assemblyDocumentationReader = GetAssemblyDocumentation(assembly);
		}

		private static IAssemblyDocumentationReader GetAssemblyDocumentation(Assembly assembly)
		{
			try
			{
				return new AssemblyDocumentationReader(assembly);
			}
			catch (Exception e)
			{
				Log.ErrorFormat("Unable to retrieve documentation of assembly '{0}':\n{1}",
					assembly,
					e);
				return new EmptyAssemblyDocumentationReader();
			}
		}

		/// <summary>
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public ITypeDocumentationCreator<T> CreateDocumentationFor<T>()
		{
			var creator = new TypeDocumentationCreator<T>();
			_types.Add(creator);
			return creator;
		}

		/// <summary>
		///     Returns an object with which the documentation for a particular control can be created.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public IControlDocumentationCreator<T> CreateDocumentationForFrameworkElement<T>() where T : FrameworkElement, new()
		{
			var creator = new ControlDocumentationCreator<T>(_dispatcher, _resourceDictionary);
			_types.Add(creator);
			return creator;
		}

		/// <summary>
		///     Renders the documentation to the given path.
		/// </summary>
		/// <param name="basePath"></param>
		public void RenderTo(string basePath)
		{
			using (var scheduler = new SerialTaskScheduler())
			{
				var filesystem = new Filesystem(scheduler);
				RenderTo(filesystem, basePath);
			}
		}

		/// <summary>
		///     Renders the documentation to the given basepath of the given filesystem.
		/// </summary>
		/// <param name="filesystem"></param>
		/// <param name="basePath"></param>
		public void RenderTo(IFilesystem filesystem, string basePath)
		{
			var tasks = new List<Task>();

			foreach (var typeCreator in _types)
			{
				var documentationWriter = new TypeDocumentationMarkdownWriter(_assemblyDocumentationReader, typeCreator.Type);
				
				var directory = Path.Combine(basePath, typeCreator.Type.FullName);
				var fileName = Path.Combine(directory, "README.md");
				typeCreator.RenderTo(filesystem, directory, documentationWriter);

				using (var stream = new MemoryStream())
				using (var streamWriter = new StreamWriter(stream))
				{
					documentationWriter.WriteTo(streamWriter);
					streamWriter.Flush();

					stream.Position = 0;
					tasks.Add(WriteDocumentationTo(filesystem, fileName, stream));
				}
			}

			Task.WaitAll(tasks.ToArray());
		}

		private static async Task WriteDocumentationTo(IFilesystem filesystem, string fileName,
			Stream serializedDocumentation)
		{
			using (var fileStream = await filesystem.CreateFile(fileName))
			{
				serializedDocumentation.CopyTo(fileStream);
			}
		}
	}
}