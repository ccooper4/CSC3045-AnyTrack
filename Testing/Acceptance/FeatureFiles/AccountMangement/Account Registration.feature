Feature: Account Registration

	As a user of the scrum client program 
	I can register 
	So that I can connect to the scrum management server

Scenario: Happy Path Registration

	Given there is no account associated with the email address 'user@test.com' 
	When I open the system 
	And I click 'Sign Up'
	Then a 'Registration' screen is displayed
	And the following fields are displayed: 
		| Field            | Required |
		| Email Address    | Yes      |
		| Full Name        | Yes      |
		| Password         | Yes      |
		| Confirm Password | Yes      |
		| Skill Set        | No       |


	#Password may change depending on password policy we implement
	When I enter the following information:
	| Field            | Value         |
	| Email Address    | user@test.com |
	| Full Name        | David Tester  |
	| Password         | Password      |
	| Confirm Password | Password      |
	| Skill Set        | Java, C#, WPF |
	And I click 'Register' 

	Then a 'Registration - Success' screen is displayed
	And an account associated with the email address 'user@test.com' exists

Scenario: User account has already been created for given email

	Given an account associated with the email address 'user@test.com' exists
	When I open the system
	And I click 'Sign Up'
	Then a 'Registration' screen is displayed
	And the following fields are displayed:
		| Field            | Required |
		| Email Address    | Yes      |
		| Full Name        | Yes      |
		| Password         | Yes      |
		| Confirm Password | Yes      |
		| Skill Set        | No       |

	When I enter the following information:
		| Field            | Value          |
		| Email Address    | user@test.com  |
		| Full Name        | David Tester   |
		| Password         | Password       |
		| Confirm Password | Password       |
		| Skill Set        | C#, C++, Linux |
	And I click 'Register' 

	Then an error message 'A user account with this email already exists' is displayed
	And '1' account is associated with the email address 'user@test.com'

Scenario: User tries to register an account with passwords that do not match

	Given there is no account associated with the email address 'user@test.com'
	When I open the system 
	And I click 'Sign Up'
	Then a 'Registration' screen is displayed 
	And the following fields are displayed:
		| Field            | Required |
		| Email Address    | Yes      |
		| Full Name        | Yes      |
		| Password         | Yes      |
		| Confirm Password | Yes      |
		| Skill Set        | No       |

	When I enter the following information:
		| Field            | Value                 |
		| Email Address    | user@test.com         |
		| Full Name        | David Tester          |
		| Password         | Password              |
		| Confirm Password | Password1             |
		| Skill Set        | C#, Scrum, Java, Html |
	And I click 'Register' 

	Then an error message 'Passwords do not match' is displayed
	And there is no account is associated with the email address 'user@test.com'

Scenario: User tries to register an invalid email address

	When I open the system 
	And I click 'Sign Up'
	Then a 'Registration' screen is displayed 
	And the following fields are displayed:
		| Field            | Required |
		| Email Address    | Yes      |
		| Full Name        | Yes      |
		| Password         | Yes      |
		| Confirm Password | Yes      |
		| Skill Set        | No       |

	When I enter the following information:
		| Field            | Value                 |
		| Email Address    | user.test.com         |
		| Full Name        | David Tester          |
		| Password         | Password              |
		| Confirm Password | Password              |
		| Skill Set        | C#, Scrum, Java, Html |
	And I click 'Register'
	Then an error message 'Email address is not valid' is displayed
	And there is no account is associated with the email address 'user.test.com'

Scenario: User doesn't specify skill set at registration

	Given there is no account associated with the email address 'user@test.com'
	When I open the system 
	And I click 'Sign Up'
	Then a 'Registration' screen is displayed 
	And the following fields are displayed:
		| Field            | Required |
		| Email Address    | Yes      |
		| Full Name        | Yes      |
		| Password         | Yes      |
		| Confirm Password | Yes      |
		| Skill Set        | No       |

	When I enter the following information:
		| Field            | Value         |
		| Email Address    | user@test.com |
		| Full Name        | David Tester  |
		| Password         | Password      |
		| Confirm Password | Password      |
	And I click 'Register' 

	Then a 'Registration - Success' screen is displayed
	And an account associated with the email address 'user@test.com' exists


	#As a registering user 
	#I can select the scrum role(s) that I will potentially take (Product Owner, Scrum Master, Developer) 
	#So that I will be able to carry out those roles in projects

	Scenario: Assigning Roles at Registration Happy Path
	Given there is no account associated with the email address 'po@test.com' 
	When I open the system 
	And I click 'Sign Up'
	Then a 'Registration' screen is displayed 
	And the following fields are displayed:
		| Field            | Required |
		| Email Address    | Yes      |
		| Full Name        | Yes      |
		| Password         | Yes      |
		| Confirm Password | Yes      |
		| Skill Set        | No       |
		| Product Owner    | No       |
		| Scrum Master     | No       |
		| Developer        | No       |

	When I enter the following information:
		| Field            | Value                 |
		| Email Address    | po@test.com           |
		| Full Name        | David Tester          |
		| Password         | Password              |
		| Confirm Password | Password              |
		| Skill Set        | C#, Scrum, Java, Html |
		| Product Owner    | Yes                   |
	And I click 'Register' 

	Then a 'Registration - Success' screen is displayed
	And an account associated with the email address 'po@test.com' exists 
	And the 'po@test.com' account has the following preferred roles: 
		| Roles         |
		| Product Owner |




	 