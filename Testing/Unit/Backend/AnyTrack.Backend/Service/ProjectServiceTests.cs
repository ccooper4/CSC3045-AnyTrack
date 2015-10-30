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
using AnyTrack.Backend.Faults;
using Project = AnyTrack.Backend.Data.Model.Project;

namespace Unit.Backend.AnyTrack.Backend.Service.ProjectServiceTests
{

    public class Context
    {
        public static IUnitOfWork unitOfWork;
        public static ProjectService service;

        [SetUp]
        public void ContextSetup()
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

        #region AddProject(Project project) Tests

        [Test]
        public void CreateNewProjectNoOtherRolesAssigned()
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

            service.AddProject(project);

            dataProject.Should().NotBeNull();
            dataProject.Name.Equals("Project");
            dataProject.Description.Equals("This is a new project");
            dataProject.VersionControl.Equals("queens.git");

            dataProject.ProjectManager.Should().NotBeNull();
            dataProject.ProjectManager.EmailAddress.Equals("John@test.com");
            dataProject.ProjectManager.FirstName.Equals("John");
            dataProject.ProjectManager.LastName.Equals("Test");
            dataProject.ProjectManager.Password.Equals("Password");
            dataProject.ProjectManager.Developer.Equals(false);
            dataProject.ProjectManager.ProductOwner.Equals(false);
            dataProject.ProjectManager.ScrumMaster.Equals(false);

            dataProject.ProductOwner.EmailAddress.Should().BeNull();
            dataProject.ScrumMasters.Count().Should().Be(0);
            dataProject.StartedOn.Should().Be(DateTime.Today);
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
            dataProject.Name.Equals("Project");
            dataProject.Description.Equals("This is a new project");
            dataProject.VersionControl.Equals("queens.git");

            dataProject.ProjectManager.Should().NotBeNull();
            dataProject.ProjectManager.EmailAddress.Equals("John@test.com");
            dataProject.ProjectManager.FirstName.Equals("John");
            dataProject.ProjectManager.LastName.Equals("Test");
            dataProject.ProjectManager.Password.Equals("Password");
            dataProject.ProjectManager.Developer.Equals(false);
            dataProject.ProjectManager.ProductOwner.Equals(false);
            dataProject.ProjectManager.ScrumMaster.Equals(false);

            dataProject.ProductOwner.Should().NotBeNull();
            dataProject.ProductOwner.EmailAddress.Equals("PO@test.com");
            dataProject.ProductOwner.FirstName.Equals("Julie");
            dataProject.ProductOwner.LastName.Equals("Test");
            dataProject.ProductOwner.Password.Equals("Password");
            dataProject.ProductOwner.Developer.Equals(false);
            dataProject.ProductOwner.ProductOwner.Equals(true);
            dataProject.ProductOwner.ScrumMaster.Equals(false);

            dataProject.ScrumMasters.Count().Should().Be(2);

            dataProject.ScrumMasters[0].Should().NotBeNull();
            dataProject.ScrumMasters[0].EmailAddress.Equals("S1@test.com");
            dataProject.ScrumMasters[0].FirstName.Equals("Jack");
            dataProject.ScrumMasters[0].LastName.Equals("Test");
            dataProject.ScrumMasters[0].Password.Equals("Password");
            dataProject.ScrumMasters[0].Developer.Equals(false);
            dataProject.ScrumMasters[0].ProductOwner.Equals(false);
            dataProject.ScrumMasters[0].ScrumMaster.Equals(true);

            dataProject.ScrumMasters[0].Should().NotBeNull();
            dataProject.ScrumMasters[0].EmailAddress.Equals("S2@test.com");
            dataProject.ScrumMasters[0].FirstName.Equals("Jane");
            dataProject.ScrumMasters[0].LastName.Equals("Test");
            dataProject.ScrumMasters[0].Password.Equals("Password");
            dataProject.ScrumMasters[0].Developer.Equals(false);
            dataProject.ScrumMasters[0].ProductOwner.Equals(true);
            dataProject.ScrumMasters[0].ScrumMaster.Equals(true);

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
    }
}
