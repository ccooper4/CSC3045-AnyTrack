using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnyTrack.Backend.Data.Model;
using AnyTrack.Backend.Migrations;
using NSubstitute;
using NUnit.Framework;
using ServiceProject = AnyTrack.Infrastructure.BackendProjectService.ServiceProject;
using ServiceSprint = AnyTrack.Infrastructure.BackendSprintService.ServiceSprint;
using User = AnyTrack.Infrastructure.BackendSprintService.ServiceUser;
using AnyTrack.Infrastructure.BackendSprintService;
using AnyTrack.Infrastructure.ServiceGateways;
using FluentAssertions;

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
                TeamEmailAddresses = new List<string>() { "tester@agile.local" },
                ProjectId = new Guid("11223344-5566-7788-99AA-BBCCDDEEFF00")
            };

            #endregion

            client.AddSprint(sprint.ProjectId, sprint);
            client.Received().AddSprint(sprint.ProjectId, sprint);

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

            client.EditSprint(sprint.SprintId, sprint);
            client.Received().EditSprint(sprint.SprintId, sprint);
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
    }
}
