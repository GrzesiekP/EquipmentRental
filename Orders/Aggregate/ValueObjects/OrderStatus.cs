﻿namespace Orders.Aggregate.ValueObjects
{
    public enum OrderStatus
    {
        Submitted = 1,
        WaitingForApproval = 2,
        Approved = 3
    }
}