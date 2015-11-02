Feature: Allocating user stories from the product backlog
	As a scrum master
	I can allocate user stories from the product backlog to the sprint backlog 
	So that they can be implemented by the team and they are shown as assigned 
	And uneditable on the product backlog until the sprint is complete

Scenario: Moving user story from product backlog to sprint backlog
	Given I am logged in as 'user@test.com' with the password 'Password'
	And 'user@test.com' is a 'Scrum Master' of the 'Scrum Management System' Project
	When I click 'Projects'
	Then the 'My Projects' screen is displayed
	And I am presented with a tile for 'Scrum Management System'
	When I click on the project with name 'Scrum Management System'
	And I click 'Manage Product Backlog' from the pop­up menu of 'Scrum Manangement System' project
	Then I move 'Story1' from 'Product Backlog' and move it to 'Sprint Backlog'
	When I click 'Save'
	Then the 'Story1' is in the 'Sprint Backlog'

Scenario: Moving user story back to product backlog from sprint backlog
	Given I am logged in as 'user@test.com' with the password 'Password'
	And 'user@test.com' is a 'Scrum Master' of the 'Scrum Management System' Project
	And the 'Scrum Management System' project has a sprint 'Sprint1' which contains a story called 'Story1'
	When I click 'Projects'
	Then the 'My Projects' screen is displayed
	And I am presented with a tile for 'Scrum Management System'
	When I click on the project with name 'Scrum Management System'
	And I click 'Manage Product Backlog' from the pop­up menu of 'Scrum Manangement System' project
	Then I move 'Story1' from 'Sprint Backlog' and move it back to 'Product Backlog'
	When I click 'Save'
	Then the 'Story1' is now in the 'Product Backlog'

Scenario: Assigning user to story
	Given I am logged in as 'user@test.com' with the password 'Password'
	And 'user@test.com' is a 'Developer' of the 'Scrum Management System' Project
	And the 'Scrum Management System' project has a sprint 'Sprint1' which contains a story called 'Story1'
	When I click 'Projects'
	Then the 'My Projects' screen is displayed
	And I am presented with a tile for 'Scrum Management System'
	When I click on the project with name 'Scrum Management System'
	And I click on 'Story1' story
	Then I assign 'user@test.com' to 'Story1' with the role 'Developer'
	When I click 'Save'
	Then 'user@test.com' is now assigned to 'Story1' with the role 'Developer'

Scenario: Unassigning user from story
	Given I am logged in as 'user@test.com' with the password 'Password'
	And 'user@test.com' is a 'Developer' of the 'Scrum Management System' Project
	And the 'Scrum Management System' project has a sprint 'Sprint1' which contains a story called 'Story1'
	When I click 'Projects'
	Then the 'My Projects' screen is displayed
	And I am presented with a tile for 'Scrum Management System'
	When I click on the project with name 'Scrum Management System'
	And I click on 'Story1' story
	Then I unassign 'user@test.com' from 'Story1' with the role 'Developer'
	When I click 'Save'
	Then 'user@test.com' is now unassigned from 'Story1' with the role 'Developer'