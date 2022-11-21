namespace Orders.Aggregate.ValueObjects
{
    public enum OrderStatus
    {
        Submitted = 1,
        WaitingForApproval = 2,
        Approved = 3,
        PartiallyPaid = 4,
        Paid = 5,
        InRealisation = 6,
        Completed = 7
    }
}