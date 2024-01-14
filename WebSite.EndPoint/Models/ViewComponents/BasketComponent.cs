using Application.BasketsService;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebSite.EndPoint.Utilities;

namespace WebSite.EndPoint.Models.ViewComponents
{
    public class BasketComponent : ViewComponent
    {
        private readonly IBasketsService basketService;
        public BasketComponent(IBasketsService basketService)
        {
            this.basketService = basketService;
        }
        private ClaimsPrincipal userClaimsPrincipal => ViewContext?.HttpContext?.User;
        public IViewComponentResult Invoke()
        {
            BasketDto basket = null;
            if (User.Identity.IsAuthenticated)
            {
                basket = basketService.GetBasketForUser(ClaimUtility.GetUserId(userClaimsPrincipal));
            }
            else
            {
                string basketCookieName = "BasketId";
                if (Request.Cookies.ContainsKey(basketCookieName))
                {
                    var buyerId = Request.Cookies[basketCookieName];
                    basket = basketService.GetBasketForUser(buyerId);
                }
            }
            return View(viewName: "BasketComponent", model: basket);
        }
    }
}
