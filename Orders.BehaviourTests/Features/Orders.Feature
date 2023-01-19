Feature: Orders
	Rental orders feature

@Submitting
Scenario: Submit a valid order
	Given the user email is user@example.com
	And there is 2 equipments of type CT
	And the rental period is 3 days from today
	When client submits an order
	Then there is no exception
	Then order status is submitted
	And order data should be the same as in input
	
@Approval
Scenario: Approving order
	Given the approval is requested for order
	When admin approves the order
	Then there is no exception