using System;
using System.Windows.Threading;

namespace dotnetdoc.Test
{
	internal sealed class DispatcherMock
		: IDispatcher
	{
		#region Implementation of IDispatcher

		public void Invoke(Action fn)
		{
			fn();
		}

		public void Invoke(Action action, DispatcherPriority priority)
		{
			action();
		}

		#endregion
	}
}
