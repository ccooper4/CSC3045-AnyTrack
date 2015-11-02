Feature: PlanningPokerSessionManagement
	As a scrum master 
	I can create a new, joinable planning poker session in a project
	So that the multiple users may carry out a planning poker session 

Scenario: Start a Planning Poker session
	Given an account associated with the email address 'user@test.com' exists
	And I am logged in as 'user@test.com' with the password 'Password'
	And 'user@test.com' is a 'Scrum Master' of the 'Scrum Management System' Project
	And the account 'user@test.com' has been assigned to 'Scrum Management System' project
	And the 'Scrum Management System' project has a sprint 'Sprint1' which contains a story called 'Story1'
	When I click 'Projects'
	Then the 'My Projects' screen is displayed
	When I click on the project with name 'Scrum Management System'
	Then the 'Scrum Management System - Product Backlog' screen is displayed
	When I click 'Start Planning Poker'
	Then the 'Scrum Management System' planning poker screen is displayed

Scenario: Join a Planning Poker session
	Given an account associated with the email address 'developer@test.com' exists
	And I am logged in as 'developer@test.com' with the password 'Password'
	And the 'developer@test.com' account has the following preferred roles:
         | Roles     |
         | Developer |
	And 'developer@test.com' is a 'Developer' of the 'Scrum Management System' Project
	And I have created a project called 'Scrum Management System'
	And the 'Scrum Management System' project has a sprint 'Sprint1' which contains a story called 'Story1'
	When I click 'Projects'
	Then the 'My Projects' screen is displayed
	When I click on the project with name 'Scrum Management System'
	Then the 'Scrum Management System - Product Backlog' screen is displayed
	When I click 'Join Planning Poker'
	Then the 'Scrum Management System' planning poker screen is displayed


	