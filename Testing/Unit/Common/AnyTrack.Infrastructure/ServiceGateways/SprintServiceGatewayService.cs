using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnyTrack.Backend.Data.Model;
using AnyTrack.Backend.Migrations;
using AnyTrack.Infrastructure.BackendProjectService;
using NSubstitute;
using NUnit.Framework;
using AnyTrack.Infrastructure.BackendSprintService;
using AnyTrack.Infrastructure.ServiceGateways;
using FluentAssertions;
using ServiceSprint = AnyTrack.Infrastructure.BackendSprintService.ServiceSprint;
using ServiceSprintStory = AnyTrack.Infrastructure.BackendSprintService.ServiceSprintStory;
using ServiceSprintSummary = AnyTrack.Infrastructure.BackendSprintService.ServiceSprintSummary;
using ServiceStory = AnyTrack.Infrastructure.BackendSprintService.ServiceStory;

namespace Unit.Modules.AnyTrack.Sprints.ServiceGateways
{
    #region Context

    public class Context
    {
        public static ISprintService client;
        public static IAccountServiceGateway accGateway;

        public static SprintServiceGateway gateway;

        [SetUp]
        public void ContextSetup()
        {
            client = Substitute.For<ISprintService>();
            accGateway = Substitute.For<IAccountServiceGateway>();

            gateway = new SprintServiceGateway(client, accGateway);
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
            gateway = new SprintServiceGateway(null, accGateway);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNoGateway()
        {
            gateway = new SprintServiceGateway(client, null);
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
                TeamEmailAddresses = new List<string>() { "tester@agile.local" },
                ProjectId = new Guid("11223344-5566-7788-99AA-BBCCDDEEFF00")
            };

            #endregion

            gateway.AddSprint(sprint.ProjectId, sprint);
            client.Received().AddSprint(sprint.ProjectId, sprint);
            accGateway.Received().RefreshLoginPrincipal();

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
                TeamEmailAddresses = new List<string>() { "tester@agile.local" },
                ProjectId = new Guid("11223344-5566-7788-99AA-BBCCDDEEFF00")
            };

            #endregion

            client.When(a => a.AddSprint(Arg.Any<Guid>(), Arg.Any<ServiceSprint>())).Do(a => { throw new ArgumentNullException(); });
            client.AddSprint(sprint.ProjectId, sprint);
            client.Received().AddSprint(sprint.ProjectId, sprint);        }


        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddSprintNullSprint()
        {
            #region Test Data

            ServiceSprint sprint = null;
            Guid projectID = new Guid("11223344-5566-7788-99AA-BBCCDDEEFF00");

            #endregion

            client.When(a => a.AddSprint(Arg.Any<Guid>(), Arg.Any<ServiceSprint>())).Do(a => { throw new ArgumentNullException(); });
            client.AddSprint(projectID, sprint);
            client.Received().AddSprint(projectID, sprint);
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
                TeamEmailAddresses = new List<string>() {"tester@agile.local"}
            };

            #endregion

            gateway.EditSprint(sprint.SprintId, sprint);
            client.Received().EditSprint(sprint.SprintId, sprint);
            accGateway.Received().RefreshLoginPrincipal();
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

            #endregion

            client.When(a => a.EditSprint(Arg.Any<Guid>(), Arg.Any<ServiceSprint>())).Do(a => { throw new ArgumentNullException(); });
            client.EditSprint(sprint.SprintId, sprint);

            client.Received().EditSprint(sprint.SprintId, sprint);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void EditSprintNullSprint()
        {
            #region Test Data

            ServiceSprint sprint = null;
            Guid sprintID = Guid.NewGuid();

            #endregion

            client.When(a => a.EditSprint(Arg.Any<Guid>(), Arg.Any<ServiceSprint>())).Do(a => { throw new ArgumentNullException(); });
            client.EditSprint(sprintID, sprint);
            client.Received().EditSprint(sprintID, sprint);
        }

        #endregion

        #region GetSprintNames(Guid? projectId, bool scrumMaster, bool developer) Tests 

        [Test]
        public void CallGetSprintNames()
        {
            var projectId = Guid.NewGuid();
            var dev = true; 
            var sm = true; 

            var innerResult = new List<ServiceSprintSummary>();
            client.GetSprintNames(projectId, sm, dev).Returns(innerResult);

            var res = gateway.GetSprintNames(projectId, sm, dev);

            res.Equals(innerResult).Should().BeTrue();
        }

        #endregion 

        #region GetSprintStories(Guid sprintId)
        
        [Test]
        public void GetSprintStories()
        {
            ServiceProject project = new ServiceProject
            {
                ProjectId = new Guid("11223344-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Project",
                Description = "This is a new project",
                VersionControl = "queens.git",
                ProjectManagerEmailAddress = "tester@agile.local",
                StartedOn = DateTime.Today,
            };

            ServiceSprint sprint = new ServiceSprint
            {
                ProjectId = new Guid(),
                SprintId = new Guid("00000001-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Sprint 1",
                StartDate = new DateTime(2015, 11, 26),
                EndDate = new DateTime(2015, 12, 3),
                Description = "Sprint",
                Backlog = new List<ServiceSprintStory>()
                {
                    new ServiceSprintStory()
                    {
                        SprintId = new Guid("00000001-5566-7788-99AA-BBCCDDEEFF00"),
                        Story = new ServiceStory()
                        {
                            StoryId = new Guid("30000001-5566-7788-99AA-BBCCDDEEFF00"),
                            AsA = "As A 1",
                            ConditionsOfSatisfaction = "Conditions of Sat",
                            IWant = "I Want 1",
                            SoThat = "So That 1",
                            Summary = "Summary 1"
                        }
                    }
                }
            };

            client.AddSprint(project.ProjectId, sprint);
            var sprintStories = client.GetSprintStories(sprint.SprintId);
            client.Received().GetSprintStories(sprint.SprintId);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetSprintStoriesNullId()
        {
            ServiceSprint sprint = new ServiceSprint
            {
                ProjectId = new Guid(),
                SprintId = Guid.Empty,
                Name = "Sprint 1",
                StartDate = new DateTime(2015, 11, 26),
                EndDate = new DateTime(2015, 12, 3),
                Description = "Sprint",
                Backlog = new List<ServiceSprintStory>()
                {
                    new ServiceSprintStory()
                    {
                        SprintId = Guid.Empty,
                        Story = new ServiceStory()
                        {
                            StoryId = new Guid("30000001-5566-7788-99AA-BBCCDDEEFF00"),
                            AsA = "As A 1",
                            ConditionsOfSatisfaction = "Conditions of Sat",
                            IWant = "I Want 1",
                            SoThat = "So That 1",
                            Summary = "Summary 1"
                        }
                    }
                }
            };


            client.When(a => a.GetSprintStories((Arg.Any<Guid>()))).Do(a => { throw new ArgumentNullException(); });
            var sprintStories = client.GetSprintStories(sprint.SprintId);
            client.Received().GetSprintStories(sprint.SprintId);
        }

        #endregion

        [Test]
        public void ManageSprintBacklog()
        {
            var projectId = Guid.NewGuid();
            var sprintId = Guid.NewGuid();
            var stories = new List<ServiceSprintStory>();

            gateway.ManageSprintBacklog(projectId, sprintId, stories);

            client.Received().ManageSprintBacklog(projectId, sprintId, stories);
        }
    }
}
