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
namespace Acceptance.FeatureFiles.SprintManagement
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.9.0.77")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("Creating a sprint as a Scrum Master")]
    public partial class CreatingASprintAsAScrumMasterFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "SprintCreation.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Creating a sprint as a Scrum Master", "As a Scrum Master\r\nI can create a new sprint\r\nSo that development can begin", ProgrammingLanguage.CSharp, ((string[])(null)));
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
        [NUnit.Framework.DescriptionAttribute("Creating a sprint")]
        public virtual void CreatingASprint()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Creating a sprint", ((string[])(null)));
#line 6
 this.ScenarioSetup(scenarioInfo);
#line 7
 testRunner.Given("I am logged in as \'user@test.com\' with the password \'Password\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 8
 testRunner.And("\'user@test.com\' is a \'Scrum Master\' of the \'Scrum Management System\' Project", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 9
 testRunner.When("I click \'Projects\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 10
 testRunner.Then("I am presented with a tile for \'Scrum Management System\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 11
 testRunner.When("I click \'Manage Sprints\' from the pop­up menu of \'Scrum Manangement System\' proje" +
                    "ct", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 12
 testRunner.Then("I click \'Add Sprint\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 13
 testRunner.Then("the \'Sprint Wizard\' screen is displayed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field"});
            table1.AddRow(new string[] {
                        "Name"});
            table1.AddRow(new string[] {
                        "Start Date"});
            table1.AddRow(new string[] {
                        "End Date"});
            table1.AddRow(new string[] {
                        "Description"});
#line 14
 testRunner.And("the following fields are displayed:", ((string)(null)), table1, "And ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table2.AddRow(new string[] {
                        "Name",
                        "Sprint 1"});
            table2.AddRow(new string[] {
                        "Start Date",
                        "<today>"});
            table2.AddRow(new string[] {
                        "End Date",
                        "<2 weeks from today>"});
            table2.AddRow(new string[] {
                        "Description",
                        "First Sprint"});
#line 20
 testRunner.When("I enter the following information:", ((string)(null)), table2, "When ");
#line 26
 testRunner.And("I click \'Save\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 27
 testRunner.Then("the sprint \'Sprint1\' has been created in the \'Scrum Management System\' project", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Creating a sprint when not a Scrum Master")]
        public virtual void CreatingASprintWhenNotAScrumMaster()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Creating a sprint when not a Scrum Master", ((string[])(null)));
#line 29
 this.ScenarioSetup(scenarioInfo);
#line 30
  testRunner.Given("I am logged in as \'user@test.com\' with the password \'Password\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 31
  testRunner.And("\'user@test.com\' is a \'Developer\' of the \'Scrum Management System\' Project", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 32
  testRunner.When("I click \'Projects\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 33
  testRunner.Then("I am presented with a tile for \'Scrum Management System\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 34
  testRunner.And("I cannot click \'Manage Sprints\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
