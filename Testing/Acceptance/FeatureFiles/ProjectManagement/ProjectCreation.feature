Feature: Project Creation
	As a logged in user
	I can create a new project
	So that I can be the Project Manager

Scenario: Happy path project creation

	Given an account associated with the email address 'user@test.com' exists
	And I am logged in as 'user@test.com' with the password 'Password'

	When I click 'Projects'
	Then a 'Projects' screen is displayed
    
	When I click 'Create a project'
	Then a 'New Project' screen is displayed
	And the following fields are displayed:
		| Field           |
		| Project Name    |
		| Description     |
		| Version Control |
		| Started On      |

	When I enter the following information:
		| Field           | Value                               |
		| Project Name    | SCRUM Management                    |
		| Description     | Software to manage agile mythology  |
		| Version Control | http://www.eeecs.qub.ac.uk/svn/cs2/ |
		| Started On      | <today>                             |
	And I click 'Save'
	Then a 'Projects' screen is displayed

Scenario: Project created that already exists

	Given an account associated with the email address 'user@test.com' exists
    And I am logged in as 'user@test.com' with the password 'Password'
	And I have created a project called 'SCRUM Management'          
	
	When I click 'Projects'
	Then a 'Projects' screen is displayed
	
	 When I click 'Create a project'
	Then a 'New Project' screen is displayed

	And the following fields are displayed:
		| Field           |
		| Project Name    |
		| Description     |
		| Version Control |
		| Started On      |

	When I enter the following information:
		| Field           | Value                               |
		| Project Name    | SCRUM Management                    |
		| Description     | Software to manage agile mythology  |
		| Version Control | http://www.eeecs.qub.ac.uk/svn/cs2/ |
		| Started On      | <today>                             |
	And I click 'Save'
	Then an error message 'A project with the name SCRUM Management already exists' is displayed

Scenario: Project created without a name present

	Given an account associated with the email address 'user@test.com' exists
	And I am logged in as 'user@test.com' with the password 'Password'

	When I click 'Projects'
	Then a 'Projects' screen is displayed
	
	 When I click 'Create a project'
	Then a 'New Project' screen is displayed
	And the following fields are displayed:
		| Field           |
		| Project Name    |
		| Description     |
		| Version Control |
		| Started On      |

	When I enter the following information:
		| Field           | Value                               |
		| Description     | Software to manage agile mythology  |
		| Version Control | http://www.eeecs.qub.ac.uk/svn/cs2/ |
		| Started On      | <today>                             |
	And I click 'Save'
	Then an error message 'Project name must be specified' is displayed

Scenario: Project created without a date present

	Given an account associated with the email address 'user@test.com' exists
    And I am logged in as 'user@test.com' with the password 'Password'

	When I click 'Projects'
	Then a 'Projects' screen is displayed

    When I click 'Create a project'
	Then a 'New Project' screen is displayed
	And the following fields are displayed:
		| Field           |
		| Project Name    |
		| Description     |
		| Version Control |
		| Started On      |

	When I enter the following information:
		| Field           | Value                               |
		| Name            | SCRUM Management                    |
		| Description     | Software to manage agile mythology  |
		| Version Control | http://www.eeecs.qub.ac.uk/svn/cs2/ |
	And I click 'Save'
	Then an error message 'Start date for project must be specified' is displayed
