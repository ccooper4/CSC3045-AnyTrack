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

        #endregion Given Steps

        #region When Steps
        #endregion When Steps

        #region Then Steps
        #endregion Then Steps
    }
}
