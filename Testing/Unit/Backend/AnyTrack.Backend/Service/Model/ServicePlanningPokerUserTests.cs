using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NSubstitute;
using FluentAssertions;
using AnyTrack.Backend.Service.Model;

namespace Unit.Backend.AnyTrack.Backend.Service.Model.ServicePlanningPokerUserTests
{
    #region Context 

    public class Context
    {
        public ServicePlanningPokerUser model;

        [SetUp]
        public void SetUp()
        {
            model = new ServicePlanningPokerUser();
        }
    }

    #endregion 

    #region Tests

    public class ServicePlanningPokerUserTests : Context
    {
        #region UserRoles Tests 

        [Test]
        public void SetUserRoles()
        {
            var roles = new List<string>() { "Test 1", "Test 2" };
            model.UserRoles = roles;

            model.RoleSummary.Should().Be("Test 1,Test 2");
        }

        #endregion 
    }

    #endregion 
}
