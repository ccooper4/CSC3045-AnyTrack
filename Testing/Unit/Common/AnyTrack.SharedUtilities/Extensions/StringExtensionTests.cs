using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using AnyTrack.SharedUtilities.Extensions;

namespace Unit.Common.AnyTrack.SharedUtilities.Extensions.StringExtensionTests
{
    #region Context 

    public class Context
    {
        public static string testString = "";
    }

    #endregion 

    #region Tests 

    public class StringExtensionTests : Context
    {
        #region .IsNotEmpty(this string s)

        [Test]
        public void NotEmptyForANotEmptyString()
        {
            testString = "Test";
            testString.IsNotEmpty().Should().BeTrue();
        }

        [Test]
        public void NotEmptyForAnEmptyString()
        {
            testString = "";
            testString.IsNotEmpty().Should().BeFalse();
        }

        #endregion 

        #region .IsEmpty(this string s)

        [Test]
        public void EmptyForANotEmptyString()
        {
            testString = "Test";
            testString.IsEmpty().Should().BeFalse();
        }

        [Test]
        public void EmptyForAnEmptyString()
        {
            testString = "";
            testString.IsEmpty().Should().BeTrue();
        }

        #endregion 

        #region .Substitute(this string s, params object[] items)

        [Test]
        public void SubstituteWithAllParams()
        {
            testString = "Hello {0}";
            testString.Substitute("There!").Should().Be("Hello There!");
        }

        [Test]
        public void SubstituteWithNoFormatString()
        {
            testString = "Hello";
            testString.Substitute("There!").Should().Be("Hello");
        }

        [Test]
        [ExpectedException(typeof(FormatException))]
        public void SubstituteWithMissingParams()
        {
            testString = "Hello {0} {1}";
            testString.Substitute("There!");
        }

        #endregion 
    }


    #endregion 
}
