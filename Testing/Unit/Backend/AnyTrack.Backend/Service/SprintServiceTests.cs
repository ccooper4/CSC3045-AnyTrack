using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnyTrack.Backend.Data;
using AnyTrack.Backend.Data.Model;
using AnyTrack.Backend.Providers;
using AnyTrack.Backend.Service;
using AnyTrack.Backend.Service.Model;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace Unit.Backend.AnyTrack.Backend.Service
{
    #region Setup

    public class Context
    {
        public static IUnitOfWork unitOfWork;
        public static SprintService service;
        public static List<User> userList;
        public static List<Project> projectList;

        [SetUp]
        public void SetUp()
        {
            unitOfWork = Substitute.For<IUnitOfWork>();
            service = new SprintService(unitOfWork);

            userList = new List<User>()
            {
                #region Test Data Users
                
                new User
                {
                    EmailAddress = "tester@agile.local",
                    FirstName = "John",
                    LastName = "Test",
                    Password = "Password",
                    Developer = false,
                    ProductOwner = false,
                    ScrumMaster = false,
                    Skills = "C#, Java",
                    SecretQuestion = "Where do you live?",
                    SecretAnswer = "At Home",
                },
                new User
                {
                    EmailAddress = "PO@test.com",
                    FirstName = "Julie",
                    LastName = "Test",
                    Password = "Password",
                    Developer = false,
                    ProductOwner = true,
                    ScrumMaster = false,
                    Skills = "C#",
                    SecretQuestion = "Where do you live?",
                    SecretAnswer = "A car"
                }
                #endregion
            };

            projectList = new List<Project>()
            {
                #region Project Data
                new Project
                {
                    Id = new Guid("11223344-5566-7788-99AA-BBCCDDEEFF00"),
                    Name = "Project",
                    Description = "This is a new project",
                    VersionControl = "queens.git",
                    ProjectManager = userList[0],
                    StartedOn = DateTime.Today,
                    Sprints = new List<Sprint>()
                },
                new Project
                {
                    Id = new Guid("11223344-5566-7788-99AA-BBCCDDEEFFFF"),
                    Name = "Project2",
                    Description = "This is a new project2",
                    VersionControl = "queens.git",
                    ProjectManager = new User
                    {
                        EmailAddress = "p@hotmail.com",
                        FirstName = "John",
                        LastName = "Test",
                        Password = "Password",
                        Developer = false,
                        ProductOwner = false,
                        ScrumMaster = false,
                        Skills = "C#, Java",
                        SecretQuestion = "Where do you live?",
                        SecretAnswer = "At Home"
                    },
                    ProductOwner = userList[0],
                    StartedOn = DateTime.Today,
                    Sprints = new List<Sprint>()
                }
                    #endregion
            };
        }
    }

    #endregion

    public class SprintServiceTests : Context
    {
        #region Constructor Tests

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructSprintServiceNoUnitOfWork()
        {
            service = new SprintService(null);
        }

        [Test]
        public void ConstructSprintService()
        {
            service = new SprintService(unitOfWork);
            service.Should().NotBeNull();
        }
        
        #endregion

        #region  void AddSprint(Guid projectId, ServiceSprint sprint) Tests

        [Test]
        [ExpectedException(typeof (ArgumentNullException))]
        public void AddSprintNullProjectId()
        {
            #region Test Data

            Guid projectId = Guid.Empty;

            ServiceSprint sprint = new ServiceSprint()
            {
                SprintId = Guid.NewGuid(),
                Name = "Sprint 1",
                Description = "Sprint",
                StartDate = new DateTime(2015, 11, 26),
                EndDate = new DateTime(2015, 12, 4),
                TeamEmailAddresses = new List<string>() {"p@q.com"}
            };

            #endregion

            service.AddSprint(projectId, sprint);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddSprintNullSprintService()
        {
            #region Test Data

            Guid projectId = Guid.NewGuid();

            ServiceSprint sprint = null;

            #endregion

            service.AddSprint(projectId, sprint);
        }

        public void AddSprint()
        {
            #region Test Data

            ServiceSprint sprint = new ServiceSprint()
            {

            };

            #endregion
        }


        #endregion
    }
}
