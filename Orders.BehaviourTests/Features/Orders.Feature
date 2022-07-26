Feature: Orders
	Rental orders feature

@Submitting
Scenario: Submit a valid order
	Given the user email is user@example.com
	And there is 2 equipments of type CT which rental price is 20
	And the rental period is 3 days from today
	When order is submitted
	Then order status is submitted
	And order data should be the same as in input
	
@Reservation
Scenario: Reserving equipment
	Given the order is fully paid
	When reserving ordered equipment
	Then order status is reserved
	And order equipment is reserved for order reservation period
	
@Renting
Scenario: Renting equipment
	Given the order is reserved
	When renting equipment
	Then order status is In Realisation
	And equipment is rent
	
@Returning
Scenario: Returning equipment
	Given the order is in realisation
	When returning equipment
	Then order status is completed
	And equipment is available