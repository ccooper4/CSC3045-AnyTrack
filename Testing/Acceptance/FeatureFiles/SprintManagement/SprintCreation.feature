Feature: Creating a sprint as a Scrum Master
	As a Scrum Master
	I can create a new sprint
	So that development can begin

	Scenario:
	Given I am logged in as 'user@test.com' with the password 'Password'
	And 'user@test.com' is a 'Scrum Master' of the 'Scrum Management System' Project
	When I click 'Projects'
	Then I am presented with a tile for 'Scrum Management System'
	When I click 'Manage Sprints' from the pop­up menu of 'Scrum Manangement System' project
	Then I click 'Add Sprint'
	Then the 'Sprint Wizard' screen is displayed
	And the following fields are displayed:
         | Field       |
         | Name        |
         | Start Date  |
         | End Date    |
         | Description |
	When I enter the following information:
	     | Field       | Value                |
	     | Name        | Sprint 1             |
	     | Start Date  | <today>              |
	     | End Date    | <2 weeks from today> |
	     | Description | First Sprint         |
	And I click 'Save'
	Then the sprint 'Sprint1' has been created in the 'Scrum Management System' project

	Scenario: Creating a sprint when not a Scrum Master
		Given I am logged in as 'user@test.com' with the password 'Password'
		And 'user@test.com' is a 'Developer' of the 'Scrum Management System' Project
		When I click 'Projects'
		Then I am presented with a tile for 'Scrum Management System'
		And I cannot click 'Manage Sprints'


