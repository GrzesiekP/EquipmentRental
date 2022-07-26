namespace Orders.Aggregate.ValueObjects
{
    public enum OrderStatus
    {
        Submitted,
        WaitingForApproval,
        Approved,
        PartiallyPaid,
        Paid,
        Reserved,
        InRealisation,
        Completed
    }
}