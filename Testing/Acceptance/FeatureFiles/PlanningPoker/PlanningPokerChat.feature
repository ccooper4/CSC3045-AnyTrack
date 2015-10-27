Feature: PlanningPokerChat
	As a scrum master 
	I can chat to users in the session
	So that the multiple users can discuss backlog tasks 

Scenario: Send a message
	Given I am logged in as 'user@test.com' with the password 'Password'
	And the 'user@test.com' account has the following preferred roles:
         | Roles        |
         | Scrum Master |
	And 'user@test.com' is a 'Scrum Master' of the 'Scrum Management System' Project
	And I have created a project called 'Scrum Management System'
	When I click 'Projects'
	Then the 'My Projects' screen is displayed
	When I click on the project with name 'Scrum Management System'
	Then the 'Scrum Management System - Product Backlog' screen is displayed
	When I click 'Start Planning Poker'
	Then the 'Scrum Management System' planning poker screen is displayed
	When I enter the following text to the send message field: 'Hello'
	And I click 'Send'
	Then the message 'Hello' is displayed in 'Scrum Management System' planning poker active session
	And the following story information is displayed:
		| StoryNumber | StoryName | Included |
		| 1           | Story1    | No       |
	When I check the checkbox for 'Story1' to include it in the planning poker
	Then the 'Planning Poker - Lobby' screen is displayed