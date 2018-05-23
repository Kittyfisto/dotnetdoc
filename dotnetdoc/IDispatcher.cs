using System;
using System.Windows.Threading;

namespace dotnetdoc
{
	internal interface IDispatcher
	{
		void Invoke(Action fn);
		void Invoke(Action action, DispatcherPriority priority);
	}
}