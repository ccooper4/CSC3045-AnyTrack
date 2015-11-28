using AnyTrack.Backend.Providers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;

namespace Unit.Backend.AnyTrack.Backend.Providers.ActivePokerSessionsProviderTests
{
    #region Setup
    public class Context
    {
        public static ActivePokerSessionsProvider provider;

        [SetUp]
        public void Setup()
        {
            provider = new ActivePokerSessionsProvider();
        }
    }

    #endregion

    #region Tests

    public class ActivePokerSessionsProviderTests : Context
    {
        #region GetListOfSessions Tests

        [Test]
        public void GetListOfClientsFirstTime()
        {
            var res = provider.GetListOfSessions();

            res.Should().NotBeNull();
        }

        [Test]
        public void GetListOfSessionsSecondTime()
        {
            var res = provider.GetListOfSessions();

            var actualRes = provider.GetListOfSessions();
            actualRes.Equals(res).Should().BeTrue();
        }

        #endregion
    }

    #endregion
}
