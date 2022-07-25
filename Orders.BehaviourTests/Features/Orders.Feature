Feature: Orders
	Rental orders feature

@Submitting
Scenario: Submit a valid order
	Given the user email is user@example.com
	And there is 2 equipments of type CT which rental price is 20
	And the rental period is 3 days from today
	When order is submitted
	Then the order should be in status Submitted
	And order data should be the same as in input