using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using AnyTrack.Backend.Data.Model;
using AnyTrack.Backend.Data;
using System.Data.Entity;
using NSubstitute;
using FluentAssertions;

namespace Unit.Backend.AnyTrack.Backend.Data.EntityUnitOfWorkTests
{
    #region Context

    public class Context
    {
        public static EntityUnitOfWork unitOfWork;

        [SetUp]
        public void ContextSetup()
        {
            unitOfWork = new EntityUnitOfWork();
        }
    }

    #endregion

    #region Tests

    public class EntityUnitOfWorkTests : Context
    {
        #region Constructor Tests 

        [Test]
        public void ConstructEntityUnitOfWork()
        {
            unitOfWork = new EntityUnitOfWork();

            unitOfWork.UserRepository.Should().NotBeNull();
            unitOfWork.RoleRepository.Should().NotBeNull();
        }

        #endregion 
    }

    #endregion
}
