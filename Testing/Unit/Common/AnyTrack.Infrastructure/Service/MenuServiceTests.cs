using AnyTrack.Infrastructure.Service;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using AnyTrack.Infrastructure.Service.Model;

namespace Unit.Common.AnyTrack.Infrastructure.Service.MenuServiceTests
{
    #region Context

    public class Contex
    {
        public static MenuService service; 

        [SetUp]
        public void SetUp()
        {
            service = new MenuService();
        }
    }

    #endregion 

    #region Tests 

    public class MenuServiceTests: Contex
    {
        #region Constructor Tests 

        [Test]
        public void ConstructMenuService()
        {
            service.MenuItems.Should().NotBeNull();
        }

        #endregion 

        #region AddMenuItem(MenuItem item) Tests 

        [Test]
        public void AddItemToMenu()
        {
            var menuItem = new MenuItem();

            service.AddMenuItem(menuItem);

            service.MenuItems.Count().Should().Be(1);
            service.MenuItems.Single().Should().Be(menuItem);
        }

        #endregion 
    }

    #endregion 
}
