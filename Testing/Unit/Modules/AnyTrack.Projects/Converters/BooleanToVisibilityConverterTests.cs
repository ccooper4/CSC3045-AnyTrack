using AnyTrack.Projects.Converters;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AnyTrack.Sprints.Converters;
using FluentAssertions;

namespace Unit.Modules.AnyTrack.Projects.Converters.BooleanToVisibilityConverterTests
{
    #region Context 

    public class Context
    {
        public static BooleanToVisibilityConverter converter; 

        [SetUp]
        public void SetUp()
        {
            converter = new BooleanToVisibilityConverter();
        }
    }

    #endregion 

    #region Tests 

    public class BooleanToVisibilityConverterTests: Context
    {
        #region Convert(object value, Type targetType, object parameter, CultureInfo culture) Tests 

        [Test]
        public void CallConvertForFalse()
        {
            var result = (Visibility)converter.Convert(false, null, null, null);

            result.Should().Be(Visibility.Collapsed);
        }

        [Test]
        public void CallConvertForTrue()
        {
            var result = (Visibility)converter.Convert(true, null, null, null);

            result.Should().Be(Visibility.Visible);
        }

        #endregion 

        #region .ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) Tests 

        [Test]
        public void ConvertBackForVisible()
        {
            var visible = Visibility.Visible;

            var result = (bool)converter.ConvertBack(visible, null, null, null);
            result.Should().BeTrue();
        }

        [Test]
        public void ConvertBackForHidden()
        {
            var visible = Visibility.Hidden;

            var result = (bool)converter.ConvertBack(visible, null, null, null);
            result.Should().BeFalse();
        }

        [Test]
        public void ConvertBackForCollapsed()
        {
            var visible = Visibility.Collapsed;

            var result = (bool)converter.ConvertBack(visible, null, null, null);
            result.Should().BeFalse();
        }

        #endregion 
    }

    #endregion 
}
