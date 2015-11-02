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
        [Then(@"I click '(.*)'")]
        [When(@"I click '(.*)'")]
        public void WhenIClick(string element)
        {
            ScenarioContext.Current.Pending();
        }

        /// <summary>
        /// Clicks a given option from the popup menu
        /// </summary>
        /// <param name="optionName">Name of option on popup menu</param>
        [When(@"I click '(.*)' from the pop­up menu")]
        public void WhenIClickFromThePopUpMenu(string optionName)
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

        /// <summary>
        /// Manage options of a given project
        /// </summary>
        /// <param name="optionName">option name to be checked</param>
        /// <param name="projectName">project name to be checked</param>
        [When(@"I click '(.*)' from the pop­up menu of '(.*)' project")]
        public void WhenIClickFromThePopUpMenuOfProject(string optionName, string projectName)
        {
            ScenarioContext.Current.Pending();
        }

        /// <summary>
        /// Enters the given text into the given search box element
        /// </summary>
        /// <param name="searchMessage">what to be searched for</param>
        /// <param name="searchBoxName">search box to enter the search request into</param>
        [When(@"I enter '(.*)' into the '(.*)' search box")]
        public void WhenIEnterIntoTheSearchBox(string searchMessage, string searchBoxName)
        {
            ScenarioContext.Current.Pending();
        }

        /// <summary>
        /// Add a user of given name from a search result
        /// </summary>
        /// <param name="name">users name from result</param>
        [When(@"I add '(.*)' from the results")]
        public void WhenIAddFromTheResults(string name)
        {
            ScenarioContext.Current.Pending();
        }      

        #endregion When Steps

        #region Then Steps

        /// <summary>
        /// Checks the given screen is displayed
        /// </summary>
        /// <param name="screenName">screen name to be checked</param>
        [Then(@"the '(.*)' screen is displayed")]
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

        /// <summary>
        /// Checks a given message is displayed on the screen
        /// </summary>
        /// <param name="message"></param>
        [Then(@"a '(.*)' message is displayed")]
        public void ThenAMessageIsDisplayed(string message)
        {
            ScenarioContext.Current.Pending();
        }


        /// <summary>
        /// Checks that the given search results is displayed
        /// </summary>
        /// <param name="searchResult">Table of expected search results</param>
        [Then(@"the following result is displayed:")]
        public void ThenTheFollowingResultIsDisplayed(Table searchResult)
        {
            ScenarioContext.Current.Pending();
        }

        /// <summary>
        /// Checks that no results are returned from last search
        /// </summary>
        [Then(@"no results are found")]
        public void ThenNoResultsAreFound()
        {
            ScenarioContext.Current.Pending();
        }

        /// <summary>
        /// Being unable to click an element due to permissions
        /// </summary>
        /// <param name="element">element to be clicked</param>
        [Then(@"I cannot click '(.*)'")]
        public void ThenICannotClick(string element)
        {
            ScenarioContext.Current.Pending();
        }


        #endregion Then Steps










    }
}
