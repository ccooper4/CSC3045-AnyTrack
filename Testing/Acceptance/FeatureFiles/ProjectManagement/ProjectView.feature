Feature: My projects view
	As a logged in user
	I can see a list of the projects to which I have been assigned and open one
	So that I can work on the project according to my role(s) on the project

Scenario: Navigate to my projects
	Given an account associated with the email address 'developer@test.com' exists
	And I am logged in as 'developer@test.com' with the password 'Password'
	And the account 'developer@test.com' has been assigned to 'Scrum Management System' project
	When I click on the projects navigation button
	Then I am presented with a tile for 'Scrum Management System'

Scenario: View Project Manager screen via Project Option
	Given I am logged in as 'developer@test.com' with the password 'Password'
	And the account 'developer@test.com' has been assigned to 'Scrum Management System' project
	When I click on the projects navigation button
	Then I am presented with a tile for 'Scrum Management System'
	When I click on the pop-up menu on the tile for 'Scrum Management System'
	Then I am able to view the Project Management Screen via the 'Manage Project' option




