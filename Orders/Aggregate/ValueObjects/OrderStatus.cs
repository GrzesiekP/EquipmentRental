namespace Orders.Aggregate.ValueObjects
{
    public enum OrderStatus
    {
        Submitted = 1,
        WaitingForApproval = 2,
        Approved = 4,
        Paid = 8,
        PartiallyPaid = 16,
        InRealisation = 32,
        Completed = 64
    }
}