using System;
using TechTalk.SpecFlow;

namespace Acceptance.StepDefinitions
{
    /// <summary>
    /// This class contains generic steps which are not specific to any feature
    /// </summary>
    [Binding]
    public sealed class GenericSteps : Steps
    {
        #region Given Steps

        /// <summary>
        /// Logs a user in using the given email address and password
        /// </summary>
        /// <param name="emailAddress">users email address</param>
        /// <param name="password">users password</param>
        [Given(@"I am logged in as '(.*)' with the password '(.*)'")]
        public void GivenIAmLoggedInAsWithThePassword(string emailAddress, string password)
        {
            When("I open the system");

            var header = new[] { "Field", "Value" };
            Table userTable = new Table(header);
            userTable.AddRow("Email Address", String.Format("{0}", emailAddress));
            userTable.AddRow("Password", String.Format("{0}", password));
            
            When("I enter the following information:", userTable);
            When("I click 'Login'");
        }


        #endregion Given Steps

        #region When Steps
        /// <summary>
        /// Starts the software
        /// </summary>
        [When(@"I open the system")]
        public void WhenIOpenTheSystem()
        {
            ScenarioContext.Current.Pending();
        }

        /// <summary>
        /// Clicks the given element
        /// </summary>
        /// <param name="element">element to be clicked</param>
        [When(@"I click '(.*)'")]
        public void WhenIClick(string element)
        {
            ScenarioContext.Current.Pending();
        }

        /// <summary>
        /// Enters the given information into the fields specified
        /// </summary>
        /// <param name="table">Table of data to be entered</param>
        [When(@"I enter the following information:")]
        public void WhenIEnterTheFollowingInformation(Table table)
        {
            ScenarioContext.Current.Pending();
        }


        #endregion When Steps

        #region Then Steps

        /// <summary>
        /// Checks the given screen is displayed
        /// </summary>
        /// <param name="screenName">screen name to be checked</param>
        [Then(@"a '(.*)' screen is displayed")]
        public void ThenAScreenIsDisplayed(string screenName)
        {
            ScenarioContext.Current.Pending();
        }

        /// <summary>
        /// Checks the fields are displayed on the screen
        /// </summary>
        /// <param name="table">Table of fields</param>
        [Then(@"the following fields are displayed:")]
        public void ThenTheFollowingFieldsAreDisplayed(Table table)
        {
            ScenarioContext.Current.Pending();
        }

        /// <summary>
        /// Checks a given error message is displayed on the screen
        /// </summary>
        /// <param name="message"></param>
        [Then(@"an error message '(.*)' is displayed")]
        public void ThenAnErrorMessageIsDisplayed(string message)
        {
            ScenarioContext.Current.Pending();
        }


        #endregion Then Steps

        


        

        

        

    }
}
