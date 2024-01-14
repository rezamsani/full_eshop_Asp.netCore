using Application.BasketsService;
using Application.User;

namespace WebSite.EndPoint.Models.ViewModels.Basket
{
    public class ShippingPaymentViewModel
    {
        public BasketDto Basket { get; set; }
        public List<UserAddressDto>  userAddresses { get; set; }
    }
}
