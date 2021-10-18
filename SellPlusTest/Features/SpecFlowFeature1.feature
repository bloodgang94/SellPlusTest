Feature: Checking authorization in your personal account
	Basic test of authorization check under the client

@Chrome
@Opera
@Firefox
Scenario: I Auhorization
	Given I Navigate to the Login page
	When  I Log in under the Role:'Manager'
	Then I should see title 'SellPlus' string