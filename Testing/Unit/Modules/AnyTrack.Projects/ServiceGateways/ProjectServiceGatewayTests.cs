using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnyTrack.Accounting.ServiceGateways;
using AnyTrack.Backend.Data.Model;
using AnyTrack.Infrastructure.BackendAccountService;
using AnyTrack.Projects.BackendProjectService;
using AnyTrack.Projects.ServiceGateways;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using Project = AnyTrack.Projects.BackendProjectService.ServiceProject;

namespace Unit.Modules.AnyTrack.Projects.ServiceGateways
{
    
        #region Context
        public class Context
        {
            public static IProjectService projectService;

            public static ProjectServiceGateway gateway;

            [SetUp]
            public void ContextSetup()
            {
                projectService = Substitute.For<IProjectService>();
                gateway = new ProjectServiceGateway(projectService);
            }
        }
        #endregion

        #region Tests

        public class ProjectServiceGatewayTests : Context
        {
            #region Constructor Tests 

            [Test]
            [ExpectedException(typeof (ArgumentNullException))]
            public void ConstructWithNoClient()
            {
                gateway = new ProjectServiceGateway(null);
            }

            #endregion 

            #region CreateProject(ServiceProject project) Tests 

            [Test]
            public void CreateProject()
            {
                Project sentModel = null;
                projectService.AddProject(Arg.Do<Project>(n => sentModel = n));

                Project testProject = new Project
                {
                    Name = "TestProject",
                    Description = "This is a project",
                    VersionControl = "V1",
                    StartedOn = new DateTime(2015, 9, 30),
                    ProjectManagerEmailAddress = "test@agile.local"
                
                };

                gateway.CreateProject(testProject);
                Assert.AreEqual(sentModel.Name, "TestProject");
                Assert.AreEqual(sentModel.Description, "This is a project");
                Assert.AreEqual(sentModel.VersionControl, "V1");
                Assert.AreEqual(sentModel.StartedOn, new DateTime(2015, 9, 30));
                Assert.AreEqual(sentModel.ProjectManagerEmailAddress, "test@agile.local");

            }

            #endregion 

            #region UpdateProject(ServiceProject project) Tests

            [Test]
            public void UpdateProject()
            {
                Project sentModel = null;
                projectService.AddProject(Arg.Do<Project>(n => sentModel = n));
                projectService.UpdateProject(Arg.Do<Project>(n => sentModel = n));

                Project testProject = new Project
                {
                    Name = "TestProject",
                    Description = "This is a project",
                    VersionControl = "V1",
                    StartedOn = new DateTime(2015, 9, 30),
                    ProjectManagerEmailAddress = "test@agile.local",
                };

                gateway.CreateProject(testProject);
                Assert.AreEqual(sentModel.Name, "TestProject");
                testProject.Name = "TestProject2";
                gateway.UpdateProject(testProject);
                Assert.AreEqual(sentModel.Name, "TestProject2");
                Assert.AreEqual(sentModel.Description, "This is a project");
                Assert.AreEqual(sentModel.VersionControl, "V1");
                Assert.AreEqual(sentModel.StartedOn, new DateTime(2015, 9, 30));
                Assert.AreEqual(sentModel.ProjectManagerEmailAddress, "test@agile.local");
            }

            #endregion

            #region DeleteProject(Guid id) Tests

            [Test]
            public void DeleteProject()
            {
                Project sentModel = null;
                projectService.AddProject(Arg.Do<Project>(n => sentModel = n));
                projectService.DeleteProject(Arg.Do<Guid>(n => sentModel.ProjectId = n));

                Project testProject = new Project
                {
                    Name = "TestProject",
                    Description = "This is a project",
                    VersionControl = "V1",
                    StartedOn = new DateTime(2015, 9, 30),
                    ProjectManagerEmailAddress = "test@agile.local"
                };

                gateway.CreateProject(testProject);
                Assert.IsNotNull(sentModel);
                gateway.DeleteProject(sentModel.ProjectId);
                Assert.IsNull(sentModel);
            }

            #endregion 

            #region GetProject(Guid id) Tests 

            public void GetProject()
            {
            }

            #endregion

            #region GetProjects() Tests 

            public void GetProjects()
            {
            }

            #endregion

            #region SearchUsers(UserSearchFilter filter)

            [Test]
            public void CallSearchUsers()
            {
                var userFilter = new UserSearchFilter();
                var results = new List<UserSearchInfo>();

                projectService.SearchUsers(userFilter).Returns(results);

                var gatewayResults = gateway.SearchUsers(userFilter);
                gatewayResults.Should().NotBeNull();
                gatewayResults.Equals(results).Should().BeTrue();
            }

            #endregion
        }

        

    #endregion
    
}
