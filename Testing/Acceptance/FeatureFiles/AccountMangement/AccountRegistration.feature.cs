﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:1.9.0.77
//      SpecFlow Generator Version:1.9.0.0
//      Runtime Version:4.0.30319.42000
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace Acceptance.FeatureFiles.AccountMangement
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.9.0.77")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("Account Registration")]
    public partial class AccountRegistrationFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "AccountRegistration.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Account Registration", "\r\nAs a user of the scrum client program \r\nI can register \r\nSo that I can connect " +
                    "to the scrum management server", ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [NUnit.Framework.TestFixtureTearDownAttribute()]
        public virtual void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [NUnit.Framework.SetUpAttribute()]
        public virtual void TestInitialize()
        {
        }
        
        [NUnit.Framework.TearDownAttribute()]
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioSetup(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioStart(scenarioInfo);
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Happy Path Registration")]
        public virtual void HappyPathRegistration()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Happy Path Registration", ((string[])(null)));
#line 7
this.ScenarioSetup(scenarioInfo);
#line 9
 testRunner.Given("there is no account associated with the email address \'user@test.com\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 10
 testRunner.When("I open the system", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 11
 testRunner.And("I click \'Sign Up\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 12
 testRunner.Then("a \'Registration\' screen is displayed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Required"});
            table1.AddRow(new string[] {
                        "Email Address",
                        "Yes"});
            table1.AddRow(new string[] {
                        "Full Name",
                        "Yes"});
            table1.AddRow(new string[] {
                        "Password",
                        "Yes"});
            table1.AddRow(new string[] {
                        "Confirm Password",
                        "Yes"});
            table1.AddRow(new string[] {
                        "Skill Set",
                        "No"});
#line 13
 testRunner.And("the following fields are displayed:", ((string)(null)), table1, "And ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table2.AddRow(new string[] {
                        "Email Address",
                        "user@test.com"});
            table2.AddRow(new string[] {
                        "Full Name",
                        "David Tester"});
            table2.AddRow(new string[] {
                        "Password",
                        "Password"});
            table2.AddRow(new string[] {
                        "Confirm Password",
                        "Password"});
            table2.AddRow(new string[] {
                        "Skill Set",
                        "Java, C#, WPF"});
#line 23
 testRunner.When("I enter the following information:", ((string)(null)), table2, "When ");
#line 30
 testRunner.And("I click \'Register\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 32
 testRunner.Then("a \'Registration - Success\' screen is displayed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 33
 testRunner.And("an account associated with the email address \'user@test.com\' exists", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("User account has already been created for given email")]
        public virtual void UserAccountHasAlreadyBeenCreatedForGivenEmail()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("User account has already been created for given email", ((string[])(null)));
#line 35
this.ScenarioSetup(scenarioInfo);
#line 37
 testRunner.Given("an account associated with the email address \'user@test.com\' exists", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 38
 testRunner.When("I open the system", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 39
 testRunner.And("I click \'Sign Up\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 40
 testRunner.Then("a \'Registration\' screen is displayed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Required"});
            table3.AddRow(new string[] {
                        "Email Address",
                        "Yes"});
            table3.AddRow(new string[] {
                        "Full Name",
                        "Yes"});
            table3.AddRow(new string[] {
                        "Password",
                        "Yes"});
            table3.AddRow(new string[] {
                        "Confirm Password",
                        "Yes"});
            table3.AddRow(new string[] {
                        "Skill Set",
                        "No"});
#line 41
 testRunner.And("the following fields are displayed:", ((string)(null)), table3, "And ");
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table4.AddRow(new string[] {
                        "Email Address",
                        "user@test.com"});
            table4.AddRow(new string[] {
                        "Full Name",
                        "David Tester"});
            table4.AddRow(new string[] {
                        "Password",
                        "Password"});
            table4.AddRow(new string[] {
                        "Confirm Password",
                        "Password"});
            table4.AddRow(new string[] {
                        "Skill Set",
                        "C#, C++, Linux"});
#line 49
 testRunner.When("I enter the following information:", ((string)(null)), table4, "When ");
#line 56
 testRunner.And("I click \'Register\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 58
 testRunner.Then("an error message \'A user account with this email already exists\' is displayed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 59
 testRunner.And("\'1\' account is associated with the email address \'user@test.com\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("User tries to register an account with passwords that do not match")]
        public virtual void UserTriesToRegisterAnAccountWithPasswordsThatDoNotMatch()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("User tries to register an account with passwords that do not match", ((string[])(null)));
#line 61
this.ScenarioSetup(scenarioInfo);
#line 63
 testRunner.Given("there is no account associated with the email address \'user@test.com\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 64
 testRunner.When("I open the system", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 65
 testRunner.And("I click \'Sign Up\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 66
 testRunner.Then("a \'Registration\' screen is displayed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Required"});
            table5.AddRow(new string[] {
                        "Email Address",
                        "Yes"});
            table5.AddRow(new string[] {
                        "Full Name",
                        "Yes"});
            table5.AddRow(new string[] {
                        "Password",
                        "Yes"});
            table5.AddRow(new string[] {
                        "Confirm Password",
                        "Yes"});
            table5.AddRow(new string[] {
                        "Skill Set",
                        "No"});
#line 67
 testRunner.And("the following fields are displayed:", ((string)(null)), table5, "And ");
#line hidden
            TechTalk.SpecFlow.Table table6 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table6.AddRow(new string[] {
                        "Email Address",
                        "user@test.com"});
            table6.AddRow(new string[] {
                        "Full Name",
                        "David Tester"});
            table6.AddRow(new string[] {
                        "Password",
                        "Password"});
            table6.AddRow(new string[] {
                        "Confirm Password",
                        "Password1"});
            table6.AddRow(new string[] {
                        "Skill Set",
                        "C#, Scrum, Java, Html"});
#line 75
 testRunner.When("I enter the following information:", ((string)(null)), table6, "When ");
#line 82
 testRunner.And("I click \'Register\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 84
 testRunner.Then("an error message \'Passwords do not match\' is displayed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 85
 testRunner.And("there is no account is associated with the email address \'user@test.com\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("User tries to register an invalid email address")]
        public virtual void UserTriesToRegisterAnInvalidEmailAddress()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("User tries to register an invalid email address", ((string[])(null)));
#line 87
this.ScenarioSetup(scenarioInfo);
#line 89
 testRunner.When("I open the system", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 90
 testRunner.And("I click \'Sign Up\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 91
 testRunner.Then("a \'Registration\' screen is displayed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table7 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Required"});
            table7.AddRow(new string[] {
                        "Email Address",
                        "Yes"});
            table7.AddRow(new string[] {
                        "Full Name",
                        "Yes"});
            table7.AddRow(new string[] {
                        "Password",
                        "Yes"});
            table7.AddRow(new string[] {
                        "Confirm Password",
                        "Yes"});
            table7.AddRow(new string[] {
                        "Skill Set",
                        "No"});
#line 92
 testRunner.And("the following fields are displayed:", ((string)(null)), table7, "And ");
#line hidden
            TechTalk.SpecFlow.Table table8 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table8.AddRow(new string[] {
                        "Email Address",
                        "user.test.com"});
            table8.AddRow(new string[] {
                        "Full Name",
                        "David Tester"});
            table8.AddRow(new string[] {
                        "Password",
                        "Password"});
            table8.AddRow(new string[] {
                        "Confirm Password",
                        "Password"});
            table8.AddRow(new string[] {
                        "Skill Set",
                        "C#, Scrum, Java, Html"});
#line 100
 testRunner.When("I enter the following information:", ((string)(null)), table8, "When ");
#line 107
 testRunner.And("I click \'Register\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 108
 testRunner.Then("an error message \'Email address is not valid\' is displayed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 109
 testRunner.And("there is no account is associated with the email address \'user.test.com\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("User doesn\'t specify skill set at registration")]
        public virtual void UserDoesnTSpecifySkillSetAtRegistration()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("User doesn\'t specify skill set at registration", ((string[])(null)));
#line 111
this.ScenarioSetup(scenarioInfo);
#line 113
 testRunner.Given("there is no account associated with the email address \'user@test.com\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 114
 testRunner.When("I open the system", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 115
 testRunner.And("I click \'Sign Up\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 116
 testRunner.Then("a \'Registration\' screen is displayed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table9 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Required"});
            table9.AddRow(new string[] {
                        "Email Address",
                        "Yes"});
            table9.AddRow(new string[] {
                        "Full Name",
                        "Yes"});
            table9.AddRow(new string[] {
                        "Password",
                        "Yes"});
            table9.AddRow(new string[] {
                        "Confirm Password",
                        "Yes"});
            table9.AddRow(new string[] {
                        "Skill Set",
                        "No"});
#line 117
 testRunner.And("the following fields are displayed:", ((string)(null)), table9, "And ");
#line hidden
            TechTalk.SpecFlow.Table table10 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table10.AddRow(new string[] {
                        "Email Address",
                        "user@test.com"});
            table10.AddRow(new string[] {
                        "Full Name",
                        "David Tester"});
            table10.AddRow(new string[] {
                        "Password",
                        "Password"});
            table10.AddRow(new string[] {
                        "Confirm Password",
                        "Password"});
#line 125
 testRunner.When("I enter the following information:", ((string)(null)), table10, "When ");
#line 131
 testRunner.And("I click \'Register\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 133
 testRunner.Then("a \'Registration - Success\' screen is displayed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 134
 testRunner.And("an account associated with the email address \'user@test.com\' exists", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Assigning Roles at Registration Happy Path")]
        public virtual void AssigningRolesAtRegistrationHappyPath()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Assigning Roles at Registration Happy Path", ((string[])(null)));
#line 141
 this.ScenarioSetup(scenarioInfo);
#line 142
 testRunner.Given("there is no account associated with the email address \'po@test.com\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 143
 testRunner.When("I open the system", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 144
 testRunner.And("I click \'Sign Up\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 145
 testRunner.Then("a \'Registration\' screen is displayed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table11 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Required"});
            table11.AddRow(new string[] {
                        "Email Address",
                        "Yes"});
            table11.AddRow(new string[] {
                        "Full Name",
                        "Yes"});
            table11.AddRow(new string[] {
                        "Password",
                        "Yes"});
            table11.AddRow(new string[] {
                        "Confirm Password",
                        "Yes"});
            table11.AddRow(new string[] {
                        "Skill Set",
                        "No"});
            table11.AddRow(new string[] {
                        "Product Owner",
                        "No"});
            table11.AddRow(new string[] {
                        "Scrum Master",
                        "No"});
            table11.AddRow(new string[] {
                        "Developer",
                        "No"});
#line 146
 testRunner.And("the following fields are displayed:", ((string)(null)), table11, "And ");
#line hidden
            TechTalk.SpecFlow.Table table12 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table12.AddRow(new string[] {
                        "Email Address",
                        "po@test.com"});
            table12.AddRow(new string[] {
                        "Full Name",
                        "David Tester"});
            table12.AddRow(new string[] {
                        "Password",
                        "Password"});
            table12.AddRow(new string[] {
                        "Confirm Password",
                        "Password"});
            table12.AddRow(new string[] {
                        "Skill Set",
                        "C#, Scrum, Java, Html"});
            table12.AddRow(new string[] {
                        "Product Owner",
                        "Yes"});
#line 157
 testRunner.When("I enter the following information:", ((string)(null)), table12, "When ");
#line 165
 testRunner.And("I click \'Register\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 167
 testRunner.Then("a \'Registration - Success\' screen is displayed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 168
 testRunner.And("an account associated with the email address \'po@test.com\' exists", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table13 = new TechTalk.SpecFlow.Table(new string[] {
                        "Roles"});
            table13.AddRow(new string[] {
                        "Product Owner"});
#line 169
 testRunner.And("the \'po@test.com\' account has the following preferred roles:", ((string)(null)), table13, "And ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion