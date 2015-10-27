Feature: Planning Poker task selection
	As a scrum master 
	I can select a task
	So that the the task can be estimated through planning poker

Scenario: Select a task
	Given I am logged in as 'user@test.com' with the password 'Password'
	And 'user@test.com' is a 'Scrum Master' of the 'Scrum Management System' Project
	And the 'Scrum Management System' project has a sprint 'Sprint1' which contains a story called 'Story1'
	When I click 'Projects'
	Then the 'My Projects' screen is displayed
	When I click on the project with name 'Scrum Management System'
	Then the 'Scrum Management System - Product Backlog' is displayed
	When I click 'Start Planning Poker'
	Then the 'Planning Poker - Lobby' screen is displayed
	When I click 'Select Stories'
	Then the 'Select Stories' screen is displayed

	#As a Developer
	#I can Indicate my estimate on the currently selected story
	#So that an agreed estimate can be achieved

Scenario: Give an estimate
	Given I am logged in as 'developer@test.com' with the password 'Password'
	And the 'developer@test.com' account has the following preferred roles:
         | Roles     |
         | Developer |
	And 'developer@test.com' is a 'Developer' of the 'Scrum Management System' Project
	And the 'Scrum Manangement System' project has a sprint 'Sprint1' which contains a story called 'Story1'
	When I click 'Projects'
	Then the 'My Projects' screen is displayed
	When I click on the project with name 'Scrum Management System'
	Then the 'Scrum Management System - Product Backlog' is displayed
	When I click 'Start Planning Poker'
	Then the 'Scrum Management System' planning poker screen is displayed
	When the 'Story1' story tab is selected
	Then the 'Story1 - Account Registration' is displayed
	And the following fields are displayed:
		| Field    | Options |
		| Estimate | 0       |
		| Estimate | 0.5     |
		| Estimate | 1       |
		| Estimate | 2       |
		| Estimate | 3       |
		| Estimate | 5       |
		| Estimate | 8       |
		| Estimate | 13      |
		| Estimate | 20      |
		| Estimate | 40      |
		| Estimate | 100     |
		| Estimate | ?       |
	When I click '5'
	Then the following message is displayed 'TestUser' : 'Estimate'

	#As a Scrum Master
	#I can provide a final estimate
	#So that point value can be associated with a story
Scenario: Give a final estimate
	Given I am logged in as 'user@test.com' with the password 'Password'
	And 'user@test.com' is a 'Scrum Master' of the 'Scrum Management System' Project
	And the 'Scrum Management System' project has a sprint 'Sprint1' which contains a story called 'Story1'
	When I click 'Projects'
	Then the 'My Projects' screen is displayed
	When I click 'Scrum Management System'
	And I click 'Start Planning Poker'
	Then the 'Scrum Management System' planning poker screen is displayed
	When the 'Story1' story tab is selected
	Then the 'Story1 - Account Registration' is displayed
	And the following fields are displayed:
        | Field       | Options |
		| Final Value | 0       |
		| Final Value | 0.5     |
		| Final Value | 1       |
		| Final Value | 2       |
		| Final Value | 3       |
		| Final Value | 5       |
		| Final Value | 8       |
		| Final Value | 13      |
		| Final Value | 20      |
		| Final Value | 40      |
		| Final Value | 100     |
		| Final Value | ?       |
	When I enter the following information:
         | Field       | Value |
         | Final Value | 5     |
	And I click 'Confirm and Proceed'
	Then the estimate value of 'Story1' is '5'


