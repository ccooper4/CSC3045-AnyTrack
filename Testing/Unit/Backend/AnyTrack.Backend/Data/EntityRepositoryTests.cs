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

namespace Unit.Backend.AnyTrack.Backend.Data.EntityRepositoryTests
{
    #region Supporting Types 
    
    public class DbObject: BaseEntity
    {

    }

    #endregion

    #region Context

    public class Context
    {
        public static IDbSet<DbObject> fakeDbSet; 
        public static EntityRepository<DbObject> repository; 

        [SetUp]
        public void ContextSetup()
        {
            fakeDbSet = Substitute.For<IDbSet<DbObject>>();
            repository = new EntityRepository<DbObject>(fakeDbSet);
        }
    }

    #endregion 

    #region Tests 

    public class EntityRepositoryTests: Context
    {
        #region Constructor Tests 

        [Test]
        public void ConstructEntityRepository()
        {
            repository = new EntityRepository<DbObject>(fakeDbSet);
            repository.Should().NotBeNull();
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructEntityRepositoryNoDbSet()
        {
            repository = new EntityRepository<DbObject>(null);
        }

        #endregion 

        #region .Insert(T Item) Tests

        [Test]
        public void AddItemToRepository()
        {
            var item = new DbObject();

            repository.Insert(item);

            fakeDbSet.Received().Add(item);
        }

        #endregion 

        #region .Delete(T Item) Tests

        [Test]
        public void DeleteItemFromRepository()
        {
            var item = new DbObject();

            repository.Delete(item);

            fakeDbSet.Received().Remove(item);
        }

        #endregion 

        #region Items Tests

        [Test]
        public void GetTheRepositoryItems()
        {
            repository.Items.Should().BeSameAs(fakeDbSet);
        }

        #endregion 

    }

    #endregion


}
