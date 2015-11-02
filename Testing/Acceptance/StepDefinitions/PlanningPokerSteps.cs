using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;

namespace Acceptance.StepDefinitions
{
    /// <summary>
    /// This class contains steps specific to planning poker features
    /// </summary>
    [Binding]
    public sealed class PlanningPokerSteps
    {
        #region Given Steps
        #endregion Given Steps

        #region When Steps
        /// <summary>
        /// Checking when string is entered into the send message field.
        /// </summary>
        /// <param name="sendString">message to be sent</param>
        [When(@"I enter the following text to the send message field: '(.*)'")]
        public void WhenIEnterTheFollowingTextToTheSendMessageField(string sendString)
        {
            ScenarioContext.Current.Pending();
        }

        /// <summary>
        /// Checking a story to include it in the planning poker
        /// </summary>
        /// <param name="storyName">story to be checked</param>
        [When(@"I check the checkbox for '(.*)' to include it in the planning poker")]
        public void WhenICheckTheCheckboxForToIncludeItInThePlanningPoker(string storyName)
        {
            ScenarioContext.Current.Pending();
        }
        #endregion When Steps

        #region Then Steps
        /// <summary>
        /// Checks if the planning poker screen for a given project is displayed
        /// </summary>
        /// <param name="projectName">project name to be checked</param>
        [Then(@"the '(.*)' planning poker screen is displayed")]
        public void ThenThePlanningPokerScreenIsDisplayed(string projectName)
        {
            ScenarioContext.Current.Pending();
        }

        /// <summary>
        /// Checks if a message can be sent in the planning poker session
        /// </summary>
        /// <param name="sendString">message to be sent</param>
        /// <param name="projectName">project name to be checked</param>
        [Then(@"the message '(.*)' is displayed in '(.*)' planning poker active session")]
        public void ThenTheMessageIsDisplayedInPlanningPokerActiveSession(string sendString, string projectName)
        {
            ScenarioContext.Current.Pending();
        }

        /// <summary>
        /// Checking that the estimate of a story can be shown during planning poker
        /// </summary>
        /// <param name="storyName">the name of the story to be checked</param>
        /// <param name="estimateValue">the value of the estimate</param>
        [Then(@"the following message is displayed '(.*)' : '(.*)'")]
        public void ThenTheFollowingMessageIsDisplayed(string storyName, string estimateValue)
        {
            ScenarioContext.Current.Pending();
        }


        #endregion Then Steps
    }
}
