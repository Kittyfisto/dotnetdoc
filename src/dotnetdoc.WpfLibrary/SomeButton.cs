using System.Windows;
using System.Windows.Controls;

namespace dotnetdoc.WpfLibrary
{
	public class SomeButton
		: Button
	{
		static SomeButton()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(SomeButton), new FrameworkPropertyMetadata(typeof(SomeButton)));
		}
	}
}