Feature: Orders
	Rental orders feature

@Submitting
Scenario: Submitting a valid order
	Given the user email is user@example.com
	And there is 2 equipments of type CT which rental price is 20
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

@OrderPayment
Scenario: Paying full order price
	Given the total equipment price is 40 per day
	And the rental period is 1 days from today
	And the order is approved
	When admin confirms order payment for 40
	Then order status is paid

Scenario: Pay order price partially
	Given the total equipment price is 40 per day
	And the rental period is 1 days from today
	And the order is approved
	When admin confirms order payment for 20
	Then order status is partially paid

@Reservation
Scenario: Reserving equipment
	Given the order is fully paid
	When reserving ordered equipment
	Then order status is reserved
	And order equipment is reserved for order reservation period

@Renting
Scenario: Renting equipment
	Given the order is reserved
	When admin confirms equipment is rent
	Then order status is In Realisation
	And equipment is rent

@Returning
Scenario: Returning equipment
	Given the order is in realisation
	When admin confirms equipment is returned
	Then order status is completed
	And equipment is available