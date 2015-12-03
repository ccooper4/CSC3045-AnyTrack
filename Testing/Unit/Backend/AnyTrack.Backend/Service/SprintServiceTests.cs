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
using System.Threading;
using AnyTrack.Infrastructure;
using AnyTrack.Infrastructure.Security;
using ServiceLoginResult = AnyTrack.Infrastructure.BackendAccountService.ServiceLoginResult;

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
                    Roles = new List<Role>()
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
                    SecretAnswer = "A car",
                    Roles = new List<Role>()
                },
                new User
                {
                    EmailAddress = "sm@test.com",
                    FirstName = "Peter",
                    LastName = "Test",
                    Password = "Password",
                    Developer = false,
                    ProductOwner = true,
                    ScrumMaster = false,
                    Skills = "C#",
                    SecretQuestion = "Where do you live?",
                    SecretAnswer = "A car",
                    Roles = new List<Role>()
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

            var principal = new GeneratedServiceUserPrincipal(userList[2]);
            Thread.CurrentPrincipal = principal;

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

            userList[2].Roles.Last().ProjectId.Should().Be(projectList[0].Id);
            userList[2].Roles.Last().SprintId.Should().Be(projectList[0].Sprints.First().Id);
            userList[2].Roles.Last().RoleName.Should().Be("Scrum Master");
            userList[2].Roles.Last().User.Should().Be(userList[2]);
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

        #region void GetAllTasksForSprintCurrentUser(Guid sprintId)

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

            List<TaskHourEstimate> taskHourEstimateList = new List<TaskHourEstimate>
            {
               new TaskHourEstimate
               {
                   Task = t,
                   Estimate = 3
               }
            };

            t.TaskHourEstimate = taskHourEstimateList;
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
            unitOfWork.TaskHourEstimateRepository.Items.Returns(taskHourEstimateList.AsQueryable());

            var result = service.GetAllTasksForSprintCurrentUser(sprintList.LastOrDefault().Id);

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
            List<TaskHourEstimate> taskHourEstimateList = new List<TaskHourEstimate>();

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
            unitOfWork.TaskHourEstimateRepository.Items.Returns(taskHourEstimateList.AsQueryable());

            var result = service.GetAllTasksForSprintCurrentUser(sprintList.LastOrDefault().Id);

            result.Count.Should().Be(0);
        }

        #endregion

        #region void SaveUpdatedTaskHours(List<ServiceTask> tasks)

        [Test]
        public void SaveUpdatedHoursForExistingTask()
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
                Id = new Guid("00000011-5566-7788-99AA-BBCCDDEEFF00"),
                Assignee = userList.FirstOrDefault(),
                ConditionsOfSatisfaction = "asdsad",
                Description = "asd",
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

            List<TaskHourEstimate> taskHourEstimateList = new List<TaskHourEstimate>
            {
               new TaskHourEstimate
               {
                   Task = t,
                   Estimate = 3
               }
            };

            t.TaskHourEstimate = taskHourEstimateList;
            tasksList.Add(t);

            List<ServiceTask> serviceTasksList = new List<ServiceTask>();
            ServiceTask serviceTask = new ServiceTask()
            {
                TaskId = new Guid("00000011-5566-7788-99AA-BBCCDDEEFF00"),
                ConditionsOfSatisfaction = "asdsad",
                Description = "asd",
                SprintStoryId = new Guid("00000001-5566-7788-99AA-BBCCDDEEFF00")
            };

            List<ServiceTaskHourEstimate> serviceTaskHourEstimateList = new List<ServiceTaskHourEstimate>
            {
               new ServiceTaskHourEstimate
               {
                   TaskId = serviceTask.TaskId,
                   Estimate = 3,
                   NewEstimate = 1
               }
            };

            serviceTask.TaskHourEstimates = serviceTaskHourEstimateList;
            serviceTasksList.Add(serviceTask);

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
            unitOfWork.TaskHourEstimateRepository.Items.Returns(taskHourEstimateList.AsQueryable());

            service.SaveUpdatedTaskHours(serviceTasksList);

            unitOfWork.TaskHourEstimateRepository.Items.Count().Should().Be(2);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SaveUpdatedHoursForNonExistingTaskReturnsError()
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
            

            List<TaskHourEstimate> taskHourEstimateList = new List<TaskHourEstimate>
            {
               new TaskHourEstimate
               {
                   Estimate = 3
               }
            };
            

            List<ServiceTask> serviceTasksList = new List<ServiceTask>();
            ServiceTask serviceTask = new ServiceTask()
            {
                TaskId = new Guid("00000011-5566-7788-99AA-BBCCDDEEFF00"),
                ConditionsOfSatisfaction = "asdsad",
                Description = "asd",
                SprintStoryId = new Guid("00000001-5566-7788-99AA-BBCCDDEEFF00")
            };

            List<ServiceTaskHourEstimate> serviceTaskHourEstimateList = new List<ServiceTaskHourEstimate>
            {
               new ServiceTaskHourEstimate
               {
                   TaskId = serviceTask.TaskId,
                   Estimate = 3,
                   NewEstimate = 1
               }
            };

            serviceTask.TaskHourEstimates = serviceTaskHourEstimateList;
            serviceTasksList.Add(serviceTask);

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
            unitOfWork.TaskHourEstimateRepository.Items.Returns(taskHourEstimateList.AsQueryable());

            service.SaveUpdatedTaskHours(serviceTasksList);
        }

        #endregion

        #region GetSprintNames(Guid? projectId, bool scrumMaster, bool developer) Tests

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void GetSprintNamesAndCannotFindUser()
        {
            unitOfWork.UserRepository.Items.Returns(new List<User>().AsQueryable());

            var user = new User
            {
                EmailAddress = "test@agile.local",
                Roles = new List<Role>()
                {
                    new Role { RoleName = "Developer", SprintId = Guid.NewGuid() },
                    new Role { RoleName = "Scrum Master", SprintId = Guid.NewGuid() }
                }
            };

            var serviceUserPrincipal = new GeneratedServiceUserPrincipal(user);
            Thread.CurrentPrincipal = serviceUserPrincipal;

            service.GetSprintNames(null, true, true);
        }

        [Test]
        public void GetSprintsNamesForUserForScrumMaster()
        {
            var sprint1Id = Guid.NewGuid();
            var sprint2Id = Guid.NewGuid();

            var user = new User
            {
                EmailAddress = "test@agile.local",
                Roles = new List<Role>()
                {
                    new Role { RoleName = "Developer", SprintId = sprint1Id },
                    new Role { RoleName = "Scrum Master", SprintId = sprint2Id }
                }
            };

            unitOfWork.UserRepository.Items.Returns(new List<User>() { user }.AsQueryable());

            var serviceUserPrincipal = new GeneratedServiceUserPrincipal(user);
            Thread.CurrentPrincipal = serviceUserPrincipal;

            var sprints = new List<Sprint>()
            {
                new Sprint { Id = sprint1Id, Name = "Sprint 1", Description = "UI"},
                new Sprint { Id = sprint2Id, Name = "Sprint 2", Description = "Backend"}
            };

            unitOfWork.SprintRepository.Items.Returns(sprints.AsQueryable());

            var results = service.GetSprintNames(null, true, false);

            results.Should().NotBeNull();
            results.Count.Should().Be(1);
            var singleResult = results.Single();
            singleResult.SprintId.Should().Be(sprint2Id);
            singleResult.Name.Should().Be(sprints.Last().Name);
            singleResult.Description.Should().Be(sprints.Last().Description);
        }

        [Test]
        public void GetSprintsNamesForUserForDeveloper()
        {
            var sprint1Id = Guid.NewGuid();
            var sprint2Id = Guid.NewGuid();

            var user = new User
            {
                EmailAddress = "test@agile.local",
                Roles = new List<Role>()
                {
                    new Role { RoleName = "Developer", SprintId = sprint1Id },
                    new Role { RoleName = "Scrum Master", SprintId = sprint2Id }
                }
            };

            unitOfWork.UserRepository.Items.Returns(new List<User>() { user }.AsQueryable());

            var serviceUserPrincipal = new GeneratedServiceUserPrincipal(user);
            Thread.CurrentPrincipal = serviceUserPrincipal;

            var sprints = new List<Sprint>()
            {
                new Sprint { Id = sprint1Id, Name = "Sprint 1", Description = "UI"},
                new Sprint { Id = sprint2Id, Name = "Sprint 2", Description = "Backend"}
            };

            unitOfWork.SprintRepository.Items.Returns(sprints.AsQueryable());

            var results = service.GetSprintNames(null, false, true);

            results.Should().NotBeNull();
            results.Count.Should().Be(1);
            var singleResult = results.Single();
            singleResult.SprintId.Should().Be(sprint1Id);
            singleResult.Name.Should().Be(sprints.First().Name);
            singleResult.Description.Should().Be(sprints.First().Description);
        }

        [Test]
        public void GetSprintsNamesForUserForBothRoles()
        {
            var sprint1Id = Guid.NewGuid();
            var sprint2Id = Guid.NewGuid();

            var user = new User
            {
                EmailAddress = "test@agile.local",
                Roles = new List<Role>()
                {
                    new Role { RoleName = "Developer", SprintId = sprint1Id },
                    new Role { RoleName = "Scrum Master", SprintId = sprint2Id }
                }
            };

            unitOfWork.UserRepository.Items.Returns(new List<User>() { user }.AsQueryable());

            var serviceUserPrincipal = new GeneratedServiceUserPrincipal(user);
            Thread.CurrentPrincipal = serviceUserPrincipal;

            var sprints = new List<Sprint>()
            {
                new Sprint { Id = sprint1Id, Name = "Sprint 1", Description = "UI"},
                new Sprint { Id = sprint2Id, Name = "Sprint 2", Description = "Backend"},
                new Sprint { Id = Guid.NewGuid(), Name = "Other sprint", Description = "Backend"}
            };

            unitOfWork.SprintRepository.Items.Returns(sprints.AsQueryable());

            var results = service.GetSprintNames(null, true, true);

            results.Should().NotBeNull();
            results.Count.Should().Be(2);
            var firstResult = results.First();
            firstResult.SprintId.Should().Be(sprint1Id);
            firstResult.Name.Should().Be(sprints.First().Name);
            firstResult.Description.Should().Be(sprints.First().Description);

            var secondResult = results.Last();
            secondResult.SprintId.Should().Be(sprint2Id);
            secondResult.Name.Should().Be(sprints[1].Name);
            secondResult.Description.Should().Be(sprints[1].Description);

            results.Select(s => s.Name).Should().NotContain("Other sprint");
        }

        [Test]
        public void GetSprintsNamesForUserAndBoundByProjectId()
        {
            var sprint1Id = Guid.NewGuid();
            var sprint2Id = Guid.NewGuid();
            var projectId = Guid.NewGuid();

            var user = new User
            {
                EmailAddress = "test@agile.local",
                Roles = new List<Role>()
                {
                    new Role { RoleName = "Developer", ProjectId = projectId, SprintId = sprint1Id },
                    new Role { RoleName = "Scrum Master", ProjectId = Guid.NewGuid(), SprintId = sprint2Id }
                }
            };

            unitOfWork.UserRepository.Items.Returns(new List<User>() { user }.AsQueryable());

            var serviceUserPrincipal = new GeneratedServiceUserPrincipal(user);
            Thread.CurrentPrincipal = serviceUserPrincipal;

            var sprints = new List<Sprint>()
            {
                new Sprint { Id = sprint1Id, Name = "Sprint 1", Description = "UI"},
                new Sprint { Id = sprint2Id, Name = "Sprint 2", Description = "Backend"},
                new Sprint { Id = Guid.NewGuid(), Name = "Other sprint", Description = "Backend"}
            };

            unitOfWork.SprintRepository.Items.Returns(sprints.AsQueryable());

            var results = service.GetSprintNames(projectId, true, true);

            results.Should().NotBeNull();
            results.Count.Should().Be(1);
            var firstResult = results.First();
            firstResult.SprintId.Should().Be(sprint1Id);
            firstResult.Name.Should().Be(sprints.First().Name);
            firstResult.Description.Should().Be(sprints.First().Description);

            results.Select(s => s.Name).Should().NotContain("Other sprint");
            results.Select(s => s.Name).Should().NotContain("Sprint 2");
        }

        #endregion 

        #region void GetSprintStories(Guid sprintId)

        [Test]
        public void GetSprintStories()
        {
            var currentPrincipal = new ServiceUserPrincipal(new ServiceLoginResult { EmailAddress = "tester@agile.local" }, "");

            UserDetailsStore.LoggedInUserPrincipal = currentPrincipal;

            ServiceSprint sprint = new ServiceSprint()
            {
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
                            IWant="I Want 1",
                            SoThat="So That 1",
                            Summary="Summary 1",
                            InSprint=true
                        }
                    }
                }           
            };

            List<Sprint> sprintList = new List<Sprint>()
            {
                new Sprint
                {
                    Id = new Guid("00000001-5566-7788-99AA-BBCCDDEEFF00"),
                    Name = "Sprint 1",
                    StartDate = new DateTime(2015, 11, 26),
                    EndDate = new DateTime(2015, 12, 3),
                    Description = "Sprint",
                    Backlog = new List<SprintStory>()
                    {
                        new SprintStory()
                        {
                            Id = new Guid("00000001-5566-7788-99AA-BBCCDDEEFF00"),
                            Story = new Story()
                            {
                                Id = new Guid("30000001-5566-7788-99AA-BBCCDDEEFF00"),
                                AsA = "As A 1",
                                ConditionsOfSatisfaction = "Conditions of Sat",
                                IWant="I Want 1",
                                SoThat="So That 1",
                                Summary="Summary 1",
                                InSprint=true
                            }
                        }
                    }
                }
            };

            unitOfWork.ProjectRepository.Items.Returns(projectList.AsQueryable());
            unitOfWork.SprintRepository.Items.Returns(sprintList.AsQueryable());
            unitOfWork.UserRepository.Items.Returns(userList.AsQueryable());
            
            service.AddSprint(projectList[0].Id, sprint);
            
            List<ServiceSprintStory> serv = service.GetSprintStories(sprint.SprintId);

            serv.First().SprintStoryId.Should().Be(new Guid("00000001-5566-7788-99AA-BBCCDDEEFF00"));
            serv.First().Story.StoryId.Should().Be(new Guid("30000001-5566-7788-99AA-BBCCDDEEFF00"));
            serv.First().Story.InSprint.Should().Be(true);
            serv.First().Story.AsA.Should().Be("As A 1");
            serv.First().Story.ConditionsOfSatisfaction.Should().Be("Conditions of Sat");
            serv.First().Story.IWant.Should().Be("I Want 1");
            serv.First().Story.SoThat.Should().Be("So That 1");
            serv.First().Story.Summary.Should().Be("Summary 1");
        }

        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public void GetSprintStoriesException()
        {
            ServiceSprint sprint = new ServiceSprint()
            {
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
                            IWant="I Want 1",
                            SoThat="So That 1",
                            Summary="Summary 1"
                        }
                    }
                }
            };

            List<Sprint> sprintList = new List<Sprint>()
            {
                new Sprint
                {
                    Id = new Guid("00000001-5566-7788-99AA-BBCCDDEEFF00"),
                    Name = "Sprint 1",
                    StartDate = new DateTime(2015, 11, 26),
                    EndDate = new DateTime(2015, 12, 3),
                    Description = "Sprint",
                    Backlog = new List<SprintStory>()
                    {
                        new SprintStory()
                        {
                            Id = new Guid("00000001-5566-7788-99AA-BBCCDDEEFF00"),
                            Story = new Story()
                            {
                                Id = new Guid("30000001-5566-7788-99AA-BBCCDDEEFF00"),
                                AsA = "As A 1",
                                ConditionsOfSatisfaction = "Conditions of Sat",
                                IWant="I Want 1",
                                SoThat="So That 1",
                                Summary="Summary 1",
                            }
                        }
                    }
                }
            };

            unitOfWork.ProjectRepository.Items.Returns(projectList.AsQueryable());
            unitOfWork.SprintRepository.Items.Returns(sprintList.AsQueryable());

            service.AddSprint(projectList[0].Id, sprint);

            List<ServiceSprintStory> serv = service.GetSprintStories(Guid.Empty);
        }
        #endregion

        #region void ManageSprintBacklog()

        [Test]
        public void ManageSprintBacklog()
        {
            #region Service Layer

            ServiceProject project = new ServiceProject
            {
                Name = "Project",
                Description = "This is a new project",
                VersionControl = "queens.git",
                ProjectManagerEmailAddress = "tester@agile.local",
                StartedOn = DateTime.Today,
                ProductOwnerEmailAddress = "tester@agile.local",
                Stories = new List<ServiceStory>()
                {
                    new ServiceStory()
                    {
                        StoryId = new Guid("80000001-5566-7788-99AA-BBCCDDEEFF00"),
                        AsA = "As A 1",
                        ConditionsOfSatisfaction = "Conditions of Sat",
                        IWant = "I Want 1",
                        SoThat = "So That 1",
                        Summary = "Summary 1",
                        InSprint = false
                    },
                    new ServiceStory()
                    {
                        StoryId = new Guid("90000001-5566-7788-99AA-BBCCDDEEFF00"),
                        AsA = "As A 2",
                        ConditionsOfSatisfaction = "Conditions of Sat",
                        IWant = "I Want 2",
                        SoThat = "So That 2",
                        Summary = "Summary 2",
                        InSprint = true
                    }
                }   
            };

            ServiceSprint sprint = new ServiceSprint()
            {
                SprintId = new Guid("60000001-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Sprint 1",
                StartDate = new DateTime(2015, 11, 26),
                EndDate = new DateTime(2015, 12, 3),
                Description = "Sprint",
                Backlog = new List<ServiceSprintStory>()
            };

            List<ServiceSprintStory> serviceSprintStory = new List<ServiceSprintStory>()
            {
                new ServiceSprintStory()
                {
                    SprintId = new Guid("60000001-5566-7788-99AA-BBCCDDEEFF00"),
                    SprintStoryId = new Guid("50000001-5566-7788-99AA-BBCCDDEEFF00"),
                    Story = new ServiceStory()
                    {
                        StoryId = new Guid("80000001-5566-7788-99AA-BBCCDDEEFF00"),
                        AsA = "As A 1",
                        ConditionsOfSatisfaction = "Conditions of Sat",
                        IWant = "I Want 1",
                        SoThat = "So That 1",
                        Summary = "Summary 1",
                        InSprint = true
                    }
                },
                new ServiceSprintStory()
                {
                    SprintId = new Guid("60000001-5566-7788-99AA-BBCCDDEEFF00"),
                    SprintStoryId = new Guid("20000001-5566-7788-99AA-BBCCDDEEFF00"),
                    Story = new ServiceStory()
                    {
                        StoryId = new Guid("90000001-5566-7788-99AA-BBCCDDEEFF00"),
                        AsA = "As A 1",
                        ConditionsOfSatisfaction = "Conditions of Sat",
                        IWant = "I Want 1",
                        SoThat = "So That 1",
                        Summary = "Summary 1",
                        InSprint = true
                    }
                }
            };

            #endregion

            #region Data Layer

            List<Story> storyList = new List<Story>()
            {
                new Story()
                {
                    Id = new Guid("80000001-5566-7788-99AA-BBCCDDEEFF00"),
                    AsA = "As A 1",
                    ConditionsOfSatisfaction = "Conditions of Sat",
                    IWant = "I Want 1",
                    SoThat = "So That 1",
                    Summary = "Summary 1",
                    InSprint = true
                },
                new Story()
                {
                    Id = new Guid("90000001-5566-7788-99AA-BBCCDDEEFF00"),
                    AsA = "As A 1",
                    ConditionsOfSatisfaction = "Conditions of Sat",
                    IWant = "I Want 1",
                    SoThat = "So That 1",
                    Summary = "Summary 1",
                    InSprint = true
                }
            };

            List<SprintStory> sprintStoryList = new List<SprintStory>()
            {
                new SprintStory()
                {
                    Id = new Guid("50000001-5566-7788-99AA-BBCCDDEEFF00"),
                    Story = storyList[0]
                }
            };

            List<Sprint> sprintList = new List<Sprint>()
            {
                new Sprint
                {
                    Id = new Guid("60000001-5566-7788-99AA-BBCCDDEEFF00"),
                    Name = "Sprint 1",
                    StartDate = new DateTime(2015, 11, 26),
                    EndDate = new DateTime(2015, 12, 3),
                    Description = "Sprint",
                    Backlog = sprintStoryList
                }
            };

            #endregion

            #region Repositories
            unitOfWork.ProjectRepository.Items.Returns(projectList.AsQueryable());
            unitOfWork.SprintRepository.Items.Returns(sprintList.AsQueryable());
            unitOfWork.StoryRepository.Items.Returns(storyList.AsQueryable());
            unitOfWork.SprintStoryRepository.Items.Returns(sprintStoryList.AsQueryable());
            #endregion

            service.AddSprint(projectList[0].Id, sprint);
            sprintList.First().Backlog.Count().Should().Be(1);
            service.ManageSprintBacklog(projectList[0].Id, sprint.SprintId, serviceSprintStory);
            sprintList.First().Backlog.Count().Should().Be(2);
        }
        #endregion
    }
}