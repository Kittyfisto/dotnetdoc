using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using dotnetdoc.Creators;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace dotnetdoc.Test.Creators
{
	[TestFixture]
	[RequiresThread(ApartmentState.STA)]
	public sealed class FrameworkElementExampleCreatorTest
	{
		[SetUp]
		public void Setup()
		{
			
		}

		[Test]
		public void TestSetValueDouble()
		{
			var creator = Create<ProgressBar>();
			creator.Element.Value.Should().Be(0);
			creator.SetValue(RangeBase.ValueProperty, 50d);
			creator.Element.Value.Should().Be(50);
		}

		[Test]
		public void TestSetValueClass()
		{
			var creator = Create<ProgressBar>();
			creator.Element.Value.Should().Be(0);
			creator.SetValue(Control.BorderBrushProperty, Brushes.Black);
			creator.Element.BorderBrush.Should().Be(Brushes.Black);
		}

		[Test]
		public void TestSetValueConvertType()
		{
			var creator = Create<ProgressBar>();
			creator.Element.Value.Should().Be(0);
			creator.SetValue(RangeBase.ValueProperty, 50);
			creator.Element.Value.Should().Be(50);
		}

		[Test]
		public void TestSetValueNullableBoolean([Values(true, false, null)] bool? value)
		{
			var creator = Create<ToggleButton>();
			creator.Element.IsChecked.Should().BeFalse();
			creator.SetValue(ToggleButton.IsCheckedProperty, value);
			creator.Element.IsChecked.Should().Be(value);
		}

		private static FrameworkElementExampleCreator<T> Create<T>() where T : FrameworkElement, new()
		{
			var controlCreator = new Mock<IInternalControlDocumentationCreator<T>>();
			var dispatcher = new DispatcherMock();
			return new FrameworkElementExampleCreator<T>(controlCreator.Object,
			                                             dispatcher,
			                                             new ResourceDictionary(),
			                                             "");
		}
	}
}
