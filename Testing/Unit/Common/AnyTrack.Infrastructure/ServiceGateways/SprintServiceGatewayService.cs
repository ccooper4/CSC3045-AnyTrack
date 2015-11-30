using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnyTrack.Backend.Data.Model;
using AnyTrack.Backend.Migrations;
using NSubstitute;
using NUnit.Framework;
using Project = AnyTrack.Infrastructure.BackendSprintService.Project;
using Sprint = AnyTrack.Infrastructure.BackendSprintService.Sprint;
using User = AnyTrack.Infrastructure.BackendSprintService.User;
using AnyTrack.Infrastructure.BackendSprintService;
using AnyTrack.Infrastructure.ServiceGateways;

namespace Unit.Modules.AnyTrack.Sprints.ServiceGateways
{
    #region Context

    public class Context
    {
        public static ISprintService client;
        public static SprintServiceGateway gateway;

        [SetUp]
        public void ContextSetup()
        {
            client = Substitute.For<ISprintService>();
            gateway = new SprintServiceGateway(client);
        }
    }

    #endregion

    public class SprintServiceGatewayService : Context
    {
        #region Constructor Tests

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNoClient()
        {
            gateway = new SprintServiceGateway(null);
        }

        #endregion

        #region void AddSprint(Guid projectId, ServiceSprint sprint) Tests

        [Test]
        public void AddSprint()
        {
            #region Test Data

            ServiceSprint sprint = new ServiceSprint()
            {
                SprintId = new Guid("00000001-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Sprint 1",
                StartDate = new DateTime(2015, 11, 26),
                EndDate = new DateTime(2015, 12, 3),
                Description = "Sprint",
                TeamEmailAddresses = new List<string>() { "tester@agile.local" }
            };

            Project project = new Project
            {
                Id = new Guid("11223344-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Project",
                Description = "This is a new project",
                VersionControl = "queens.git",
                ProjectManager = new User()
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
                    SecretAnswer = "At Home"
                },
                StartedOn = DateTime.Today,
                Sprints = new List<Sprint>()
                                {
                                    new Sprint()
                                    {
                                        Id = new Guid("00000001-5566-7788-99AA-BBCCDDEEFF00"),
                                        Name = "Sprint 1",
                                        StartDate = new DateTime(2015, 11, 26),
                                        EndDate = new DateTime(2015, 12, 3),
                                        Description = "Sprint"
                                    }
                                }
            };

            #endregion

            client.AddSprint(project.Id, sprint);

            client.Received().AddSprint(project.Id, sprint);

        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddSprintNullProjectId()
        {
            #region Test Data

            ServiceSprint sprint = new ServiceSprint()
            {
                SprintId = new Guid("00000001-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Sprint 1",
                StartDate = new DateTime(2015, 11, 26),
                EndDate = new DateTime(2015, 12, 3),
                Description = "Sprint",
                TeamEmailAddresses = new List<string>() { "tester@agile.local" }
            };

            Project project = new Project
            {
                Id = Guid.Empty,
                Name = "Project",
                Description = "This is a new project",
                VersionControl = "queens.git",
                ProjectManager = new User()
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
                    SecretAnswer = "At Home"
                },
                StartedOn = DateTime.Today,
                Sprints = new List<Sprint>()
                {
                    new Sprint()
                    {
                        Id = new Guid("00000001-5566-7788-99AA-BBCCDDEEFF00"),
                        Name = "Sprint 1",
                        StartDate = new DateTime(2015, 11, 26),
                        EndDate = new DateTime(2015, 12, 3),
                        Description = "Sprint"
                    }
                }
            };

            #endregion

            client.When(a => a.AddSprint(Arg.Any<Guid>(), Arg.Any<ServiceSprint>())).Do(a => { throw new ArgumentNullException(); });
            client.AddSprint(project.Id, sprint);

            client.Received().AddSprint(project.Id, sprint);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddSprintNullSprint()
        {
            #region Test Data

            ServiceSprint sprint = null;

            Project project = new Project
            {
                Id = new Guid("11223344-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Project",
                Description = "This is a new project",
                VersionControl = "queens.git",
                ProjectManager = new User()
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
                    SecretAnswer = "At Home"
                },
                StartedOn = DateTime.Today,
                Sprints = new List<Sprint>()
                {
                    new Sprint()
                    {
                        Id = new Guid("00000001-5566-7788-99AA-BBCCDDEEFF00"),
                        Name = "Sprint 1",
                        StartDate = new DateTime(2015, 11, 26),
                        EndDate = new DateTime(2015, 12, 3),
                        Description = "Sprint"
                    }
                }
            };

            #endregion

            client.When(a => a.AddSprint(Arg.Any<Guid>(), Arg.Any<ServiceSprint>())).Do(a => { throw new ArgumentNullException(); });
            client.AddSprint(project.Id, sprint);

            client.Received().AddSprint(project.Id, sprint);
        }

        #endregion

        #region void EditSprint(Guid sprintId, ServiceSprint updatedSprint) Tests

        [Test]
        public void EditSprint()
        {
            #region Test Data

            ServiceSprint sprint = new ServiceSprint()
            {
                SprintId = new Guid("00000001-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Sprint 1",
                StartDate = new DateTime(2015, 11, 26),
                EndDate = new DateTime(2015, 12, 3),
                Description = "Sprint",
                TeamEmailAddresses = new List<string>() { "tester@agile.local" }
            };

            Sprint dataSprint = new Sprint()
            {
                Id = new Guid("00000001-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Sprint 1",
                StartDate = new DateTime(2015, 11, 26),
                EndDate = new DateTime(2015, 12, 3),
                Description = "Sprint"
            };

            #endregion

            client.EditSprint(dataSprint.Id, sprint);

            client.Received().EditSprint(dataSprint.Id, sprint);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void EditSprintNullSprintId()
        {
            #region Test Data

            ServiceSprint sprint = new ServiceSprint()
            {
                SprintId = new Guid("00000001-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Sprint 1",
                StartDate = new DateTime(2015, 11, 26),
                EndDate = new DateTime(2015, 12, 3),
                Description = "Sprint",
                TeamEmailAddresses = new List<string>() { "tester@agile.local" }
            };

            Sprint dataSprint = new Sprint()
            {
                Id = Guid.Empty,
                Name = "Sprint 1",
                StartDate = new DateTime(2015, 11, 26),
                EndDate = new DateTime(2015, 12, 3),
                Description = "Sprint"
            };

            #endregion

            client.When(a => a.EditSprint(Arg.Any<Guid>(), Arg.Any<ServiceSprint>())).Do(a => { throw new ArgumentNullException(); });
            client.EditSprint(dataSprint.Id, sprint);

            client.Received().EditSprint(dataSprint.Id, sprint);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void EditSprintNullSprint()
        {
            #region Test Data

            ServiceSprint sprint = null;

            Sprint dataSprint = new Sprint()
            {
                Id = new Guid("00000001-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Sprint 1",
                StartDate = new DateTime(2015, 11, 26),
                EndDate = new DateTime(2015, 12, 3),
                Description = "Sprint"
            };

            #endregion

            client.When(a => a.EditSprint(Arg.Any<Guid>(), Arg.Any<ServiceSprint>())).Do(a => { throw new ArgumentNullException(); });
            client.EditSprint(dataSprint.Id, sprint);

            client.Received().EditSprint(dataSprint.Id, sprint);
        }

        #endregion
    }
}
