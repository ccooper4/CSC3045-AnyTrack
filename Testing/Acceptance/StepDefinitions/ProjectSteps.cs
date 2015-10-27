using TechTalk.SpecFlow;

namespace Acceptance.StepDefinitions
{
    /// <summary>
    /// This class contains steps specific to project features
    /// </summary>
    [Binding]
    public sealed class ProjectSteps : Steps
    {
        #region Given Steps

        /// <summary>
        /// Checks that the current logged in user has created a project with the given name
        /// </summary>
        /// <param name="projectName"></param>
        [Given(@"I have created a project called '(.*)'")]
        public void GivenIHaveCreatedAProjectCalled(string projectName)
        {
            ScenarioContext.Current.Pending();
        }

        /// <summary>
        /// Checks whether a specifed role has been assigned on a given project
        /// </summary>
        /// <param name="role">role to be checked</param>
        /// <param name="projectName">project to be checked</param>
        [Given(@"a '(.*)' has not been assigned to '(.*)'")]
        public void GivenAHasNotBeenAssignedTo(string role, string projectName)
        {
            ScenarioContext.Current.Pending();
        }

        /// <summary>
        /// Checkes whether or not the account has been assigned on a given project.
        /// </summary>
        /// <param name="email">email to be checked</param>
        /// <param name="projectName">project to be checked</param>
        [Given(@"the account '(.*)' has been assigned to '(.*)' project")]
        public void GivenTheAccountHasBeenAssignedToProject(string email, string projectName)
        {
            ScenarioContext.Current.Pending();
        }

        /// <summary>
        /// Checks the given role has been assigned on the specified project
        /// </summary>
        /// <param name="role">role to be checked</param>
        /// <param name="projectName">name of project to be checked</param>
        [Given(@"a '(.*)' has been assigned to '(.*)'")]
        public void GivenAHasBeenAssignedTo(string role, string projectName)
        {
            ScenarioContext.Current.Pending();
        }

        #endregion Given Steps

        #region When Steps
        #endregion When Steps

        #region Then Steps
        /// <summary>
        /// Checks if a given user has a given role in the specified project
        /// </summary>
        /// <param name="email">email of user to be checked</param>
        /// <param name="role">user role to be checked</param>
        /// <param name="projectName">project name to be checked</param>
        [Given(@"'(.*)' is a '(.*)' of the '(.*)' Project")]
        [Then(@"'(.*)' is a '(.*)' of the '(.*)' Project")]
        public void ThenIsAOfTheProject(string email, string role, string projectName)
        {
            ScenarioContext.Current.Pending();
        }

        /// <summary>
        /// Checks if the project manager can be viewed
        /// </summary>
        /// <param name="p"></param>
        [Then(@"I am able to view the project manager via the '(.*)' option")]
        public void ThenIAmAbleToViewTheProjectManagerViaTheOption(string p0)
        {
            ScenarioContext.Current.Pending();
        }

        /// <summary>
        /// Checking tile display for given project
        /// </summary>
        /// <param name="projectName">project name being checked</param>
        [Then(@"I am presented with a tile for '(.*)'")]
        public void ThenIAmPresentedWithATileFor(string projectName)
        {
            ScenarioContext.Current.Pending();
        }
        #endregion Then Steps
    }
}
