﻿using System.Windows;

namespace dotnetdoc
{
	/// <summary>
	///     Responsible for creating the documentation for a particular <see cref="FrameworkElement" />.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IControlDocumentationCreator<out T>
		: ITypeDocumentationCreator<T>
		where T : FrameworkElement
	{
		/// <summary>
		///     Adds a new example which shows how to use the <see cref="T"/> <see cref="FrameworkElement"/>.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		IFrameworkElementExampleCreator<T> AddExample(string name);
	}
}