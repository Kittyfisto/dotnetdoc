using System;
using System.Windows.Threading;

namespace dotnetdoc
{
	internal sealed class DispatcherProxy
		: IDispatcher
	{
		private readonly Dispatcher _dispatcher;

		public DispatcherProxy(Dispatcher dispatcher)
		{
			_dispatcher = dispatcher;
		}

		#region Implementation of IDispatcher

		public void Invoke(Action fn)
		{
			_dispatcher.Invoke(fn);
		}

		public void Invoke(Action action, DispatcherPriority priority)
		{
			_dispatcher.Invoke(action, priority);
		}

		#endregion
	}
}