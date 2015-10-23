Feature: Account Login
	As a registered user
	I can log in with my username and password 
	So that I can begin using the system

Scenario: Successful Login

	Given an account associated with the email address 'user@test.com' exists
	When I open the system 
	Then a 'Login' screen is displayed 
	And the following fields are displayed:
		| Field         |
		| Email Address |
		| Password      |

	When I enter the following information:
		| Field         | Value         |
		| Email Address | user@test.com |
		| Password      | Password      |
	And I click 'Login' 
	Then a 'Home' screen is displayed

Scenario: Unsuccessful Login (Incorrect Password)

	Given an account associated with the email address 'user@test.com' exists
	When I open the system 
	And I click 'Login'
	Then a 'Login' screen is displayed 
	And the following fields are displayed:
		| Field         |
		| Email Address |
		| Password      |

	When I enter the following information:
		| Field         | Value             |
		| Email Address | user@test.com     |
		| Password      | IncorrectPassword |
	And I click 'Login' 
	Then an error message 'Incorrect email address or password' is displayed

Scenario: Unsuccessful Login (No Account)

	Given there is no account associated with the email address 'newuser@test.com'
	When I open the system 
	And I click 'Login'
	Then a 'Login' screen is displayed 
	And the following fields are displayed:
		| Field         |
		| Email Address |
		| Password      |

	When I enter the following information:
		| Field         | Value            |
		| Email Address | newuser@test.com |
		| Password      | Password         |
	And I click 'Login' 
	Then an error message 'An account does not exist for this email address' is displayed
