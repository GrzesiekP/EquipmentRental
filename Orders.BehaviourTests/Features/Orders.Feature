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
	
@Approving
Scenario: Approving order
	Given order is awaiting approval
	When order is approved
	Then equipment is reserved for 24 hours
	
@Reservation
Scenario: Reserving equipment
	Given the order is fully paid
	When reserving ordered equipment
	Then order status is reserved
	And order equipment is reserved for order reservation period
	
@Reservation
Scenario: Reserving not available equipment
	Given the order is fully paid
	And equipment is no longer available
	When reserving ordered equipment
	Then exception is thrown
	And order equipment is not reserved
	
@Reservation
Scenario: Expiring reservation
	Given the order is reserved
	When Reservation time expired
	Then order is cancelled
	And order equipment available
	
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
	
@Returning
Scenario: Returning already returned equipment
	Given the order is completed
	When returning equipment
	Then order status is completed
	And equipment is available