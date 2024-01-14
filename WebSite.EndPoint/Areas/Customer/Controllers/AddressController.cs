using Application.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver.Core.Operations;
using WebSite.EndPoint.Utilities;

namespace WebSite.EndPoint.Areas.Customer.Controllers
{
    [Authorize]
    [Area("Customer")]
    public class AddressController : Controller
    {
        private readonly IUserAddressServices userAddressServices;

        public AddressController(IUserAddressServices userAddressServices)
        {
            this.userAddressServices = userAddressServices;
        }
        // GET: AddressController
        public ActionResult Index()
        {
            var address = userAddressServices.GetUserAddresses(ClaimUtility.GetUserId(User));
            return View(address);
        }

        // GET: AddressController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AddressController/Create
        public ActionResult Create()
        {           
            return View();
        }

        // POST: AddressController/Create
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(UserAddressAddDto address)
        {
            string userId = ClaimUtility.GetUserId(User);
            address.UserId = userId;
            userAddressServices.AddNewAddress(address);
            return RedirectToAction(nameof(Index));
        }

        // GET: AddressController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AddressController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AddressController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AddressController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
