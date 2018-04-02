﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using dotnetdoc.Writers;

namespace dotnetdoc.Creators
{
	/// <summary>
	///     Responsible for creating the documentation for a particular control in a particular example.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	internal sealed class FrameworkElementExampleCreator<T>
		: IFrameworkElementExampleCreator<T>
		where T : FrameworkElement, new()
	{
		private readonly ControlDocumentationCreator<T> _controlDocumentationCreator;
		private readonly Dispatcher _dispatcher;
		private readonly T _element;
		private readonly string _exampleName;
		private readonly string _controlName;
		private readonly Dictionary<DependencyProperty, object> _values;

		public FrameworkElementExampleCreator(ControlDocumentationCreator<T> controlDocumentationCreator,
		                             Dispatcher dispatcher,
		                             ResourceDictionary resourceDictionary,
		                             string exampleName)
		{
			_controlDocumentationCreator = controlDocumentationCreator;
			_dispatcher = dispatcher;
			_exampleName = exampleName;
			_element = Invoke(() =>
			{
				var element = new T();
				return element;
			});
			Invoke(() =>
			{
				var style = (Style) resourceDictionary[typeof(T)];
				_element.Style = style;
			});

			_controlName = typeof(T).Name;
			_values = new Dictionary<DependencyProperty, object>();
		}

		public void Dispose()
		{
		}

		public void SetValue(DependencyProperty property, object value)
		{
			Invoke(() => { _element.SetValue(property, value); });
			_values[property] = value;
		}

		public void Resize(int width, int height)
		{
			Invoke(() =>
			{
				_element.Width = width;
				_element.Height = height;

				var size = new Size(width, height);
				_element.Measure(size);
				_element.Arrange(new Rect(new Point(), size));
			});
		}

		public void Prepare(Action<T> action)
		{
			Invoke(() => action(_element));
		}

		public void Focus()
		{
			Invoke(() =>
			{
				_element.Focusable = true;
				SetKeyboardFocused();
				_element.Focus();

				MakeFocusBorderVisible();
			});
		}

		private void MakeFocusBorderVisible()
		{
			// I'm too fucking stupid to get the fucking animation to work in this application
			// and therefore I have to cheat...
			var method = typeof(T).GetMethod("GetTemplateChild", BindingFlags.NonPublic | BindingFlags.Instance);
			var focusBorder = (Border) method.Invoke(_element, new object[] {"focusBorder"});
			focusBorder.Opacity = 1;
		}

		private void SetKeyboardFocused()
		{
			// Black magic necessary to make the control to believe that it's keyboard focused.
			// Haven't found an easier wa to make this work (and no, Keyboard.Focus() has no fucking effect).

			var field = typeof(UIElement).GetField("_flags", BindingFlags.NonPublic | BindingFlags.Instance);
			var flags = Convert.ToInt32(field.GetValue(_element));
			flags |= 0x00000400;
			flags |= 0x00000800;
			var type = typeof(UIElement).Assembly.GetType("System.Windows.CoreFlags");
			var newFlags = Enum.ToObject(type, flags);
			field.SetValue(_element, newFlags);

			var method = typeof(UIElement).GetMethod("RaiseIsKeyboardFocusWithinChanged",
			                                         BindingFlags.NonPublic | BindingFlags.Instance);
			method.Invoke(_element, new object[]
			{
				new DependencyPropertyChangedEventArgs(UIElement.IsKeyboardFocusWithinProperty, oldValue: false, newValue: true)
			});
		}

		public void Wait(TimeSpan timeout)
		{
			Thread.Sleep(timeout);
			Invoke(() => { });
		}

		private BitmapSource CaptureScreenshot(FrameworkElement element)
		{
			var pixelWidth = (int) Math.Ceiling(element.ActualWidth);
			var pixelHeight = (int) Math.Ceiling(element.ActualHeight);
			const int dpi = 96;
			var image = new RenderTargetBitmap(pixelWidth, pixelHeight, dpi, dpi, PixelFormats.Pbgra32);
			image.Render(element);
			image.Freeze();
			return image;
		}

		private void Invoke(Action action)
		{
			_dispatcher.Invoke(action, DispatcherPriority.Background);
		}

		private TY Invoke<TY>(Func<TY> func)
		{
			var result = default(TY);
			Invoke(() => { result = func(); });
			return result;
		}

		public void AddCodeSnippetFromMethod(MethodInfo method)
		{
			throw new NotImplementedException();
		}

		public void RenderTo(IExampleWriter writer)
		{
			var codeSnippetWriter = writer.AddCodeSnippet("xaml");
			var xamlNamespace = typeof(T).Assembly.GetName().Name;
			codeSnippetWriter.Write("<{0}:{1} ", xamlNamespace, _controlName);

			foreach (var pair in _values)
			{
				var property = pair.Key;
				var value = pair.Value;
				codeSnippetWriter.Write("{0}=\"{1}\" ", property.Name, value);
			}

			codeSnippetWriter.WriteLine("/>");

			_dispatcher.Invoke(() =>
			{
				var screenshot = CaptureScreenshot(_element);
				var relativeImagePath = _controlDocumentationCreator.AddImage(screenshot, _exampleName);
				writer.AddImage(string.Format("Image of {0}, {1}", _controlName, _exampleName), relativeImagePath);
			}, DispatcherPriority.Background);
		}

		public string Name => _exampleName;
	}
}