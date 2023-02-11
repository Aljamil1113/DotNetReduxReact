namespace RestoreAPI.Entities.OrderAggregate
{
    public enum OrderStatus
    {
        Pending,
        PaymentReceiver,
        PaymentFailed,
        PaymentReceived
    }
}
