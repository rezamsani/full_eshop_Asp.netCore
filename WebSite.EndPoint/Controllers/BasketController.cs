using Application.BasketsService;
using Application.OrderServices;
using Application.PaymentService;
using Application.User;
using Domain.Entities;
using Domain.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebSite.EndPoint.Models.ViewModels.Basket;
using WebSite.EndPoint.Utilities;

namespace WebSite.EndPoint.Controllers
{
    [Authorize]
    public class BasketController : Controller
    {
        private readonly IBasketsService basketService;
        private readonly SignInManager<User> signInManager;
        private readonly IUserAddressServices userAddressServices;
        private readonly IOrderService orderService;
        private readonly IPaymentServices paymentServices;
        private string userId = null;
        public BasketController(IBasketsService basketService
            , SignInManager<User> signInManager,
            IUserAddressServices userAddressServices,
            IOrderService orderService,
            IPaymentServices paymentServices
            )
        {
            this.basketService = basketService;
            this.signInManager = signInManager;
            this.userAddressServices = userAddressServices;
            this.orderService = orderService;
            this.paymentServices = paymentServices;
        }
        [AllowAnonymous]
        public IActionResult Index()
        {
            var data = GetOrSetBasket();
            return View(data);
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Index(int CatalogitemId, int quantity = 1)
        {
            var basket = GetOrSetBasket();
            basketService.AddItemToBasket(basket.Id, CatalogitemId, quantity);
            return RedirectToAction(nameof(Index));
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult RemoveItemFromBasket(int ItemId)
        {
            basketService.RemoveItemFromBasket(ItemId);
            return RedirectToAction(nameof(Index));
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult setQuantity(int basketItemId, int quantity)
        {
            return Json(basketService.SetQuantities(basketItemId, quantity));
        }

        public IActionResult ShippingPayment()
        {
            ShippingPaymentViewModel model =new ShippingPaymentViewModel();
            string userId = ClaimUtility.GetUserId(User);
            model.Basket = basketService.GetBasketForUser(userId);
            model.userAddresses = userAddressServices.GetUserAddresses(userId);
            
            return View(model);
        }
        [HttpPost]
        public IActionResult ShippingPayment(int Address, PaymentMethod PaymentMethod)
        {
            string userId = ClaimUtility.GetUserId(User);
            var basket = basketService.GetBasketForUser(userId);
            int ordeId = orderService.CreateOrder(basket.Id, Address, PaymentMethod);
            if(PaymentMethod == PaymentMethod.OnlinePayment)
            {
                // ثبت پرداخت
                var payment = paymentServices.PayForOrder(ordeId);
                // ارسال به درگاه پرداخت
                return RedirectToAction("Index", "Pay", new { PaymentId = payment.PaymentId });
            }
            else
            {
                // برو به صفحه سفارش من
                return RedirectToAction("Index", "Orders", new { area = "Customer" });
            }
            
        }
        public IActionResult checkout()
        {
            return View();
        }
        private BasketDto GetOrSetBasket()
        {
            if (signInManager.IsSignedIn(User))
            {
                userId = ClaimUtility.GetUserId(User);
                return basketService.GetOrCreateBasketForUser(userId);
            }
            else
            {
                SetCookiesForBasket();
                return basketService.GetOrCreateBasketForUser(userId);
            }
        }

        private void SetCookiesForBasket()
        {
            string basketCookieName = "BasketId";
            if (Request.Cookies.ContainsKey(basketCookieName))
            {
                userId = Request.Cookies[basketCookieName];
            }
            if (userId != null) return;
            userId = Guid.NewGuid().ToString();
            var cookieOptions = new CookieOptions { IsEssential = true };
            cookieOptions.Expires = DateTime.Today.AddYears(2);
            Response.Cookies.Append(basketCookieName, userId, cookieOptions);


        }
    }
}
