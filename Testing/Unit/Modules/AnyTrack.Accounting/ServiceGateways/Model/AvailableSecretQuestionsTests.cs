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
        public static ClientAvailableSecretQuestions model; 

        [SetUp]
        public void SetUp()
        {
            model = new ClientAvailableSecretQuestions();
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
            var all = ClientAvailableSecretQuestions.All();

            all.Count.Should().Be(3);
            all[0].Should().Be(ClientAvailableSecretQuestions.MaidenName);
            all[1].Should().Be(ClientAvailableSecretQuestions.FirstSchool);
            all[2].Should().Be(ClientAvailableSecretQuestions.FirstPet);
        }

        #endregion 
    }

    #endregion  
}
