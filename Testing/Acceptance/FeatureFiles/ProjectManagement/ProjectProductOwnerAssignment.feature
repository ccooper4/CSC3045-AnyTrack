Feature: ProjectPoductOwnerAssignment
	As a Project Manager
	I can search for a specific registered Product Owner and assign them as a 
	Product Owner for my project
	So that they can begin to add stories to the Product Backlog

Scenario: Happy Path Product Owner Assignment

	Given an account associated with the email address 'user@test.com' exists
	And I am logged in as 'user@test.com' with the password 'Password'
	And I have created a project called 'Scrum Management System'
	And a 'Product Owner' has not been assigned to 'Scrum Management System'
	And an account associated with the email address 'po@test.com' exists
	And the 'po@test.com' account has the following preferred roles: 
			| Roles         |
			| Product Owner |
	
	When I click 'Projects'
	Then a 'Projects' screen is displayed
	
	#step may need to be changed to When I click the 'Scrum Management system' tile
	When I click 'Scrum Management System'
	And I click 'Manage Project' from the pop­up menu

	Then a 'Project Details' screen is displayed
	And  the following fields are displayed:
		| Field           |
		| Description     |
		| Version Control |
		| Project Manager |
		| Project Owner   |
		| Scrum Masters   |

	When I enter 'po@test.com' into the 'Product Owner' search box
	And I click 'Search'
	Then the following result is displayed:
		| Name     |
		| Joe Test |

	When I add 'Joe Test' from the results
	Then a 'Assignment Successful' message is displayed

	When I click 'Save'
	Then 'Joe Test' is a 'Product Owner' of the 'Scrum Management System' Project

Scenario: Assigning Registered User without Product Owner Role as Product Owner

	Given an account associated with the email address 'user@test.com' exists
	And  I have created a project called 'Scrum Management System'
	And a 'Product Owner' has not been assigned to 'Scrum Management System'
	Given an account associated with the email address 'developer@test.com' exists
	And the 'po@test.com' account has the following preferred roles: 
			| Roles     |
			| Developer |
	And I am logged in as 'user@test.com' with the password 'Password'

	When I click 'Projects'
	Then a 'Projects' screen is displayed

	When I click 'Scrum Management System'
	And I click 'Manage Project' from the pop­up menu

	Then a 'Project Details' screen is displayed
	And  the following fields are displayed:
			| Field           |
			| Description     |
			| Version Control |
			| Project Manager |
			| Project Owner   |
			| Scrum Masters   |

	When I enter 'developer@test.com' into the 'Product Owner' search box
	And I click 'Search'
	Then no results are found

Scenario: Assigning an unregistered user to be Product Owner

	Given an account associated with the email address 'user@test.com' exists
	And  I have created a project called 'Scrum Management System'
	And a 'Product Owner' has not been assigned to 'Scrum Management System'
	And there is no account associated with the email address 'po@test.com' 
	And I am logged in as 'user@test.com' with the password 'Password'

	When I click 'Projects'
	Then a 'Projects' screen is displayed

	When I click 'Scrum Management System'
	And I click 'Manage Project' from the pop­up menu
	Then a 'Project Details' screen is displayed
	And  the following fields are displayed:
			| Field           |
			| Description     |
			| Version Control |
			| Project Manager |
			| Project Owner   |
			| Scrum Masters   |

	When I enter 'po@test.com' into the 'Product Owner' search box
	And I click 'Search'
	Then no results are found

	Scenario: Assigning Product Owner to Project with Product Owner

	Given an account associated with the email address 'user@test.com' exists
	And  I have created a project called 'Scrum Management System'
	And a 'Product Owner' has been assigned to 'Scrum Management System'
	And an account associated with the email address 'po@test.com' exists
	And the 'po@test.com' account has the following preferred roles: 
			| Roles         |
			| Product Owner |
	And I am logged in as 'user@test.com' with the password 'Password'
	When I click 'Projects'

	Then a 'Projects' screen is displayed
	When I click 'Scrum Management System'
	And I click 'Manage Project' from the pop­up menu

	Then a 'Project Details' screen is displayed

	And  the following fields are displayed:
			| Field           |
			| Description     |
			| Version Control |
			| Project Manager |
			| Project Owner   |
			| Scrum Masters   |

	When I enter 'developer@test.com' into the 'Product Owner' search box
	And I click 'Search'

	Then the following result is displayed:
		| Name     |
		| Joe Test |

	When I add 'Joe Test' from the results
	Then an error message 'Product Owner already assigned' is displayed

