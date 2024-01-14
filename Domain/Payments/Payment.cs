using Domain.Attributes;

namespace Domain.Payments
{
    [AudiTable]
    public class Payment
    {
        public Guid Id { get; set; }
        public int Amount { get;private set; }
        public bool IsPay { get;private set; } = false;
        public DateTime? DatePay { get; private set; } = null;
        public string? Authority { get;private set; }
        public int RefId { get; private set; } = 0;
        public Order.Order Order { get;private set; }
        public int OrderId { get; set; }
        public Payment(int amount, int orderId)
        {
            Amount = amount;
            OrderId = orderId;
        }
        public void PaymentIsDone(string authority, int refId)
        {
            IsPay = true;
            DatePay = DateTime.Now;
            Authority = authority;
            RefId = refId;
        }
    }
}
