using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;

namespace Acceptance.StepDefinitions
{
    [Binding]
    public sealed class NavigationSteps
    {
        #region Given Steps
        #endregion Given Steps

        #region When Steps
        /// <summary>
        /// Navigation helper to Projects
        /// </summary>
        [When(@"I click on the projects navigation button")]
        public void WhenIClickOnTheProjectsNavigationButton()
        {
            ScenarioContext.Current.Pending();
        }

        /// <summary>
        /// Clicking on pop-up menu to navigate to Project Management screen
        /// </summary>
        /// <param name="projectName">project name to be checked</param>
        [When(@"I click on the pop-up menu on the tile for '(.*)'")]
        public void WhenIClickOnThePop_UpMenuOnTheTileFor(string projectName)
        {
            ScenarioContext.Current.Pending();
        }

        /// <summary>
        /// Clicking on a project in planning poker to navigate to a given project backlog screen
        /// </summary>
        /// <param name="projectBacklogName">the name of the backlog screen to be checked</param>
        [When(@"I click on the project with name '(.*)'")]
        public void WhenIClickOnTheProjectWithName(string projectBacklogName)
        {
            ScenarioContext.Current.Pending();
        }
        #endregion When Steps

        #region Then Steps
        /// <summary>
        /// Navigation to Project Management Screen given a project
        /// </summary>
        /// <param name="projectName">project name to be checked</param>
        [Then(@"I am able to view the Project Management Screen via the '(.*)' option")]
        public void ThenIAmAbleToViewTheProjectManagementScreenViaTheOption(string projectName)
        {
            ScenarioContext.Current.Pending();
        }

        /// <summary>
        /// Navigation to the My Projects screen
        /// </summary>
        /// <param name="screen">the screen to navigate to</param>
        [Then(@"the '(.*)' screen is displayed")]
        public void ThenTheScreenIsDisplayed(string screen)
        {
            ScenarioContext.Current.Pending();
        }
        #endregion Then Steps
    }
}
