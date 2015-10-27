using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;

namespace Acceptance.StepDefinitions
{
    /// <summary>
    /// This class contains steps specific to sprint features
    /// </summary>
    [Binding]
    public sealed class SprintSteps
    {
        /// <summary>
        /// Checking a story exists on a sprint within a project
        /// </summary>
        /// <param name="projectName">the project name to be checked</param>
        /// <param name="sprintName">the sprint name to be checked</param>
        /// <param name="storyName">the story name to be checked</param>
        [Given(@"the '(.*)' project has a sprint '(.*)' which contains a story called '(.*)'")]
        public void GivenTheProjectHasASprintWhichContainsAStoryCalled(string projectName, string sprintName, string storyName)
        {
            ScenarioContext.Current.Pending();
        }

        #region When Steps
        #endregion When Steps

        #region Then Steps
        /// <summary>
        /// Adding a sprint to a project
        /// </summary>
        /// <param name="sprintName">the name of sprint to be created</param>
        /// <param name="projectName">the project name to be checked</param>
        [Then(@"the sprint '(.*)' has been created in the '(.*)' project")]
        public void ThenTheSprintHasBeenCreatedInTheProject(string sprintName, string projectName)
        {
            ScenarioContext.Current.Pending();
        }
        #endregion Then Steps
    }
}
