using AnyTrack.Accounting.ServiceGateways.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;

namespace Unit.Modules.AnyTrack.Accounting.ServiceGateways.Model.AvailableSecretQuestionsTests
{
    #region Context

    public class Context
    {
        public static AvailableSecretQuestions model; 

        [SetUp]
        public void SetUp()
        {
            model = new AvailableSecretQuestions();
        }
    }

    #endregion 

    #region Tests

    public class AvailableSecretQuestionsTests: Context
    {
        #region All Tests 

        [Test]
        public void GetAllQuestions()
        {
            var all = AvailableSecretQuestions.All();

            all.Count.Should().Be(3);
            all[0].Should().Be(AvailableSecretQuestions.MAIDENNAME);
            all[1].Should().Be(AvailableSecretQuestions.FIRSTSCHOOL);
            all[2].Should().Be(AvailableSecretQuestions.FIRSTPET);
        }

        #endregion 
    }

    #endregion  
}
