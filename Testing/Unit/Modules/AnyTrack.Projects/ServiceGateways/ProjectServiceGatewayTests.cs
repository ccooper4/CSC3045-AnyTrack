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
                ServiceProject sentModel = null;
                projectService.AddProject(Arg.Do<ServiceProject>(n => sentModel = n));

                ServiceProject testProject = new ServiceProject
                {
                    Name = "TestProject",
                    Description = "This is a project",
                    VersionControl = "V1",
                    StartedOn = new DateTime(2015, 9, 30),
                    ProjectManagerEmailAddress = "test@agile.local"
                
                };

                gateway.CreateProject(testProject);
                projectService.Received().AddProject(testProject);
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
                ServiceProject sentModel = null;
                ServiceProject updatedModel = null;
                projectService.AddProject(Arg.Do<ServiceProject>(n => sentModel = n));
                projectService.UpdateProject(Arg.Do<ServiceProject>(n => updatedModel = n));

                ServiceProject testProject = new ServiceProject
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
                projectService.Received().UpdateProject(testProject);
                Assert.AreEqual(updatedModel.Name, "TestProject2");
                Assert.AreEqual(updatedModel.Description, "This is a project");
                Assert.AreEqual(updatedModel.VersionControl, "V1");
                Assert.AreEqual(updatedModel.StartedOn, new DateTime(2015, 9, 30));
                Assert.AreEqual(updatedModel.ProjectManagerEmailAddress, "test@agile.local");
            }

            #endregion

            #region DeleteProject(Guid id) Tests

            [Test]
            public void DeleteProject()
            {
                ServiceProject sentModel = null;
                ServiceProject deletedModel = new ServiceProject();
                projectService.AddProject(Arg.Do<ServiceProject>(n => sentModel = n));
                projectService.DeleteProject(Arg.Do<Guid>(n => deletedModel.ProjectId = n));

                ServiceProject testProject = new ServiceProject
                {
                    ProjectId = Guid.Parse("9245fe4a-d402-451c-b9ed-9c1a04247482"),
                    Name = "TestProject",
                    Description = "This is a project",
                    VersionControl = "V1",
                    StartedOn = new DateTime(2015, 9, 30),
                    ProjectManagerEmailAddress = "test@agile.local"
                };

                gateway.CreateProject(testProject);
                Assert.IsNotNull(sentModel);
                gateway.DeleteProject(testProject.ProjectId);
                projectService.Received().DeleteProject(testProject.ProjectId);
            }

            #endregion 

            #region GetProject(Guid id) Tests 

            [Test]
            public void GetProject()
            {
                ServiceProject sentModel = null;
                projectService.AddProject(Arg.Do<ServiceProject>(n => sentModel = n));

                ServiceProject testProject = new ServiceProject
                {
                    Name = "TestProject",
                    Description = "This is a project",
                    VersionControl = "V1",
                    StartedOn = new DateTime(2015, 9, 30),
                    ProjectManagerEmailAddress = "test@agile.local"

                };

                var results = new ServiceProject();
                gateway.CreateProject(testProject);
                projectService.GetProject(testProject.ProjectId).Returns(testProject);
                var gatewayResults = gateway.GetProject(testProject.ProjectId);
                projectService.Received().GetProject(testProject.ProjectId);

                Assert.AreEqual(gatewayResults.Name, "TestProject");
                Assert.AreEqual(gatewayResults.Description, "This is a project");
                Assert.AreEqual(gatewayResults.VersionControl, "V1");
                Assert.AreEqual(gatewayResults.StartedOn, new DateTime(2015, 9, 30));
                Assert.AreEqual(gatewayResults.ProjectManagerEmailAddress, "test@agile.local");
            }

            #endregion

            #region GetProjects() Tests 

            [Test]
            public void GetProjects()
            {
                ServiceProject sentModel = null;
                projectService.AddProject(Arg.Do<ServiceProject>(n => sentModel = n));

                List<ServiceProject> testProjects = new List<ServiceProject>()
                {
                    new ServiceProject()
                    {
                        Name = "TestProject1",
                        Description = "This is a project",
                        VersionControl = "V1",
                        StartedOn = new DateTime(2015, 9, 30),
                        ProjectManagerEmailAddress = "test@agile.local"
                    },
                    new ServiceProject()
                    {
                        Name = "TestProject2",
                        Description = "This is a project",
                        VersionControl = "V1",
                        StartedOn = new DateTime(2015, 9, 30),
                        ProjectManagerEmailAddress = "test@agile.local"
                    }
                };

                var results = new List<ServiceProject>();
                gateway.CreateProject(testProjects[0]);
                gateway.CreateProject(testProjects[1]);
                projectService.GetProjects().Returns(testProjects);
                var gatewayResults = gateway.GetProjects();
                projectService.Received().GetProjects();
                Assert.AreEqual(gatewayResults[0].Name, "TestProject1");
                Assert.AreEqual(gatewayResults[0].Description, "This is a project");
                Assert.AreEqual(gatewayResults[0].VersionControl, "V1");
                Assert.AreEqual(gatewayResults[0].StartedOn, new DateTime(2015, 9, 30));
                Assert.AreEqual(gatewayResults[0].ProjectManagerEmailAddress, "test@agile.local");
                Assert.AreEqual(gatewayResults[1].Name, "TestProject2");
                Assert.AreEqual(gatewayResults[1].Description, "This is a project");
                Assert.AreEqual(gatewayResults[1].VersionControl, "V1");
                Assert.AreEqual(gatewayResults[1].StartedOn, new DateTime(2015, 9, 30));
                Assert.AreEqual(gatewayResults[1].ProjectManagerEmailAddress, "test@agile.local");
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
