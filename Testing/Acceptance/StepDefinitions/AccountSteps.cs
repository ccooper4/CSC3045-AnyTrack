using TechTalk.SpecFlow;

namespace Acceptance.StepDefinitions
{
    /// <summary>
    /// This class contains steps specific to user account functionality
    /// </summary>
    [Binding]
    public sealed class AccountSteps : Steps
    {
        #region Given Steps

        /// <summary>
        /// Checks that no account exists that is associated with the given email
        /// </summary>
        /// <param name="emailAddress">Email address to be checked</param>
        [Given(@"there is no account associated with the email address '(.*)'")]
        [Then(@"there is no account is associated with the email address '(.*)'")]
        public void GivenThereIsNoAccountAssociatedWithTheEmailAddress(string emailAddress)
        {
            ScenarioContext.Current.Pending();
        }

        /// <summary>
        /// Checks that an account exists that is associated with the given email
        /// </summary>
        /// <param name="emailAddress">Email address to be checked</param>
        [Given(@"an account associated with the email address '(.*)' exists")]
        [Then(@"an account associated with the email address '(.*)' exists")]
        public void GivenAnAccountAssociatedWithTheEmailExists(string emailAddress)
        {
            ScenarioContext.Current.Pending();
        }

        #endregion Given Steps

        #region When Steps

        #endregion WhenSteps

        #region Then Steps

        /// <summary>
        /// Checks the number of accounts associated with the given email address
        /// </summary>
        /// <param name="numberOfAccounts">Number of accounts</param>
        /// <param name="emailAddress">Email Address to be checked</param>
        [Then(@"'(.*)' account is associated with the email address '(.*)'")]
        public void ThenAccountIsAssociatedWithTheEmailAddress(int numberOfAccounts, string emailAddress)
        {
            ScenarioContext.Current.Pending();
        }

        /// <summary>
        /// Checks the given user has the following preferred roles
        /// </summary>
        /// <param name="emailAddress">users email address</param>
        /// <param name="rolesTable">roles to be checked</param>
        [Then(@"the '(.*)' account has the following preferred roles:")]
        public void ThenTheAccountHasTheFollowingPreferredRoles(string emailAddress, Table rolesTable)
        {
            ScenarioContext.Current.Pending();
        }

        #endregion ThenSteps

    }
}
