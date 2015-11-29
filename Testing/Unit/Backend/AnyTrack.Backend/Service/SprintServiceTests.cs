using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AnyTrack.Backend.Data;
using AnyTrack.Backend.Data.Model;
using AnyTrack.Backend.Providers;
using AnyTrack.Backend.Security;
using AnyTrack.Backend.Service;
using AnyTrack.Backend.Service.Model;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using Task = AnyTrack.Backend.Data.Model.Task;

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

        [Test]
        public void AddSprint()
        {
            #region Test Data

            ServiceSprint sprint = new ServiceSprint
            {
                SprintId = new Guid("00000001-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Sprint 1",
                StartDate = new DateTime(2015, 11, 26),
                EndDate = new DateTime(2015, 12, 3),
                Description = "Sprint",
                TeamEmailAddresses = new List<string>() {userList[0].EmailAddress, userList[1].EmailAddress}
            };

            #endregion

            unitOfWork.UserRepository.Items.Returns(userList.AsQueryable());
            unitOfWork.ProjectRepository.Items.Returns(projectList.AsQueryable());

            service.AddSprint(projectList[0].Id, sprint);

            sprint.Length.Should().Be(8);

            projectList[0].Sprints.Count.Should().Be(1);
            projectList[0].Sprints.First().Name.Should().Be("Sprint 1");
            projectList[0].Sprints.First().Description.Should().Be("Sprint");
            projectList[0].Sprints.First().StartDate.Should().Be(new DateTime(2015, 11, 26));
            projectList[0].Sprints.First().EndDate.Should().Be(new DateTime(2015, 12, 3));
            projectList[0].Sprints.First().Team.Count.Should().Be(2);

            userList[0].Roles.Last().ProjectId.Should().Be(projectList[0].Id);
            userList[0].Roles.Last().SprintId.Should().Be(projectList[0].Sprints.First().Id);
            userList[0].Roles.Last().RoleName.Should().Be("Developer");
            userList[0].Roles.Last().User.Should().Be(userList[0]);

            userList[1].Roles.Last().ProjectId.Should().Be(projectList[0].Id);
            userList[1].Roles.Last().SprintId.Should().Be(projectList[0].Sprints.First().Id);
            userList[1].Roles.Last().RoleName.Should().Be("Developer");
            userList[1].Roles.Last().User.Should().Be(userList[1]);
        }
        
        #endregion

        #region void EditSprint(Guid sprintId, ServiceSprint updatedSprint)

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void EditSprintNullProjectId()
        {
            #region Test Data

            Guid sprintId = Guid.Empty;

            ServiceSprint sprint = new ServiceSprint()
            {
                SprintId = Guid.NewGuid(),
                Name = "Sprint 1",
                Description = "Sprint",
                StartDate = new DateTime(2015, 11, 26),
                EndDate = new DateTime(2015, 12, 4),
                TeamEmailAddresses = new List<string>() { "p@q.com" }
            };

            #endregion

            service.AddSprint(sprintId, sprint);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void EditprintNullSprintService()
        {
            #region Test Data

            Guid sprintId = Guid.NewGuid();

            ServiceSprint sprint = null;

            #endregion

            service.EditSprint(sprintId, sprint);
        }

        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public void EditSprintProjectDoesNotExist()
        {
            #region Test Data

            ServiceSprint sprint = new ServiceSprint
            {
                SprintId = new Guid("00000001-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Sprint 1",
                StartDate = new DateTime(2015, 11, 26),
                EndDate = new DateTime(2015, 12, 3),
                Description = "Sprint",
                TeamEmailAddresses = new List<string>() { userList[0].EmailAddress, userList[1].EmailAddress }
            };

            Guid sprintId = Guid.Empty;

            #endregion

            unitOfWork.UserRepository.Items.Returns(userList.AsQueryable());
            unitOfWork.ProjectRepository.Items.Returns(projectList.AsQueryable());

            service.AddSprint(sprintId, sprint);
        }

        [Test]
        public void EditSprint()
        {
            #region Test Data

            List<Sprint> sprintList = new List<Sprint>()
            {
                new Sprint
                {
                    Id = new Guid("00000001-5566-7788-99AA-BBCCDDEEFF00"),
                    Name = "Sprint 1",
                    StartDate = new DateTime(2015, 11, 26),
                    EndDate = new DateTime(2015, 12, 3),
                    Description = "Sprint"
                }
            };

            List<Role> rolesList = new List<Role>()
            {
                new Role()
                {
                    Id = new Guid("00000000-5566-7788-99AA-BBCCDDEEFF00"),
                    ProjectId = new Guid("11223344-5566-7788-99AA-BBCCDDEEFF00"),
                    RoleName = "Developer",
                    SprintId = new Guid("00000001-5566-7788-99AA-BBCCDDEEFF00"),
                    User = userList[0]
                },
                new Role()
                {
                    Id = new Guid("00000000-5566-7788-99AA-BBCCDDEEFF00"),
                    ProjectId = new Guid("11223344-5566-7788-99AA-BBCCDDEEFF00"),
                    RoleName = "Developer",
                    SprintId = new Guid("00000001-5566-7788-99AA-BBCCDDEEFF00"),
                    User = userList[1]
                }
            };

            userList[0].Roles = new List<Role>(){rolesList[0]};
            userList[1].Roles = new List<Role>() { rolesList[1] };
            sprintList[0].Team = new List<User>(){userList[0], userList[1]};

            ServiceSprint sprint = new ServiceSprint
            {
                SprintId = new Guid("00000001-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Sprint Changed",
                StartDate = new DateTime(2015, 11, 26),
                EndDate = new DateTime(2015, 12, 9),
                Description = "Sprint Changed",
                TeamEmailAddresses = new List<string>() { userList[1].EmailAddress }
            };

            List<Project> projects = new List<Project>()
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
                    Sprints = new List<Sprint>(){sprintList[0]}
                }
                #endregion
            };

            #endregion

            unitOfWork.UserRepository.Items.Returns(userList.AsQueryable());
            unitOfWork.ProjectRepository.Items.Returns(projects.AsQueryable());
            unitOfWork.RoleRepository.Items.Returns(rolesList.AsQueryable());
            unitOfWork.SprintRepository.Items.Returns(sprintList.AsQueryable());

            projects[0].Sprints.First().Name.Should().Be("Sprint 1");
            projects[0].Sprints.First().StartDate.Should().Be(new DateTime(2015, 11, 26));
            projects[0].Sprints.First().EndDate.Should().Be(new DateTime(2015, 12, 3));
            projects[0].Sprints.First().Description.Should().Be("Sprint");
            projects[0].Sprints.First().Team.Count.Should().Be(2);
            projects[0].Sprints.First().Team.First().Should().Be(userList[0]);
            projects[0].Sprints.First().Team.Last().Should().Be(userList[1]);
            
            service.EditSprint(sprintList[0].Id, sprint);

            projects[0].Sprints.First().Name.Should().Be("Sprint Changed");
            projects[0].Sprints.First().StartDate.Should().Be(new DateTime(2015, 11, 26));
            projects[0].Sprints.First().EndDate.Should().Be(new DateTime(2015, 12, 9));
            projects[0].Sprints.First().Description.Should().Be("Sprint Changed");
            projects[0].Sprints.First().Team.Count.Should().Be(1);
            projects[0].Sprints.First().Team.First().Should().Be(userList[1]);

            userList[0].Roles.Count.Should().Be(0);
            userList[1].Roles.Count.Should().Be(1);
        }

        #endregion

        #region void GetAllTasksForSprint(Guid sprintId)

        [Test]
        public void GetAllTasksReturnsOneTask()
        {
            #region Test Data

            List<Sprint> sprintList = new List<Sprint>()
            {
                new Sprint
                {
                    Id = new Guid("00000001-5566-7788-99AA-BBCCDDEEFF00"),
                    Name = "Sprint 1",
                    StartDate = new DateTime(2015, 11, 26),
                    EndDate = new DateTime(2015, 12, 3),
                    Description = "Sprint"
                }
            };

            List<Task> tasksList = new List<Task>();
            Task t = new Task()
            {
                Assignee = userList.FirstOrDefault(),
                ConditionsOfSatisfaction = "asdsad",
                Description = "asd",
                TaskHourEstimates = new List<TaskHourEstimate>(),
                SprintStory = new SprintStory
                {
                    Sprint = sprintList.FirstOrDefault(),
                    Story = new Story
                    {
                        ConditionsOfSatisfaction = "asddas",
                        IWant = "adsasd",
                        Project = projectList.FirstOrDefault(),
                        SoThat = "asd",
                        Summary = "asdd"
                    }
                }
                
            };
            tasksList.Add(t);

            List<Role> rolesList = new List<Role>()
            {
                new Role()
                {
                    Id = new Guid("00000000-5566-7788-99AA-BBCCDDEEFF00"),
                    ProjectId = new Guid("11223344-5566-7788-99AA-BBCCDDEEFF00"),
                    RoleName = "Developer",
                    SprintId = new Guid("00000001-5566-7788-99AA-BBCCDDEEFF00"),
                    User = userList[0]
                },
                new Role()
                {
                    Id = new Guid("00000000-5566-7788-99AA-BBCCDDEEFF00"),
                    ProjectId = new Guid("11223344-5566-7788-99AA-BBCCDDEEFF00"),
                    RoleName = "Developer",
                    SprintId = new Guid("00000001-5566-7788-99AA-BBCCDDEEFF00"),
                    User = userList[1]
                }
            };

            userList.FirstOrDefault().Roles = rolesList;

            Thread.CurrentPrincipal = new GeneratedServiceUserPrincipal(userList.FirstOrDefault());

            #endregion
            unitOfWork.UserRepository.Items.Returns(userList.AsQueryable());
            unitOfWork.SprintRepository.Items.Returns(sprintList.AsQueryable());
            unitOfWork.RoleRepository.Items.Returns(rolesList.AsQueryable());
            unitOfWork.TaskRepository.Items.Returns(tasksList.AsQueryable());

            var result = service.GetAllTasksForSprint(sprintList.LastOrDefault().Id);

            result.Count.Should().Be(1);
        }

        [Test]
        public void GetAllTasksReturnsZero()
        {
            #region Test Data

            List<Sprint> sprintList = new List<Sprint>()
            {
                new Sprint
                {
                    Id = new Guid("00000001-5566-7788-99AA-BBCCDDEEFF00"),
                    Name = "Sprint 1",
                    StartDate = new DateTime(2015, 11, 26),
                    EndDate = new DateTime(2015, 12, 3),
                    Description = "Sprint"
                }
            };

            List<Task> tasksList = new List<Task>();

            List<Role> rolesList = new List<Role>()
            {
                new Role()
                {
                    Id = new Guid("00000000-5566-7788-99AA-BBCCDDEEFF00"),
                    ProjectId = new Guid("11223344-5566-7788-99AA-BBCCDDEEFF00"),
                    RoleName = "Developer",
                    SprintId = new Guid("00000001-5566-7788-99AA-BBCCDDEEFF00"),
                    User = userList[0]
                },
                new Role()
                {
                    Id = new Guid("00000000-5566-7788-99AA-BBCCDDEEFF00"),
                    ProjectId = new Guid("11223344-5566-7788-99AA-BBCCDDEEFF00"),
                    RoleName = "Developer",
                    SprintId = new Guid("00000001-5566-7788-99AA-BBCCDDEEFF00"),
                    User = userList[1]
                }
            };

            userList.FirstOrDefault().Roles = rolesList;

            Thread.CurrentPrincipal = new GeneratedServiceUserPrincipal(userList.FirstOrDefault());

            #endregion
            unitOfWork.UserRepository.Items.Returns(userList.AsQueryable());
            unitOfWork.SprintRepository.Items.Returns(sprintList.AsQueryable());
            unitOfWork.RoleRepository.Items.Returns(rolesList.AsQueryable());
            unitOfWork.TaskRepository.Items.Returns(tasksList.AsQueryable());

            var result = service.GetAllTasksForSprint(sprintList.LastOrDefault().Id);

            result.Count.Should().Be(0);
        }

        #endregion
    }
}
