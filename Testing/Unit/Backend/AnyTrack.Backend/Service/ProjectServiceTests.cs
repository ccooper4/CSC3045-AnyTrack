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
using System.ServiceModel;
using System.ServiceModel.Channels;
using AnyTrack.Backend.Faults;
using Unit.Backend.AnyTrack.Backend.Service.PrincipalBuilderServiceTests;
using Project = AnyTrack.Backend.Data.Model.Project;

namespace Unit.Backend.AnyTrack.Backend.Service.ProjectServiceTests
{

    public class Context
    {
        public static IUnitOfWork unitOfWork;
        public static ProjectService service;

        [SetUp]
        public void SetUp()
        {
            unitOfWork = Substitute.For<IUnitOfWork>();
            service = new ProjectService(unitOfWork);
        }
    }

    public class ProjectServiceTests : Context
    {
        #region Constructor Tests

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructProjectServiceNoUnitOfWork()
        {
            service = new ProjectService(null);
        }

        [Test]
        public void ConstructProjectService()
        {
            service = new ProjectService(unitOfWork);

            service.Should().NotBeNull();
        }
        
        #endregion

        #region AddProject(ServiceProject project) Tests

        [Test]
        public void CreateNewProjectNoOtherRolesAssigned()
        {
            #region Setup Thread.CurrentPrincipal

            FormsAuthenticationProvider provider = Substitute.For<FormsAuthenticationProvider>();
            OperationContextProvider context = Substitute.For<OperationContextProvider>();
            TestService testService;

            var channel = Substitute.For<IContextChannel>();
            var requestMessage = new HttpRequestMessageProperty();
            var authCookie = "test";
            var user = new User { EmailAddress = "tester@agile.local", Roles = new List<Role>() };
            requestMessage.Headers.Set("Set-Cookie", "AuthCookie=" + authCookie + ";other=other");

            var properties = new MessageProperties();
            properties.Add(HttpRequestMessageProperty.Name, requestMessage);
            context.IncomingMessageProperties.Returns(properties);

            var decryptedTicket = new FormsAuthenticationTicket("tester@agile.local", false, 100);
            provider.Decrypt(authCookie).Returns(decryptedTicket);

            unitOfWork.UserRepository.Items.Returns(new List<User>() { user }.AsQueryable());

            testService = new TestService(unitOfWork, provider, context);

            provider.Received().Decrypt(authCookie);

            #endregion

            #region Test Data

            List<User> userList = new List<User>()
            {
                new User 
                { 
                   EmailAddress = "tester@agile.local",
                   FirstName = "John",
                   LastName = "Test",
                   Password = "Password",
                   Developer = false,
                   ProductOwner = false,
                   ScrumMaster = false 
                }
            };

            Project dataProject = null;

            ServiceProject project = new ServiceProject
            {
                Name = "Project",
                Description = "This is a new project",
                VersionControl = "queens.git",
                ProjectManager = new NewUser
                {
                    EmailAddress = "tester@agile.local",
                    FirstName = "John",
                    LastName = "Test",
                    Password = "Password",
                    Developer = false,
                    ProductOwner = false,
                    ScrumMaster = false
                },
                StartedOn = DateTime.Today
            };

            #endregion

            #region Setup Fake Repos

            unitOfWork.UserRepository.Items.Returns(userList.AsQueryable());
            unitOfWork.ProjectRepository.Items.Returns(new List<Project>().AsQueryable());
            unitOfWork.ProjectRepository.Insert(Arg.Do<Project>(p => dataProject = p));

            #endregion

            #region Test Checks

           service.AddProject(project);

            dataProject.Should().NotBeNull();
            dataProject.Name.Should().Be("Project");
            dataProject.Description.Should().Be("This is a new project");
            dataProject.VersionControl.Should().Be("queens.git");

            dataProject.ProjectManager.Should().NotBeNull();
            dataProject.ProjectManager.EmailAddress.Should().Be("tester@agile.local");
            dataProject.ProjectManager.FirstName.Should().Be("John");
            dataProject.ProjectManager.LastName.Should().Be("Test");
            dataProject.ProjectManager.Password.Should().Be("Password");
            dataProject.ProjectManager.Developer.Should().Be(false);
            dataProject.ProjectManager.ProductOwner.Should().Be(false);
            dataProject.ProjectManager.ScrumMaster.Should().Be(false);

            dataProject.ProductOwner.Should().BeNull();
            dataProject.ScrumMasters.Count().Should().Be(0);
            dataProject.StartedOn.Should().Be(DateTime.Today);

            unitOfWork.ProjectRepository.Received().Insert(dataProject);
            unitOfWork.Received().Commit();

           #endregion
        }

        [Test]
        public void CreateProjectAllRolesAssigned()
        {
            Project dataProject = null;

            unitOfWork.ProjectRepository.Items.Returns(new List<Project>().AsQueryable());
            unitOfWork.ProjectRepository.Insert(Arg.Do<Project>(p => dataProject = p));

            ServiceProject project = new ServiceProject
            {
                Name = "Project",
                Description = "This is a new project",
                VersionControl = "queens.git",
                ProjectManager = new NewUser
                {
                    EmailAddress = "John@test.com",
                    FirstName = "John",
                    LastName = "Test",
                    Password = "Password",
                    Developer = false,
                    ProductOwner = false,
                    ScrumMaster = false
                },
                ProductOwner = new NewUser
                {
                    EmailAddress = "PO@test.com",
                    FirstName = "Julie",
                    LastName = "Test",
                    Password = "Password",
                    Developer = false,
                    ProductOwner = true,
                    ScrumMaster = false
                },
                StartedOn = DateTime.Today
            };

            project.ScrumMasters.Add(new NewUser
            {
                EmailAddress = "S1@test.com",
                FirstName = "Jack",
                LastName = "Test",
                Password = "Password",
                Developer = false,
                ProductOwner = false,
                ScrumMaster = true
            });

            project.ScrumMasters.Add(new NewUser
            {
                EmailAddress = "S2@test.com",
                FirstName = "Jane",
                LastName = "Test",
                Password = "Password",
                Developer = false,
                ProductOwner = true,
                ScrumMaster = true
            });

            service.AddProject(project);

            dataProject.Should().NotBeNull();
            dataProject.Name.Should().Be("Project");
            dataProject.Description.Should().Be("This is a new project");
            dataProject.VersionControl.Should().Be("queens.git");

            dataProject.ProjectManager.Should().NotBeNull();
            dataProject.ProjectManager.EmailAddress.Should().Be("John@test.com");
            dataProject.ProjectManager.FirstName.Should().Be("John");
            dataProject.ProjectManager.LastName.Should().Be("Test");
            dataProject.ProjectManager.Password.Should().Be("Password");
            dataProject.ProjectManager.Developer.Should().Be(false);
            dataProject.ProjectManager.ProductOwner.Should().Be(false);
            dataProject.ProjectManager.ScrumMaster.Should().Be(false);

            dataProject.ProductOwner.Should().NotBeNull();
            dataProject.ProductOwner.EmailAddress.Should().Be("PO@test.com");
            dataProject.ProductOwner.FirstName.Should().Be("Julie");
            dataProject.ProductOwner.LastName.Should().Be("Test");
            dataProject.ProductOwner.Password.Should().Be("Password");
            dataProject.ProductOwner.Developer.Should().Be(false);
            dataProject.ProductOwner.ProductOwner.Should().Be(true);
            dataProject.ProductOwner.ScrumMaster.Should().Be(false);

            dataProject.ScrumMasters.Count().Should().Be(2);

            dataProject.ScrumMasters[0].Should().NotBeNull();
            dataProject.ScrumMasters[0].EmailAddress.Should().Be("S1@test.com");
            dataProject.ScrumMasters[0].FirstName.Should().Be("Jack");
            dataProject.ScrumMasters[0].LastName.Should().Be("Test");
            dataProject.ScrumMasters[0].Password.Should().Be("Password");
            dataProject.ScrumMasters[0].Developer.Should().Be(false);
            dataProject.ScrumMasters[0].ProductOwner.Should().Be(false);
            dataProject.ScrumMasters[0].ScrumMaster.Should().Be(true);

            dataProject.ScrumMasters[1].Should().NotBeNull();
            dataProject.ScrumMasters[1].EmailAddress.Should().Be("S2@test.com");
            dataProject.ScrumMasters[1].FirstName.Should().Be("Jane");
            dataProject.ScrumMasters[1].LastName.Should().Be("Test");
            dataProject.ScrumMasters[1].Password.Should().Be("Password");
            dataProject.ScrumMasters[1].Developer.Should().Be(false);
            dataProject.ScrumMasters[1].ProductOwner.Should().Be(true);
            dataProject.ScrumMasters[1].ScrumMaster.Should().Be(true);

            dataProject.StartedOn.Should().Be(DateTime.Today);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateProjectAlreadyAdded()
        {
            Project dataProject = null;

            unitOfWork.ProjectRepository.Items.Returns(new List<Project>().AsQueryable());
            unitOfWork.ProjectRepository.Insert(Arg.Do<Project>(p => dataProject = p));

            ServiceProject project = new ServiceProject
            {
                Name = "Project",
                Description = "This is a new project",
                VersionControl = "queens.git",
                ProjectManager = new NewUser
                {
                    EmailAddress = "John@test.com",
                    FirstName = "John",
                    LastName = "Test",
                    Password = "Password",
                    Developer = false,
                    ProductOwner = false,
                    ScrumMaster = false
                },
                StartedOn = DateTime.Today
            };

            Project project2 = new Project
            {
                Name = "Project",
                Description = "This is a new project",
                VersionControl = "queens.git",
                ProjectManager = new User
                {
                    EmailAddress = "John@test.com",
                    FirstName = "John",
                    LastName = "Test",
                    Password = "Password",
                    Developer = false,
                    ProductOwner = false,
                    ScrumMaster = false
                },
                StartedOn = DateTime.Today
            };

            project2.Id = project.ProjectId;

            service.AddProject(project);

            List<Project> projects = new List<Project>();
            projects.Add(project2);
            unitOfWork.ProjectRepository.Items.Returns(projects.AsQueryable());
            // unitOfWork.ProjectRepository.Insert(Arg.Do<Project>(p => dataProject = p));
            service.AddProject(project);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateProjectNullProject()
        {
            Project dataProject = null;

            unitOfWork.ProjectRepository.Items.Returns(new List<Project>().AsQueryable());
            unitOfWork.ProjectRepository.Insert(Arg.Do<Project>(p => dataProject = p));

            ServiceProject project = null;

            service.AddProject(project);
        }
        #endregion

        #region UpdateProject(ServiceProject project) Tests



        #endregion
    }
}
