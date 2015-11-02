using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnyTrack.Accounting.ServiceGateways;
using AnyTrack.Backend.Data.Model;
using AnyTrack.Projects.BackendProjectService;
using AnyTrack.Projects.ServiceGateways;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using Project = AnyTrack.Projects.BackendProjectService.Project;

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

        #region Test Constructor

        public class ProjectServiceGatewayTests : Context
        {
            [Test]
            [ExpectedException(typeof (ArgumentNullException))]
            public void ConstructWithNoClient()
            {
                gateway = new ProjectServiceGateway(null);
            }

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
                    ProjectManager = new NewUser
                    {
                        EmailAddress = "test@agile.local",
                        FirstName = "Test",
                        LastName = "Test",
                        Password = "Test",
                        ProductOwner = false,
                        ScrumMaster = false,
                        Developer = false   
                    }
                };

                gateway.CreateProject(testProject);
                Assert.AreEqual(sentModel.Name, "TestProject");
                Assert.AreEqual(sentModel.Description, "This is a project");
                Assert.AreEqual(sentModel.VersionControl, "V1");
                Assert.AreEqual(sentModel.StartedOn, new DateTime(2015, 9, 30));
                Assert.AreEqual(sentModel.ProjectManager.EmailAddress, "test@agile.local");
                Assert.AreEqual(sentModel.ProjectManager.FirstName, "Test");
                Assert.AreEqual(sentModel.ProjectManager.LastName, "Test");
                Assert.AreEqual(sentModel.ProjectManager.Password, "Test");
                Assert.IsFalse(sentModel.ProjectManager.ProductOwner);
                Assert.IsFalse(sentModel.ProjectManager.ScrumMaster);
                Assert.IsFalse(sentModel.ProjectManager.Developer);
            }

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
                    ProjectManager = new NewUser
                    {
                        EmailAddress = "test@agile.local",
                        FirstName = "Test",
                        LastName = "Test",
                        Password = "Test",
                        ProductOwner = false,
                        ScrumMaster = false,
                        Developer = false
                    }
                };

                gateway.CreateProject(testProject);
                Assert.AreEqual(sentModel.Name, "TestProject");
                testProject.Name = "TestProject2";
                gateway.UpdateProject(testProject);
                Assert.AreEqual(sentModel.Name, "TestProject2");
                Assert.AreEqual(sentModel.Description, "This is a project");
                Assert.AreEqual(sentModel.VersionControl, "V1");
                Assert.AreEqual(sentModel.StartedOn, new DateTime(2015, 9, 30));
                Assert.AreEqual(sentModel.ProjectManager.EmailAddress, "test@agile.local");
                Assert.AreEqual(sentModel.ProjectManager.FirstName, "Test");
                Assert.AreEqual(sentModel.ProjectManager.LastName, "Test");
                Assert.AreEqual(sentModel.ProjectManager.Password, "Test");
                Assert.IsFalse(sentModel.ProjectManager.ProductOwner);
                Assert.IsFalse(sentModel.ProjectManager.ScrumMaster);
                Assert.IsFalse(sentModel.ProjectManager.Developer);
            }

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
                    ProjectManager = new NewUser
                    {
                        EmailAddress = "test@agile.local",
                        FirstName = "Test",
                        LastName = "Test",
                        Password = "Test",
                        ProductOwner = false,
                        ScrumMaster = false,
                        Developer = false
                    }
                };

                gateway.CreateProject(testProject);
                Assert.IsNotNull(sentModel);
                gateway.DeleteProject(sentModel.ProjectId);
                Assert.IsNull(sentModel);
            }

            public void GetProject()
            {
            }

            public void GetProjects()
            {
            }
        }

        

    #endregion
    
}
