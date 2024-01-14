using Domain.Attributes;

namespace Domain.Order
{
    [AudiTable]
    public class Order
    {
        public int Id { get; set; }
        public string ByedrId { get;private set; }
        public DateTime OrderDate { get;private set; } = DateTime.Now;
        public Address Address { get;private set; }
        public PaymentMethod PaymentMethod { get;private set; }
        public PayMentStatus PayMentStatus { get;private set; }
        public OrderStatus OrderStatus { get;private set; }
        private readonly List<OrderItem> _orderItems = new List<OrderItem>();
        public IReadOnlyCollection<OrderItem> orderItems => _orderItems.AsReadOnly();
        public Order(string byedrId, Address address,List<OrderItem> orderItems, PaymentMethod paymentMethod)
        {
            ByedrId = byedrId;
            Address = address;
            _orderItems = orderItems;
            PaymentMethod = paymentMethod;
        }
        public Order()
        {
            
        }
        /// <summary>
        /// پرداخت انجام شد
        /// </summary>
        /// <returns></returns>
        public void PaymentDone()
        {
            PayMentStatus = PayMentStatus.Paid;
        }
        /// <summary>
        /// کالا تحویل داده شد
        /// </summary>
        public void OrderDeliverd()
        {
            OrderStatus = OrderStatus.Delivered;
        }
        /// <summary>
        /// ثبت مرجوعی کالا
        /// </summary>
        public void OrderReturned()
        {
            OrderStatus = OrderStatus.Returned;
        }
        /// <summary>
        /// لغو سفارش
        /// </summary>
        public void OrderCancelled()
        {
            OrderStatus = OrderStatus.Cancelled;
        }
        public int TotalPrice()
        {
            return _orderItems.Sum(p => p.UnitPrice * p.Units);
        }
    }
    [AudiTable]
    public class OrderItem
    {
        public int Id { get; set; }
        public int CatalogItemId { get;private set; }
        public string ProductName { get;private set; }
        public string PictureUri { get;private set; }
        public int UnitPrice { get;private set; }
        public int Units { get;private set; }
        public OrderItem(int catalogItemId, string productName, string pictureUri, int unitPrice, int units)
        {
            CatalogItemId = catalogItemId;
            ProductName = productName;
            PictureUri = pictureUri;
            UnitPrice = unitPrice;
            Units = units;
        }
        public OrderItem()
        {
            
        }
       
    }
    
    public class Address
    {
        public string State { get; private set; }
        public string PostaCode { get; }
        public string City { get; private set; }
        public string ZipCode { get; private set; }
        public string PostalAddress { get; private set; }
        public string ReciverName { get; private set; }
        public Address(string city, string state, string postaCode,
                       string reciverName
                      )
        {
            City = city;
            State = state;
            PostaCode = postaCode;
            ReciverName = reciverName;
        }
        public Address()
        {
            
        }
    }
    public enum PaymentMethod
    {
       /// <summary>
       /// پرداخت در آنلاین
       /// </summary>
        OnlinePayment = 0,
        /// <summary>
        /// پرداخت در محل
        /// </summary>
        PaymentOnTheSpot = 1,
    }
    public enum PayMentStatus
    {
        /// <summary>
        /// منتظر پرداخت
        /// </summary>
        WaitinForPayment=0,
        /// <summary>
        /// پرداخت انجام شد
        /// </summary>
        Paid=1,
    }
    public enum OrderStatus
    {
        /// <summary>
        /// درحال پردازش
        /// </summary>
        Processing=0,
        /// <summary>
        /// تحویل داده شد
        /// </summary>
        Delivered=1,
        /// <summary>
        /// مرجوعی
        /// </summary>
        Returned=2,
        /// <summary>
        /// لغو شده
        /// </summary>
        Cancelled=3
    }
}
