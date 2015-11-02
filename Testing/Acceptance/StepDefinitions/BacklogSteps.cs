using TechTalk.SpecFlow;

namespace Acceptance.StepDefinitions
{
    /// <summary>
    /// This class contains steps specific to backlog features
    /// </summary>
    [Binding]
    public sealed class BacklogSteps : Steps
    {
        #region Given Steps
        #endregion Given Steps

        #region When Steps
        #endregion When Steps

        #region Then Steps
        /// <summary>
        /// Checking that stories are in sprint backlog
        /// </summary>
        /// <param name="storyName">the story name to be checked</param>
        /// <param name="sprintName">the sprint backlog</param>
        [Then(@"the '(.*)' is in the '(.*)'")]
        public void ThenTheIsInThe(string storyName, string sprintName)
        {
            ScenarioContext.Current.Pending();
        }

        /// <summary>
        /// Checking that stories are in product backlogs
        /// </summary>
        /// <param name="storyName">the story name to be checked</param>
        /// <param name="productName">the product backlog</param>
        [Then(@"the '(.*)' is now in the '(.*)'")]
        public void ThenTheIsNowInThe(string storyName, string productName)
        {
            ScenarioContext.Current.Pending();
        }

        /// <summary>
        /// Checking stories being moved from product backlog to sprint backlog
        /// </summary>
        /// <param name="storyName">story name to be checked</param>
        /// <param name="productName">moving from product backlog</param>
        /// <param name="sprintName">moving to sprint backlog</param>
        [Then(@"I move '(.*)' from '(.*)' and move it to '(.*)'")]
        public void ThenIMoveFromAndMoveItTo(string storyName, string productName, string sprintName)
        {
            ScenarioContext.Current.Pending();
        }

        /// <summary>
        /// Checking stories being moved from sprint backlog to product backlog
        /// </summary>
        /// <param name="storyName">story name to be checked</param>
        /// <param name="sprintName">moving from sprint backlog</param>
        /// <param name="productName">moving to product backlog</param>
        [Then(@"I move '(.*)' from '(.*)' and move it back to '(.*)'")]
        public void ThenIMoveFromAndMoveItBackTo(string storyName, string sprintName, string productName)
        {
            ScenarioContext.Current.Pending();
        }
        #endregion Then Steps
    }
}
