using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;

namespace Acceptance.StepDefinitions
{
    /// <summary>
    /// This class contains steps specific to story features
    /// </summary>
    [Binding]
    public sealed class StorySteps
    {
        #region Given Steps
        #endregion Given Steps

        #region When Steps
        /// <summary>
        /// Clicking on a story to assign / unassign user
        /// </summary>
        /// <param name="storyName"></param>
        [When(@"I click on '(.*)' story")]
        public void WhenIClickOnStory(string storyName)
        {
            ScenarioContext.Current.Pending();
        }

        /// <summary>
        /// Checking that story tabs for planning poker can be selected
        /// </summary>
        /// <param name="storyName">story name to be checked</param>
        [When(@"the '(.*)' story tab is selected")]
        public void WhenTheStoryTabIsSelected(string storyName)
        {
            ScenarioContext.Current.Pending();
        }

        #endregion When Steps

        #region Then Steps
        /// <summary>
        /// Checking that stories appear in the table
        /// </summary>
        /// <param name="table"></param>

        [Then(@"the following story information is displayed:")]
        public void ThenTheFollowingInformationIsDisplayed(Table table)
        {
            ScenarioContext.Current.Pending();
        }

        /// <summary>
        /// Checking the estimate value of a story
        /// </summary>
        /// <param name="storyName">the storyName to be checked</param>
        /// <param name="estimateValue">the estimate value to be applied</param>
        [Then(@"the estimate value of '(.*)' is '(.*)'")]
        public void ThenTheEstimateValueOfIs(string storyName, int estimateValue)
        {
            ScenarioContext.Current.Pending();
        }

        /// <summary>
        /// Checking the assigning of user to stories
        /// </summary>
        /// <param name="userName">the user name to be checked</param>
        /// <param name="storyName">the story name to be checked</param>
        /// <param name="roleName">the role name to be checked</param>
        [Then(@"I assign '(.*)' with the role '(.*)'")]
        [Then(@"'(.*)' is now assigned to '(.*)' with the role '(.*)'")]
        public void ThenIAssignTo(string userName, string storyName, string roleName)
        {
            ScenarioContext.Current.Pending();
        }

        /// <summary>
        /// Checking the unassigning of user from stories
        /// </summary>
        /// <param name="userName">the user name to be checked</param>
        /// <param name="storyName">the story name to be checked</param>
        /// <param name="roleName">the role name to be checked</param>
        [Then(@"I unassign '(.*)' from '(.*)' with the role '(.*)'")]
        [Then(@"'(.*)' is now unassigned from '(.*)' with the role '(.*)'")]
        public void ThenIsNowUnassignedFromWithTheRole(string userName, string storyName, string roleName)
        {
            ScenarioContext.Current.Pending();
        }
        #endregion Then Steps
    }
}
