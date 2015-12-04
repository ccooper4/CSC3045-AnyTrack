using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using FluentAssertions;
using AnyTrack.Infrastructure.Converters;

namespace Unit.Modules.AnyTrack.Infrastructure.Converters.DateConverterTests
{
    #region Context 

    public class Context
    {
        public static DateConverter converter; 

        [SetUp]
        public void SetUp()
        {
            converter = new DateConverter();
        }
    }

    #endregion 

    #region Tests 

    public class DateConverterTests: Context
    {
        #region Convert(object value, Type targetType, object parameter, CultureInfo culture) Tests 

        [Test]
        public void CallConvertForDate()
        {
            var result = converter.Convert(new DateTime?(DateTime.Today), null, null, null).ToString();

            result.Should().Be(DateTime.Today.ToString("d"));
        }

        [Test]
        public void CallConvertForNull()
        {
            var result = converter.Convert(null, null, null, null).ToString();

            result.Should().Be(string.Empty);
        }

        #endregion 

        #region .ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) Tests 

        [Test]
        public void ConvertBackForValidString()
        {
            var result = (DateTime?)converter.ConvertBack(DateTime.Today.ToString("d"), null, null, null);
            result.Should().Be(DateTime.Today);
        }

        [Test]
        public void ConvertBackForInvalidString()
        {
            var result = (DateTime?)converter.ConvertBack("", null, null, null);
            result.Should().Be(null);
        }

        #endregion 
    }

    #endregion 
}
