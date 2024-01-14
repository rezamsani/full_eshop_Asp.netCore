using Application.Interfaces.Contexts;
using Domain.Order;
using Domain.Payments;
using Microsoft.EntityFrameworkCore;

namespace Application.PaymentService
{
    public interface IPaymentServices
    {
        PaymentOfOrderDto PayForOrder(int OrderId);
        PaymentDto GetPayment(Guid Id);
        bool VerifyPayment(Guid Id, string Authority, int RefId);
    }
    public class PaymentServices : IPaymentServices
    {
        private readonly IDataBaseContext context;
        private readonly IIdentityDataBaseContext userContext;

        public PaymentServices(IDataBaseContext context, IIdentityDataBaseContext userContext)
        {
            this.context = context;
            this.userContext = userContext;
        }

        public PaymentDto GetPayment(Guid Id)
        {
            var payment = context.payments.Include(p => p.Order)
                .ThenInclude(p => p.orderItems)
                .SingleOrDefault(p => p.Id == Id);
            var user = userContext.Users.SingleOrDefault(p => p.Id == payment.Order.ByedrId);
            string description = $"شماره سفارش پرداخت {payment.OrderId} " + Environment.NewLine;
            description += "محصولات" + Environment.NewLine;
            foreach (var item in payment.Order.orderItems.Select(p=> p.ProductName))
            {
                description += $" -{item}";
            }
            return new PaymentDto
            {
                Amount = payment.Order.TotalPrice(),
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Userid = user.Id,
                Id = payment.Id,
                Description = description
            };
        }

        public PaymentOfOrderDto PayForOrder(int OrderId)
        {
            var order = context.Orders.Include(p => p.orderItems)
                .SingleOrDefault(p => p.Id == OrderId);
            if (order == null)
            {
                throw new Exception("");
            }
            var payment = context.payments.SingleOrDefault(p => p.OrderId == order.Id);
            if (payment == null)
            {
                payment = new Payment(order.TotalPrice(), order.Id);
                context.payments.Add(payment);
                context.SaveChanges();
            }
            return new PaymentOfOrderDto
            {
                Amount = payment.Amount,
                PaymentId = payment.Id,
                paymentMethod = order.PaymentMethod
            };
        }

        public bool VerifyPayment(Guid Id, string Authority, int RefId)
        {
            var payment = context.payments.Include(p => p.Order)
                .SingleOrDefault(p => p.Id == Id);
            if (payment == null)
                throw new Exception("Payment Not Found");
            payment.Order.PaymentDone();
            payment.PaymentIsDone(Authority, RefId);
            context.SaveChanges();
            return true;
        }
    }
    public class PaymentOfOrderDto
    {
        public Guid PaymentId { get; set; }
        public int Amount { get; set; }
        public PaymentMethod paymentMethod { get; set; }
    }
    public class PaymentDto
    {
        public Guid Id { get; set; }
        public string PhoneNumber { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public int Amount { get; set; }
        public string Userid { get; set; }
    }
}
