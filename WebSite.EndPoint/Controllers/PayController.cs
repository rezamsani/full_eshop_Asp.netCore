using Application.PaymentService;
using Domain.Payments;
using Dto.Payment;
using Microsoft.AspNetCore.Mvc;
using WebSite.EndPoint.Utilities;
using ZarinPal.Class;

namespace WebSite.EndPoint.Controllers
{
    public class PayController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly IPaymentServices paymentServices;
        private readonly string merchentId;
        private readonly ZarinPal.Class.Payment _payment;
        private readonly Authority _authority;
        private readonly Transactions _transactions;
        public PayController(IConfiguration configuration ,IPaymentServices paymentServices)
        {
            this.configuration = configuration;
            this.paymentServices = paymentServices;
            merchentId = configuration["ZarinPallMerchenId"];
            var expose = new Expose();
            _payment = expose.CreatePayment();
            _authority = expose.CreateAuthority();
            _transactions = expose.CreateTransactions();
        }
        public async Task<IActionResult> Index(Guid PaymentId)
        {
            var payment = paymentServices.GetPayment(PaymentId);
            if(payment == null)
                return NotFound();
            string userId = ClaimUtility.GetUserId(User);
            if (userId != payment.Userid)
                return BadRequest();
            string callBackUrl = Url.Action(nameof(Verify), "Pay", new { payment.Id }, protocol: Request.Scheme);
            var resultRequsetZarinPall = await _payment.Request(new DtoRequest()
            {
                Mobile = payment.PhoneNumber,
                CallbackUrl =  callBackUrl,
                Description = payment.Description,
                Email = payment.Email,
                Amount = payment.Amount,
                MerchantId = configuration["ZarinPallMerchenId"]
            }, ZarinPal.Class.Payment.Mode.sandbox);
            return Redirect($"https://sandbox.zarinpal.com/pg/StartPay/{resultRequsetZarinPall.Authority}");
        }
        public async Task<IActionResult> Verify(Guid Id, string Authority)
        {
            string Status = HttpContext.Request.Query["Status"];
            if(Status != "" && Status.ToString().ToLower() == "ok"
               && Authority != ""                 
               )
            {
                var payment = paymentServices.GetPayment(Id);
                if (payment == null)
                    return NotFound();
                var verification =  _payment.Verification(new DtoVerification
                {
                    Amount = payment.Amount,
                    Authority = Authority,
                    MerchantId = configuration["ZarinPallMerchenId"]
                }, ZarinPal.Class.Payment.Mode.sandbox).Result;
                if (verification.Status == 100)
                {
                    bool verifyResult = paymentServices.VerifyPayment(Id, Authority, verification.RefId);
                    if (verifyResult)
                    {
                        return Redirect("Customer/orders");
                    }
                    else
                    {
                        TempData["message"] = "پرداخت انجام شد اما ثبت نشد";
                        return RedirectToAction("checkout", "basket");
                    }    
                }
                else
                {
                    TempData["message"] = "مجدداً تلاش نمایید در صورت بروز مشکل با مدیر سامانه تماس بگیرید.";
                    return RedirectToAction("checkout", "basket");
                }

            }
            TempData["message"] = "پرداخت شما ناموفق بوده است";
            return RedirectToAction("checkout", "basket");
        }
        
    }
}
