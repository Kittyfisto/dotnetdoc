using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using dotnetdoc.Creators;
using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;

namespace dotnetdoc
{
	/// <summary>
	/// </summary>
	public sealed class Doc
		: IDisposable
	{
		private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		private readonly Assembly _assembly;
		private readonly Task<AssemblyDocumentationCreator> _creator;
		private readonly Thread _wpfThread;
		private Dispatcher _dispatcher;
		private ConsoleAppender _consoleAppender;
		private Hierarchy _hierarchy;

		/// <summary>
		/// </summary>
		/// <param name="assembly"></param>
		public Doc(Assembly assembly)
			: this(assembly, resourceDictionaryUri: null)
		{
		}

		/// <summary>
		/// </summary>
		/// <param name="assembly"></param>
		/// <param name="resourceDictionaryUri"></param>
		public Doc(Assembly assembly, string resourceDictionaryUri)
		{
			if (assembly == null)
				throw new ArgumentNullException(nameof(assembly));

			EnableLog4Net();

			_assembly = assembly;

			if (resourceDictionaryUri != null)
			{
				var creationTask = new TaskCompletionSource<AssemblyDocumentationCreator>();

				_creator = creationTask.Task;

				_wpfThread = new Thread(() => WpfLoop(creationTask, resourceDictionaryUri))
				{
					IsBackground = true
				};
				_wpfThread.SetApartmentState(ApartmentState.STA);
				_wpfThread.Start();
			}
			else
			{
				var creator = new AssemblyDocumentationCreator(_assembly);
				_creator = Task.FromResult(creator);
			}
		}

		/// <inheritdoc />
		public void Dispose()
		{
			DisableLog4Net();

			_dispatcher?.BeginInvokeShutdown(DispatcherPriority.Normal);
			if (_wpfThread?.Join(TimeSpan.FromSeconds(value: 5)) == false)
				Log.WarnFormat("Thread '{0}' did not stop in time, continuing anyways...", _wpfThread);
		}

		/// <summary>
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public ITypeDocumentationCreator<T> CreateDocumentationFor<T>()
		{
			Log.InfoFormat("Creating documentation for {0}...", typeof(T).FullName);

			return _creator.Result.CreateDocumentationFor<T>();
		}

		/// <summary>
		///     Returns an object with which the documentation for a particular control can be created.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public IControlDocumentationCreator<T> CreateDocumentationForFrameworkElement<T>() where T : FrameworkElement, new()
		{
			Log.InfoFormat("Creating documentation for {0}...", typeof(T).FullName);

			return _creator.Result.CreateDocumentationForFrameworkElement<T>();
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
		/// <exception cref="ArgumentNullException"></exception>
		public void RenderTo(IFilesystem filesystem, string basePath)
		{
			if (filesystem == null)
				throw new ArgumentNullException(nameof(filesystem));
			if (basePath == null)
				throw new ArgumentNullException(nameof(basePath));

			_creator.Result.RenderTo(filesystem, basePath);
		}

		private void EnableLog4Net()
		{
			_hierarchy = (Hierarchy)LogManager.GetRepository();

			PatternLayout patternLayout = new PatternLayout
			{
				ConversionPattern = "%date - %message%newline"
			};
			patternLayout.ActivateOptions();

			_consoleAppender = new ConsoleAppender
			{
				Layout = patternLayout
			};
			_consoleAppender.ActivateOptions();

			_hierarchy.Root.AddAppender(_consoleAppender);
			_hierarchy.Root.Level = Level.Info;
			_hierarchy.Configured = true;
		}

		private void DisableLog4Net()
		{
			_hierarchy.Root.RemoveAppender(_consoleAppender);
		}

		private void WpfLoop(TaskCompletionSource<AssemblyDocumentationCreator> creationTask, string resourceDictionaryUri)
		{
			try
			{
				var application = new WpfApplication();
				_dispatcher = application.Dispatcher;
				var resourceDictionary = new ResourceDictionary
				{
					Source = new Uri(resourceDictionaryUri, UriKind.RelativeOrAbsolute)
				};
				var creator = new AssemblyDocumentationCreator(_assembly, _dispatcher, resourceDictionary);
				creationTask.SetResult(creator);
				application.Run();
			}
			catch (Exception e)
			{
				// Just in case we haven't set the result above,
				// we try to set the exception in case the ctor failed...
				creationTask.TrySetException(e);

				Log.ErrorFormat("Caught unexpected exception: {0}", e);
			}
		}

		private sealed class WpfApplication
			: Application
		{
		}
	}
}