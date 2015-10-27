using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;

namespace Acceptance.StepDefinitions
{
    [Binding]
    public sealed class PlanningPokerSteps
    {
        #region Given Steps
        #endregion Given Steps

        #region When Steps
        #endregion When Steps

        #region Then Steps
        /// <summary>
        /// Checks if a backlog screen for a given project is displayed
        /// </summary>
        /// <param name="backlogScreen">the screen to be checked if it appears</param>
        [Then(@"the '(.*)' is displayed")]
        public void ThenTheIsDisplayed(string backlogScreen)
        {
            ScenarioContext.Current.Pending();
        }

        /// <summary>
        /// Checks if the planning poker screen for a given project is displayed
        /// </summary>
        /// <param name="projectName">project name to be checked</param>
        [Then(@"the '(.*)' planning poker screen is displayed")]
        public void ThenThePlanningPokerScreenIsDisplayed(string projectName)
        {
            ScenarioContext.Current.Pending();
        }


        #endregion Then Steps
    }
}
