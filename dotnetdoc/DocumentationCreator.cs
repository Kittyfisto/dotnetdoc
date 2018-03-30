using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;

namespace dotnetdoc
{
	/// <summary>
	///     Responsible for creating documentation for a library.
	/// </summary>
	public sealed class DocumentationCreator
	{
		private readonly Dispatcher _dispatcher;
		private readonly ResourceDictionary _resourceDictionary;
		private readonly List<ITypeDocumentationCreator> _controlDocumentationCreators;
		private readonly AssemblyDocumentationReader _assemblyDocumentationReader;
		private readonly string _basePath;

		public DocumentationCreator(Dispatcher dispatcher, ResourceDictionary resourceDictionary, Assembly assembly, string basePath)
		{
			if (dispatcher == null)
				throw new ArgumentNullException(nameof(dispatcher));
			if (resourceDictionary == null)
				throw new ArgumentNullException(nameof(resourceDictionary));
			if (assembly == null)
				throw new ArgumentNullException(nameof(assembly));

			_dispatcher = dispatcher;
			_resourceDictionary = resourceDictionary;
			_basePath = basePath;
			_controlDocumentationCreators = new List<ITypeDocumentationCreator>();
			_assemblyDocumentationReader = new AssemblyDocumentationReader(assembly);
		}

		/// <summary>
		///     Returns an object with which the documentation for a particular control can be created.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public IControlDocumentationCreator<T> CreateDocumentationFor<T>() where T : FrameworkElement, new()
		{
			var creator = new ControlDocumentationCreator<T>(_dispatcher, _resourceDictionary, _assemblyDocumentationReader, _basePath);
			_controlDocumentationCreators.Add(creator);
			return creator;
		}
	}
}