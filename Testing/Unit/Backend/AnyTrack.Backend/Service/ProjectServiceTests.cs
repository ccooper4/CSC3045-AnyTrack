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
using AnyTrack.Backend.Security;
using AnyTrack.Infrastructure.BackendAccountService;
using AnyTrack.Infrastructure.Security;
using Unit.Backend.AnyTrack.Backend.Service.PrincipalBuilderServiceTests;
using Project = AnyTrack.Backend.Data.Model.Project;

namespace Unit.Backend.AnyTrack.Backend.Service.ProjectServiceTests
{

    public class Context
    {
        public static IUnitOfWork unitOfWork;
        public static ProjectService service;
        public static FormsAuthenticationProvider provider;
        public static OperationContextProvider context;
        public static TestService testService;
        public static List<User> userList;
        public static List<Role> roleList;

        [SetUp]
        public void SetUp()
        {
            unitOfWork = Substitute.For<IUnitOfWork>();
            provider = Substitute.For<FormsAuthenticationProvider>();
            context = Substitute.For<OperationContextProvider>();
            service = new ProjectService(unitOfWork, provider, context);
           
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
                },
                new User
                {
                    EmailAddress = "S1@test.com",
                    FirstName = "Jack",
                    LastName = "Test",
                    Password = "Password",
                    Developer = false,
                    ProductOwner = false,
                    ScrumMaster = true,
                    Skills = "C#, Java",
                    SecretQuestion = "Where do you live?",
                    SecretAnswer = "A Tent"
                },
                new User
                {
                    EmailAddress = "S2@test.com",
                    FirstName = "Jane",
                    LastName = "Test",
                    Password = "Password",
                    Developer = false,
                    ProductOwner = true,
                    ScrumMaster = true,
                    Skills = "C#, Java",
                    SecretQuestion = "Where do you live?",
                    SecretAnswer = "A Tent"
                }
                #endregion
            };

            roleList = new List<Role>()
            {
                #region Test Data Roles
                
                new Role()
                {
                    ProjectID = new Guid("11223344-5566-7788-99AA-BBCCDDEEFF00"),
                    RoleName = "Project Manager",
                    User = userList[0]
                }
                
                #endregion
            };

            userList[0].Roles = new List<Role>(){roleList[0]};
        }
    }

    public class ProjectServiceTests : Context
    {
        #region Constructor Tests

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructProjectServiceNoUnitOfWork()
        {
            service = new ProjectService(null, provider, context);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructProjectServiceNoProvider()
        {
            service = new ProjectService(unitOfWork, null, context);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructProjectServiceNoContextProvider()
        {
            service = new ProjectService(unitOfWork, provider, null);
        }

        [Test]
        public void ConstructProjectService()
        {
            service = new ProjectService(unitOfWork, provider, context);

            service.Should().NotBeNull();
        }
        
        #endregion

        #region AddProject(ServiceProject project) Tests

        [Test]
        public void CreateNewProjectNoOtherRolesAssigned()
        {
            SetUpThreadCurrent();

            #region Test Data

            Project dataProject = null;

            ServiceProject project = new ServiceProject
            {
                Name = "Project",
                Description = "This is a new project",
                VersionControl = "queens.git",
                ProjectManagerEmailAddress = "tester@agile.local",
                StartedOn = DateTime.Today,
                ProductOwnerEmailAddress = "tester@agile.local"
            };

            #endregion

            unitOfWork.UserRepository.Items.Returns(userList.AsQueryable());
            unitOfWork.ProjectRepository.Items.Returns(new List<Project>().AsQueryable());
            unitOfWork.ProjectRepository.Insert(Arg.Do<Project>(p => dataProject = p));

            service.AddProject(project);

            #region Test Checks

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
            dataProject.ProjectManager.Skills.Should().Be("C#, Java");
            dataProject.ProjectManager.SecretQuestion.Should().Be("Where do you live?");
            dataProject.ProjectManager.SecretAnswer.Should().Be("At Home");
            dataProject.ProjectManager.Roles.Count.Should().Be(3);
            dataProject.ProjectManager.Roles.ToList()[1].RoleName.Should().Be("Project Manager");

            dataProject.ProductOwner.Should().NotBeNull();
            dataProject.ScrumMasters.Count().Should().Be(0);
            dataProject.StartedOn.Should().Be(DateTime.Today);

            unitOfWork.ProjectRepository.Received().Insert(dataProject);
            unitOfWork.Received().Commit();

           #endregion
        }

        [Test]
        public void CreateProjectAllRolesAssigned()
        {
            SetUpThreadCurrent();

            #region Test Data

            Project dataProject = null;

            ServiceProject project = new ServiceProject
            {
                Name = "Project",
                Description = "This is a new project",
                VersionControl = "queens.git",
                ProjectManagerEmailAddress = "tester@agile.local",
                ProductOwnerEmailAddress = "tester@agile.local",
                StartedOn = DateTime.Today
            };

            project.ScrumMasterEmailAddresses.Add("S1@test.com");
            project.ScrumMasterEmailAddresses.Add("S2@test.com");

            #endregion

            unitOfWork.UserRepository.Items.Returns(userList.AsQueryable());
            unitOfWork.ProjectRepository.Items.Returns(new List<Project>().AsQueryable());
            unitOfWork.ProjectRepository.Insert(Arg.Do<Project>(p => dataProject = p));

            service.AddProject(project);

            #region Test Checks

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
            dataProject.ProjectManager.Skills.Should().Be("C#, Java");
            dataProject.ProjectManager.SecretQuestion.Should().Be("Where do you live?");
            dataProject.ProjectManager.SecretAnswer.Should().Be("At Home");
            dataProject.ProjectManager.Roles.Count.Should().Be(3);
            dataProject.ProjectManager.Roles.ToList().Select(r => r.RoleName).Should().Contain("Project Manager");

            dataProject.ProductOwner.Should().NotBeNull();
            dataProject.ProductOwner.EmailAddress.Should().Be("tester@agile.local");
            dataProject.ProductOwner.FirstName.Should().Be("John");
            dataProject.ProductOwner.LastName.Should().Be("Test");
            dataProject.ProductOwner.Password.Should().Be("Password");
            dataProject.ProductOwner.Developer.Should().Be(false);
            dataProject.ProductOwner.ProductOwner.Should().Be(false);
            dataProject.ProductOwner.ScrumMaster.Should().Be(false);
            dataProject.ProductOwner.Skills.Should().Be("C#, Java");
            dataProject.ProductOwner.SecretQuestion.Should().Be("Where do you live?");
            dataProject.ProductOwner.SecretAnswer.Should().Be("At Home");
            dataProject.ProductOwner.Roles.Count.Should().Be(3);
            dataProject.ProductOwner.Roles.ToList().Select(r => r.RoleName).Should().Contain("Product Owner");

            dataProject.ScrumMasters.Count().Should().Be(2);

            dataProject.ScrumMasters[0].Should().NotBeNull();
            dataProject.ScrumMasters[0].EmailAddress.Should().Be("S1@test.com");
            dataProject.ScrumMasters[0].FirstName.Should().Be("Jack");
            dataProject.ScrumMasters[0].LastName.Should().Be("Test");
            dataProject.ScrumMasters[0].Password.Should().Be("Password");
            dataProject.ScrumMasters[0].Developer.Should().Be(false);
            dataProject.ScrumMasters[0].ProductOwner.Should().Be(false);
            dataProject.ScrumMasters[0].ScrumMaster.Should().Be(true);
            dataProject.ScrumMasters[0].Skills.Should().Be("C#, Java");
            dataProject.ScrumMasters[0].SecretQuestion.Should().Be("Where do you live?");
            dataProject.ScrumMasters[0].SecretAnswer.Should().Be("A Tent");
            dataProject.ScrumMasters[0].Roles.Count.Should().Be(1);
            dataProject.ScrumMasters[0].Roles.ToList().Select(r => r.RoleName).Should().Contain("Scrum Master");

            dataProject.ScrumMasters[1].Should().NotBeNull();
            dataProject.ScrumMasters[1].EmailAddress.Should().Be("S2@test.com");
            dataProject.ScrumMasters[1].FirstName.Should().Be("Jane");
            dataProject.ScrumMasters[1].LastName.Should().Be("Test");
            dataProject.ScrumMasters[1].Password.Should().Be("Password");
            dataProject.ScrumMasters[1].Developer.Should().Be(false);
            dataProject.ScrumMasters[1].ProductOwner.Should().Be(true);
            dataProject.ScrumMasters[1].ScrumMaster.Should().Be(true);
            dataProject.ScrumMasters[1].SecretQuestion.Should().Be("Where do you live?");
            dataProject.ScrumMasters[1].SecretAnswer.Should().Be("A Tent");
            dataProject.ScrumMasters[1].Roles.Count.Should().Be(1);
            dataProject.ScrumMasters[1].Roles.ToList().Select(r => r.RoleName).Should().Contain("Scrum Master");

            dataProject.StartedOn.Should().Be(DateTime.Today);
            #endregion
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateProjectAlreadyAdded()
        {
            SetUpThreadCurrent();

            #region Test Data

            Project dataProject = null;

            ServiceProject project = new ServiceProject
            {
                Name = "Project",
                Description = "This is a new project",
                VersionControl = "queens.git",
                ProjectManagerEmailAddress = "tester@agile.local",
                StartedOn = DateTime.Today
            };

            Project project2 = new Project
            {
                Name = "Project",
                Description = "This is a new project",
                VersionControl = "queens.git",
                ProjectManager = new User
                {
                    EmailAddress = "tester@agile.local"
                },
                StartedOn = DateTime.Today
            };

            project2.Id = project.ProjectId;

            List<Project> projects = new List<Project>();
            projects.Add(project2);

            #endregion

            unitOfWork.UserRepository.Items.Returns(userList.AsQueryable());
            unitOfWork.ProjectRepository.Insert(Arg.Do<Project>(p => dataProject = p));
            unitOfWork.ProjectRepository.Items.Returns(projects.AsQueryable());

            service.AddProject(project);
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

        [Test]
        public void UpdateProject()
        {
            #region Test Data

            List<Project> projectList = new List<Project>()
            {
                new Project
                {
                    Id = new Guid("11223344-5566-7788-99AA-BBCCDDEEFF00"),
                    Name = "Project",
                    Description = "This is a new project",
                    VersionControl = "queens.git",
                    ProjectManager = new User
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
                    StartedOn = DateTime.Today
                },
                new Project
                {
                    Id = Guid.NewGuid(),
                    Name = "Project2",
                    Description = "This is a new project",
                    VersionControl = "queens.git",
                    ProjectManager = new User
                    {
                        EmailAddress = "tester@agile.local"
                    },
                    StartedOn = DateTime.Today
                }
            };

            ServiceProject serviceProject = new ServiceProject
            {
                ProjectId = new Guid("11223344-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Updated Project Name",
                Description = "Updated Description",
                ProjectManagerEmailAddress = "tester@agile.local",
                ProductOwnerEmailAddress = "PO@test.com",
            };

            serviceProject.ScrumMasterEmailAddresses = new List<string>()
            {
                "S1@test.com"
            };

            #endregion

            unitOfWork.UserRepository.Items.Returns(userList.AsQueryable());
            unitOfWork.RoleRepository.Items.Returns(roleList.AsQueryable());
            unitOfWork.ProjectRepository.Items.Returns(projectList.AsQueryable());

            service.UpdateProject(serviceProject);

            projectList[0].Name.Should().Be("Updated Project Name");
            projectList[0].Description.Should().Be("Updated Description");
            projectList[0].ProjectManager.EmailAddress.Should().Be("tester@agile.local");
            projectList[0].ScrumMasters.Count.Should().Be(1);
            projectList[0].ScrumMasters[0].EmailAddress.Should().Be("S1@test.com");
        }

        #endregion

        #region DeleteProject(Guid projectId) Tests

        [Test]
        public void DeleteProject()
        {
            #region Test Data

            List<Project> projectList = new List<Project>()
            {
                new Project
                {
                    Id = new Guid("11223344-5566-7788-99AA-BBCCDDEEFF00"),
                    Name = "Project",
                    Description = "This is a new project",
                    VersionControl = "queens.git",
                    ProjectManager = new User
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
                    StartedOn = DateTime.Today
                },
                new Project
                {
                    Id = Guid.NewGuid(),
                    Name = "Project2",
                    Description = "This is a new project",
                    VersionControl = "queens.git",
                    ProjectManager = new User
                    {
                        EmailAddress = "tester@agile.local"
                    },
                    StartedOn = DateTime.Today
                }
            };

            #endregion

            Project project = projectList[0];
            unitOfWork.UserRepository.Items.Returns(userList.AsQueryable());
            unitOfWork.RoleRepository.Items.Returns(roleList.AsQueryable());
            unitOfWork.ProjectRepository.Items.Returns(projectList.AsQueryable());
            unitOfWork.ProjectRepository.Delete(Arg.Do<Project>(n => projectList.Remove(n)));

            service.DeleteProject(new Guid("11223344-5566-7788-99AA-BBCCDDEEFF00"));

            projectList.Should().NotContain(project);
        }

        #endregion

        #region GetProject(Guid projectId) Tests

        [Test]
        public void GetProject()
        {
            #region Test Data

            List<Project> projectList = new List<Project>()
            {
                new Project
                {
                    Id = Guid.NewGuid(),
                    Name = "Project",
                    Description = "This is a new project",
                    VersionControl = "queens.git",
                    ProjectManager = new User
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
                    StartedOn = DateTime.Today
                },
                new Project
                {
                    Id = Guid.NewGuid(),
                    Name = "Project2",
                    Description = "This is a new project",
                    VersionControl = "queens.git",
                    ProjectManager = new User
                    {
                        EmailAddress = "tester@agile.local"
                    },
                    StartedOn = DateTime.Today
                }

            };

            #endregion

            unitOfWork.ProjectRepository.Items.Returns(projectList.AsQueryable());

            ServiceProject project = service.GetProject(projectList[0].Id);

            #region Test checks

            project.ProjectId.Should().Be(projectList[0].Id);
            project.Name.Should().Be("Project");
            project.Description.Should().Be("This is a new project");
            project.VersionControl.Should().Be("queens.git");
            project.ProjectManagerEmailAddress.Should().Be("tester@agile.local");
            project.ProductOwnerEmailAddress.Should().BeNull();
            project.ScrumMasterEmailAddresses.Count.Should().Be(0);

            #endregion

        }

        [Test]
        [ExpectedException(typeof (ArgumentException))]
        public void GetProjectInvalidGuid()
        {
            #region Test Data

            List<Project> projectList = new List<Project>()
            {
                new Project
                {
                    Id = Guid.NewGuid(),
                    Name = "Project",
                    Description = "This is a new project",
                    VersionControl = "queens.git",
                    ProjectManager = new User
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
                    StartedOn = DateTime.Today
                },
                new Project
                {
                    Id = Guid.NewGuid(),
                    Name = "Project2",
                    Description = "This is a new project",
                    VersionControl = "queens.git",
                    ProjectManager = new User
                    {
                        EmailAddress = "tester@agile.local"
                    },
                    StartedOn = DateTime.Today
                }

            };

            #endregion

            unitOfWork.ProjectRepository.Items.Returns(projectList.AsQueryable());

            ServiceProject project = service.GetProject(Guid.Empty);
        }
        #endregion

        #region GetProjects() Tests

        [Test]
        public void GetProjects()
        {
            #region Test Data

            List<Project> projectList = new List<Project>()
            {
                new Project
                {
                    Id = Guid.NewGuid(),
                    Name = "Project",
                    Description = "This is a new project",
                    VersionControl = "queens.git",
                    ProjectManager = new User
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
                    StartedOn = DateTime.Today
                },
                new Project
                {
                    Id = Guid.NewGuid(),
                    Name = "Project2",
                    Description = "This is a new project",
                    VersionControl = "queens.git",
                    ProjectManager = new User
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
                    StartedOn = DateTime.Today
                }

            };

            #endregion

            unitOfWork.ProjectRepository.Items.Returns(projectList.AsQueryable());

            List<ServiceProject> project = service.GetProjects();

            #region Test checks

            project[0].ProjectId.Should().Be(projectList[0].Id);
            project[0].Name.Should().Be("Project");
            project[0].Description.Should().Be("This is a new project");
            project[0].VersionControl.Should().Be("queens.git");
            project[0].ProjectManagerEmailAddress.Should().Be("tester@agile.local");
            project[0].ProductOwnerEmailAddress.Should().BeNull();
            project[0].ScrumMasterEmailAddresses.Count.Should().Be(0);
            
            project[1].ProjectId.Should().Be(projectList[1].Id);
            project[1].Name.Should().Be("Project2");
            project[1].Description.Should().Be("This is a new project");
            project[1].VersionControl.Should().Be("queens.git");
            project[1].ProjectManagerEmailAddress.Should().Be("tester@agile.local");
            project[1].ProductOwnerEmailAddress.Should().BeNull();
            project[1].ScrumMasterEmailAddresses.Count.Should().Be(0);

            #endregion
        }
        
        #endregion

        #region GetProjectNames() Tests

        [Test]
        public void GetProjectNames()
        {
            #region Test Data

            var projectId1 = Guid.NewGuid();
            var projectId2 = Guid.NewGuid(); 

            User user = new User
            {
                EmailAddress = "tester@agile.local",
                FirstName = "John",
                LastName = "Test",
                Password = "Password",
                Developer = false,
                ProductOwner = true,
                ScrumMaster = false,
                Skills = "C#, Java",
                SecretQuestion = "Where do you live?",
                SecretAnswer = "At Home",
                Roles = new List<Role>() {new Role() {RoleName = "Product Owner", ProjectID = projectId1}}

            };
            
            User user2 = new User
            {
                EmailAddress = "tester2@agile.local",
                FirstName = "John",
                LastName = "Test",
                Password = "Password",
                Developer = false,
                ProductOwner = false,
                ScrumMaster = false,
                Skills = "C#, Java",
                SecretQuestion = "Where do you live?",
                SecretAnswer = "At Home",
                Roles = new List<Role>()
            };

            List<Project> projectList = new List<Project>()
            {
                new Project
                {
                    Id = projectId1,
                    Name = "Project",
                    Description = "This is a new project",
                    VersionControl = "queens.git",
                    ProjectManager = user,
                    StartedOn = DateTime.Today
                },
                new Project
                {
                    Id = projectId2,
                    Name = "Project2",
                    Description = "This is a new project",
                    VersionControl = "queens.git",
                    ProjectManager = user2,
                    StartedOn = DateTime.Today
                }
            };

            Thread.CurrentPrincipal = new GeneratedServiceUserPrincipal(user);

            var users = new List<User>() {user, user2};
            unitOfWork.UserRepository.Items.Returns(users.AsQueryable());

            #endregion

            unitOfWork.ProjectRepository.Items.Returns(projectList.AsQueryable());

            List<ProjectDetails> projectNames = service.GetProjectNames(false, true, false);

            #region Test checks

            projectNames.Count.Should().Be(1);

            #endregion
        }

        #endregion

        #region List<ProjectRoleSummary> GetUserProjectRoleSummaries(string currentUserEmailAddress)

        [Test]
        public void GetUserProjectRoleSummary()
        {
            #region Test Data

            List<User> users = new List<User>()
            {
                #region User Data
                new User()
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
                }
                #endregion
            };

            List<Role> roles = new List<Role>()
            {
                #region Role Data
                new Role()
                {
                    ProjectID = new Guid("11223344-5566-7788-99AA-BBCCDDEEFF00"),
                    RoleName = "Project Manager",
                    User = users[0]
                },
                new Role()
                {
                    ProjectID = new Guid("11223344-5566-7788-99AA-BBCCDDEEFF00"),
                    RoleName = "Scrum Master",
                    User = users[0]
                },
                new Role()
                {
                    ProjectID = new Guid("11223344-5566-7788-99AA-BBCCDDEEFFFF"),
                    RoleName = "Product Owner",
                    User = users[0]
                },
                new Role()
                {
                    ProjectID = new Guid("11223344-5566-7788-99AA-BBCCDDEEFFFF"),
                    RoleName = "Developer",
                    User = users[0]
                }
               #endregion
            };

            users[0].Roles = roles;

            List<Project> projects = new List<Project>()
            {
                #region Project Data
                new Project
                {
                    Id = new Guid("11223344-5566-7788-99AA-BBCCDDEEFF00"),
                    Name = "Project",
                    Description = "This is a new project",
                    VersionControl = "queens.git",
                    ProjectManager = users[0],
                    StartedOn = DateTime.Today
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
                    ProductOwner = users[0],
                    StartedOn = DateTime.Today
                }
                #endregion
            };

            projects[0].ScrumMasters = new List<User>() {users[0]};

            #endregion

            unitOfWork.UserRepository.Items.Returns(users.AsQueryable());
            unitOfWork.RoleRepository.Items.Returns(roles.AsQueryable());
            unitOfWork.ProjectRepository.Items.Returns(projects.AsQueryable());

            var result = service.GetUserProjectRoleSummaries("tester@agile.local");

            result.Should().NotBeNull();
            result.Count.Should().Be(2);
            result[0].ProjectId.Should().Be("11223344-5566-7788-99AA-BBCCDDEEFF00");
            result[0].Name.Should().Be("Project");
            result[0].Description.Should().Be("This is a new project");
            result[0].Developer.Should().BeFalse();
            result[0].ProductOwner.Should().BeFalse();
            result[0].ProjectManager.Should().BeTrue();
            result[0].ScrumMaster.Should().BeTrue();
            result[1].Name.Should().Be("Project2");
            result[1].Description.Should().Be("This is a new project2");
            result[1].Developer.Should().BeTrue();
            result[1].ProductOwner.Should().BeTrue();
            result[1].ProjectManager.Should().BeFalse();
            result[1].ScrumMaster.Should().BeFalse();
        }


        [Test]
        public void UserHasNoRoles()
        {
            #region Test Data

            List<User> users = new List<User>()
            {
                #region User Data
                new User()
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
                new User()
                {
                    EmailAddress = "PO@agile.local",
                    FirstName = "John",
                    LastName = "Test",
                    Password = "Password",
                    Developer = false,
                    ProductOwner = false,
                    ScrumMaster = false,
                    Skills = "C#, Java",
                    SecretQuestion = "Where do you live?",
                    SecretAnswer = "At Home"
                }
                #endregion
            };

            List<Role> roles = new List<Role>()
            {
                #region Role Data
                new Role()
                {
                    ProjectID = new Guid("11223344-5566-7788-99AA-BBCCDDEEFF00"),
                    RoleName = "Project Manager",
                    User = users[1]
                },
                new Role()
                {
                    ProjectID = new Guid("11223344-5566-7788-99AA-BBCCDDEEFF00"),
                    RoleName = "Scrum Master",
                    User = users[1]
                },
                new Role()
                {
                    ProjectID = new Guid("11223344-5566-7788-99AA-BBCCDDEEFFFF"),
                    RoleName = "Product Owner",
                    User = users[1]
                },
                new Role()
                {
                    ProjectID = new Guid("11223344-5566-7788-99AA-BBCCDDEEFFFF"),
                    RoleName = "Developer",
                    User = users[1]
                }
               #endregion
            };

            users[0].Roles = new List<Role>();

            #endregion

            unitOfWork.UserRepository.Items.Returns(users.AsQueryable());
            unitOfWork.RoleRepository.Items.Returns(roles.AsQueryable());

            var result = service.GetUserProjectRoleSummaries("tester@agile.local");

            result.Should().NotBeNull();
            result.Count.Should().Be(0);
        }
        #endregion

        #region SearchUsers(UserSearchFilter filter) Tests

        [Test]
        public void CallSearchUsersWithEmptyFilter()
        {
            var users = new List<User>()
            {
                new User { Id = Guid.NewGuid(), EmailAddress = "test1@mail.com", FirstName = "Andrew", LastName = "Fletcher", ScrumMaster = true, ProductOwner = true},
                new User { Id = Guid.NewGuid(), EmailAddress = "test1@mail.com", FirstName = "Liam", LastName = "Fletcher", ScrumMaster = false, ProductOwner = true},
                new User { Id = Guid.NewGuid(), EmailAddress = "test1@mail.com", FirstName = "David", LastName = "Tester", ScrumMaster = true, ProductOwner = false},
                new User { Id = Guid.NewGuid(), EmailAddress = "test1@mail.com", FirstName = "Bill", LastName = "Tester", ScrumMaster = false, ProductOwner = false}
            };

            unitOfWork.UserRepository.Items.Returns(users.AsQueryable());

            var userFilter = new UserSearchFilter();

            var result = service.SearchUsers(userFilter);

            result.Count.Should().Be(4);
            result.Select(r => r.FullName).Should().ContainInOrder("Andrew Fletcher", "Bill Tester", "David Tester", "Liam Fletcher");
            var firstResult = result.First();
            firstResult.FullName.Should().Be("Andrew Fletcher");
            firstResult.EmailAddress.Should().Be("test1@mail.com");
            firstResult.UserID.Should().Be(users.First().Id);

        }

        [Test]
        public void CallSearchUsersWithNoResults()
        {
            var users = new List<User>()
            {
                new User { Id = Guid.NewGuid(), EmailAddress = "test1@mail.com", FirstName = "Andrew", LastName = "Fletcher", ScrumMaster = true, ProductOwner = true},
                new User { Id = Guid.NewGuid(), EmailAddress = "test1@mail.com", FirstName = "Liam", LastName = "Fletcher", ScrumMaster = false, ProductOwner = true},
                new User { Id = Guid.NewGuid(), EmailAddress = "test1@mail.com", FirstName = "David", LastName = "Tester", ScrumMaster = true, ProductOwner = false},
                new User { Id = Guid.NewGuid(), EmailAddress = "test1@mail.com", FirstName = "Bill", LastName = "Tester", ScrumMaster = false, ProductOwner = false}
            };

            unitOfWork.UserRepository.Items.Returns(users.AsQueryable());

            var userFilter = new UserSearchFilter { EmailAddress = "none@mail.com" };

            var result = service.SearchUsers(userFilter);

            result.Count.Should().Be(0);
        }

        [Test]
        public void CallSearchFilterOnEmail()
        {
            var users = new List<User>()
            {
                new User { Id = Guid.NewGuid(), EmailAddress = "test1@mail.com", FirstName = "Andrew", LastName = "Fletcher", ScrumMaster = true, ProductOwner = true},
                new User { Id = Guid.NewGuid(), EmailAddress = "test2@mail.com", FirstName = "Liam", LastName = "Fletcher", ScrumMaster = false, ProductOwner = true},
                new User { Id = Guid.NewGuid(), EmailAddress = "test3@mail.com", FirstName = "David", LastName = "Tester", ScrumMaster = true, ProductOwner = false},
                new User { Id = Guid.NewGuid(), EmailAddress = "test4@mail.com", FirstName = "Bill", LastName = "Tester", ScrumMaster = false, ProductOwner = false}
            };

            unitOfWork.UserRepository.Items.Returns(users.AsQueryable());

            var userFilter = new UserSearchFilter { EmailAddress = "test4@mail.com" };

            var result = service.SearchUsers(userFilter);

            result.Count.Should().Be(1);
            var singleResult = result.Single();
            singleResult.FullName.Should().Be("Bill Tester");
            singleResult.EmailAddress.Should().Be("test4@mail.com");
            singleResult.UserID.Should().Be(users.Last().Id);
        }

        [Test]
        public void CallSearchFilterOnScrumMaster()
        {
            var users = new List<User>()
            {
                new User { Id = Guid.NewGuid(), EmailAddress = "test1@mail.com", FirstName = "Andrew", LastName = "Fletcher", ScrumMaster = true, ProductOwner = true},
                new User { Id = Guid.NewGuid(), EmailAddress = "test2@mail.com", FirstName = "Liam", LastName = "Fletcher", ScrumMaster = false, ProductOwner = true},
                new User { Id = Guid.NewGuid(), EmailAddress = "test3@mail.com", FirstName = "David", LastName = "Tester", ScrumMaster = true, ProductOwner = false},
                new User { Id = Guid.NewGuid(), EmailAddress = "test4@mail.com", FirstName = "Bill", LastName = "Tester", ScrumMaster = false, ProductOwner = false}
            };

            unitOfWork.UserRepository.Items.Returns(users.AsQueryable());

            var userFilter = new UserSearchFilter { ScrumMaster = true, ProductOwner = false };

            var result = service.SearchUsers(userFilter);

            result.Count.Should().Be(1);

            var userResult = result.Single();
            userResult.FullName.Should().Be("David Tester");
            userResult.EmailAddress.Should().Be("test3@mail.com");
            userResult.UserID.Should().Be(users[2].Id);

        }

        [Test]
        public void CallSearchFilterOnProductOwner()
        {
            var users = new List<User>()
            {
                new User { Id = Guid.NewGuid(), EmailAddress = "test1@mail.com", FirstName = "Andrew", LastName = "Fletcher", ScrumMaster = true, ProductOwner = true},
                new User { Id = Guid.NewGuid(), EmailAddress = "test2@mail.com", FirstName = "Liam", LastName = "Fletcher", ScrumMaster = false, ProductOwner = true},
                new User { Id = Guid.NewGuid(), EmailAddress = "test3@mail.com", FirstName = "David", LastName = "Tester", ScrumMaster = true, ProductOwner = false},
                new User { Id = Guid.NewGuid(), EmailAddress = "test4@mail.com", FirstName = "Bill", LastName = "Tester", ScrumMaster = false, ProductOwner = false}
            };

            unitOfWork.UserRepository.Items.Returns(users.AsQueryable());

            var userFilter = new UserSearchFilter { ScrumMaster = false, ProductOwner = true };

            var result = service.SearchUsers(userFilter);

            result.Count.Should().Be(1);

            var lastResult = result.Last();
            lastResult.FullName.Should().Be("Liam Fletcher");
            lastResult.EmailAddress.Should().Be("test2@mail.com");
            lastResult.UserID.Should().Be(users[1].Id);

        }

        #endregion 

        #region Helper Methods

        private void SetUpThreadCurrent()
        {
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
        }
       
        #endregion

        
    }
}
