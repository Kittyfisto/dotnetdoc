using System;
using System.Windows;

namespace dotnetdoc
{
	/// <summary>
	///     Responsible for creating example code to show how a particular <see cref="FrameworkElement"/> is to be used.
	/// </summary>
	public interface IFrameworkElementExampleCreator
		: IDisposable
	{
		/// <summary>
		///     Set a property to the given value.
		/// </summary>
		/// <param name="property"></param>
		/// <param name="value"></param>
		void SetValue(DependencyProperty property, object value);

		/// <summary>
		/// </summary>
		/// <param name="width"></param>
		/// <param name="height"></param>
		void Resize(int width, int height);

		/// <summary>
		/// </summary>
		void Focus();

		/// <summary>
		/// </summary>
		/// <param name="timeout"></param>
		void Wait(TimeSpan timeout);
	}

	/// <summary>
	///     Responsible for creating example code to show how a particular <see cref="FrameworkElement"/> is to be used.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IFrameworkElementExampleCreator<out T>
		: IFrameworkElementExampleCreator where T : FrameworkElement
	{
		/// <summary>
		/// </summary>
		/// <param name="action"></param>
		void Prepare(Action<T> action);
	}
}