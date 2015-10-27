Feature: PlanningPokerChat
	As a scrum master 
	I can chat to users in the session
	So that the multiple users can discuss backlog tasks 

Scenario: Send a message
	Given there is no account associated with the email address 'user@test.com'
	And the 'user@test.com' account has the following preferred roles:
         | Roles        |
         | Scrum Master |