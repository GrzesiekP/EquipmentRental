namespace Orders.Aggregate.ValueObjects
{
    public enum OrderStatus
    {
        Submitted = 1,
        WaitingForApproval = 2,
        Approved = 4,
        Paid = 8,
        InRealisation = 16,
        Completed = 32
    }
}