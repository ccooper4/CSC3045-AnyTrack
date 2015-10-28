using AnyTrack.Backend.Data;
using AnyTrack.Backend.Data.Model;
using AnyTrack.Backend.Providers;
using AnyTrack.Backend.Service;
using AnyTrack.Backend.Service.Model;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using System.Web.Security;
using System.Web.Helpers;
using System.Threading;

namespace Unit.Backend.AnyTrack.Backend.Service.AccountServiceTests
{
    public class Context
    {
        public static IUnitOfWork unitOfWork;
        public static FormsAuthenticationProvider provider;
        public static AccountService service; 

        [SetUp]
        public void ContextSetup()
        {
            unitOfWork = Substitute.For<IUnitOfWork>();
            provider = Substitute.For<FormsAuthenticationProvider>();
            service = new AccountService(unitOfWork, provider);
        }
    }

    public class AccountServiceTests: Context
    {
        #region Constructor Tests 

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructAccountServiceNoUnitOfWork()
        {
            service = new AccountService(null, provider);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructAccountServiceNoProvider()
        {
            service = new AccountService(unitOfWork, null);
        }

        #endregion

        #region CreateAccount(NewUser user) Tests 

        [Test]
        public void CreateNewAccount()
        {
            User dataUser = null;

            unitOfWork.UserRepository.Items.Returns(new List<User>().AsQueryable());
            unitOfWork.UserRepository.Insert(Arg.Do<User>(u => dataUser = u));

            var newUser = new NewUser
            {
                EmailAddress = "test@agile.local",
                FirstName = "David",
                LastName = "Tester",
                Password = "Letmein"
            };

            service.CreateAccount(newUser);

            dataUser.Should().NotBeNull();
            dataUser.EmailAddress.Should().Be(newUser.EmailAddress);
            dataUser.FirstName.Should().Be(newUser.FirstName);
            dataUser.LastName.Should().Be(newUser.LastName);
            Crypto.VerifyHashedPassword(dataUser.Password, newUser.Password).Should().BeTrue();
            unitOfWork.UserRepository.Received().Insert(dataUser);
            unitOfWork.Received().Commit();
        }

        [Test]
        [ExpectedException(typeof(MembershipCreateUserException))]
        public void CreateNewAccountWithDuplicateEmail()
        {
            var userList = new List<User>()
            {
                new User { EmailAddress = "test@agile.local" }
            };

            unitOfWork.UserRepository.Items.Returns(userList.AsQueryable());

            var newUser = new NewUser
            {
                EmailAddress = "test@agile.local",
                FirstName = "David",
                LastName = "Tester",
                Password = "Letmein"
            };

            service.CreateAccount(newUser);
        }


        #endregion

        #region LogIn(UserCredential credential) Tests 

        [Test]
        public void LogInWithNoAccount()
        {
            var credentials = new UserCredential
            {
                EmailAddress = "test@agile.local",
                Password = "Letmein"
            };

            unitOfWork.UserRepository.Items.Returns(new List<User>().AsQueryable());

            var result = service.LogIn(credentials);

            result.Success.Should().BeFalse();
        }

        [Test]
        public void LogInWithWrongPassword()
        {
            var credentials = new UserCredential
            {
                EmailAddress = "test@agile.local",
                Password = "Letmein"
            };

            var dataUser = new User
            {
                EmailAddress = "test@agile.local",
                Password = Crypto.HashPassword("Wrong")
            };

            unitOfWork.UserRepository.Items.Returns(new List<User>()
            {
                dataUser
            }.AsQueryable());

            var result = service.LogIn(credentials);

            result.Success.Should().BeFalse();
        }

        [Test]
        public void LogIn()
        {
            var credentials = new UserCredential
            {
                EmailAddress = "test@agile.local",
                Password = "Letmein"
            };

            var dataUser = new User
            {
                EmailAddress = "test@agile.local",
                Password = Crypto.HashPassword("Letmein"),
                Roles = new List<Role>()
            };

            unitOfWork.UserRepository.Items.Returns(new List<User>()
            {
                dataUser
            }.AsQueryable());

            var result = service.LogIn(credentials);

            provider.Received().SetAuthCookie(dataUser.EmailAddress, false);

            result.Success.Should().BeTrue();
        }

        #endregion 
    }
}
