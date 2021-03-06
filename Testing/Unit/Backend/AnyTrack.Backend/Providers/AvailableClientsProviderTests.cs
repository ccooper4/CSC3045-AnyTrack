﻿using AnyTrack.Backend.Providers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;

namespace Unit.Backend.AnyTrack.Backend.Providers.AvailableClientsProviderTests
{
    #region Setup
    public class Context
    {
        public static AvailableClientsProvider provider; 

        [SetUp]
        public void Setup()
        {
            provider = new AvailableClientsProvider();
        }
    }

    #endregion 

    #region Tests 

    public class ConnectedClientsProviderTests : Context
    {
        #region GetListOfClients Tests

        [Test]
        public void GetListOfClientsFirstTime()
        {
            var res = provider.GetListOfClients();

            res.Should().NotBeNull();
        }

        [Test]
        public void GetListOfClientsSecondTime()
        {
            var res = provider.GetListOfClients();

            var actualRes = provider.GetListOfClients();
            actualRes.Equals(res).Should().BeTrue();
        }

        #endregion
    }

    #endregion
}
