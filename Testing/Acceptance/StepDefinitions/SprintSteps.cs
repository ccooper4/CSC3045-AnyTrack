using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;

namespace Acceptance.StepDefinitions
{
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

    }
}
