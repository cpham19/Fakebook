using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Fakebook.Models;
using Fakebook.Services;
using Microsoft.AspNetCore.Mvc;

namespace Fakebook.Controllers
{
    public class MarketplaceController : Controller
    {
        private readonly StoreService storeService;
        private readonly UserService userService;

        public MarketplaceController(StoreService storeService, UserService userService)
        {
            this.storeService = storeService;
            this.userService = userService;
        }

        // Get all stores
        [HttpGet("/Marketplace", Name = "MarketplaceIndex")]
        public IActionResult Index()
        {
            ViewBag.Me = userService.GetPersonBasedOnId(User.Identity.GetPersonId());
            ViewBag.HasStore = storeService.CheckUserHasStore(User.Identity.GetPersonId());
            ViewBag.Cart = storeService.GetCart(User.Identity.GetPersonId());
            ViewBag.Stores = storeService.GetStores();
            return View();
        }

        // Get store
        [HttpGet("/Marketplace/store/{StoreId}", Name = "StoreIndex")]
        public IActionResult Store(int StoreId)
        {
            ViewBag.Me = userService.GetPersonBasedOnId(User.Identity.GetPersonId());
            ViewBag.Cart = storeService.GetCart(User.Identity.GetPersonId());
            ViewBag.PersonId = User.Identity.GetPersonId();
            ViewBag.Store = storeService.GetStore(StoreId);
            return View();
        }

        // Add store
        [HttpGet("/Marketplace/AddStore", Name = "AddStore")]
        public IActionResult AddStore()
        {
            ViewBag.Me = userService.GetPersonBasedOnId(User.Identity.GetPersonId());
            return View();
        }

        // Add store
        [HttpPost("/Marketplace/AddStore", Name = "SubmitAddStore")]
        public IActionResult AddStore(Store store)
        {
            ViewBag.Me = userService.GetPersonBasedOnId(User.Identity.GetPersonId());
            if (ModelState.IsValid)
            {
                store.StoreOwnerId = User.Identity.GetPersonId();
                store.DateCreated = DateTime.Now;
                storeService.AddStore(store);
                return RedirectToAction(nameof(Index));
            }

            return View(store);
        }

        // Edit store
        [HttpGet("/Marketplace/store/{StoreId}/EditStore", Name = "EditStore")]
        public IActionResult EditStore(int StoreId)
        {
            ViewBag.Me = userService.GetPersonBasedOnId(User.Identity.GetPersonId());
            Store store = storeService.GetStore(StoreId);
            return View(store);
        }

        // Edit store
        [HttpPost("/Marketplace/store/{StoreId}/EditStore", Name = "SubmitEditStore")]
        public IActionResult EditStore(int StoreId, Store store)
        {
            ViewBag.Me = userService.GetPersonBasedOnId(User.Identity.GetPersonId());
            if (ModelState.IsValid)
            {
                storeService.EditStore(store);
                return RedirectToAction("Store", new { StoreId = StoreId });
            }
            return View(store);
        }

        // Delete store
        [HttpGet("/Marketplace/store/{StoreId}/DeleteStore", Name = "DeleteStore")]
        public IActionResult DeleteStore(int StoreId)
        {
            storeService.DeleteStore(StoreId);
            return RedirectToAction(nameof(Index));
        }

        // View Store Item
        [HttpGet("/Marketplace/store/{StoreId}/storeitem/{StoreItemId}", Name = "ViewStoreItem")]
        public IActionResult StoreItem(int StoreId, int StoreItemId)
        {
            ViewBag.Me = userService.GetPersonBasedOnId(User.Identity.GetPersonId());
            ViewBag.Cart = storeService.GetCart(User.Identity.GetPersonId());
            ViewBag.IsOwner = storeService.checkStoreOwner(StoreId, User.Identity.GetPersonId());
            ViewBag.PersonId = User.Identity.GetPersonId();
            StoreItem storeItem = storeService.GetStoreItem(StoreItemId);
            return View(storeItem);
        }

        // Add store item
        [HttpGet("/Marketplace/store/{StoreId}/AddItem", Name = "AddStoreItem")]
        public IActionResult AddStoreItem(int StoreId)
        {
            ViewBag.Me = userService.GetPersonBasedOnId(User.Identity.GetPersonId());
            ViewBag.StoreId = StoreId;
            return View();
        }

        // Add store item
        [HttpPost("/Marketplace/store/{StoreId}/AddItem", Name = "SubmitAddStoreItem")]
        public IActionResult AddStoreItem(int StoreId, StoreItem storeItem)
        {
            ViewBag.Me = userService.GetPersonBasedOnId(User.Identity.GetPersonId());
            if (ModelState.IsValid)
            {
                storeItem.StoreId = StoreId;
                storeItem.DateCreated = DateTime.Now;
                storeService.AddStoreItem(storeItem);
                return RedirectToAction("Store", new { StoreId = StoreId });
            }
            return View(storeItem);
        }

        // Edit store item
        [HttpGet("/Marketplace/store/{StoreId}/storeitem/{StoreItemId}/EditItem", Name = "EditStoreItem")]
        public IActionResult EditStoreItem(int StoreId, int StoreItemId)
        {
            ViewBag.Me = userService.GetPersonBasedOnId(User.Identity.GetPersonId());
            StoreItem si = storeService.GetStoreItem(StoreItemId);
            return View(si);
        }
        
        // Edit store item
        [HttpPost("/Marketplace/store/{StoreId}/storeitem/{StoreItemId}/EditItem", Name = "SubmitEditStoreItem")]
        public IActionResult EditStoreItem(int StoreId, int StoreItemId, StoreItem si)
        {
            ViewBag.Me = userService.GetPersonBasedOnId(User.Identity.GetPersonId());
            if (ModelState.IsValid)
            {
                si.StoreId = StoreId;
                si.StoreItemId = StoreItemId;
                storeService.EditStoreItem(si);
                return RedirectToAction("StoreItem", new { StoreId = StoreId, StoreItemId = StoreItemId });
            }

            return View(si);
        }

        public IActionResult DeleteStoreItem(int StoreId, int StoreItemId)
        {
            storeService.DeleteStoreItem(StoreItemId);
            return RedirectToAction("Store", new { StoreId = StoreId});
        }

        public IActionResult AddReview(int StoreId, int StoreItemId, Review r)
        {
            if (ModelState.IsValid)
            {
                r.StoreItemId = StoreItemId;
                r.PosterId = User.Identity.GetPersonId();
                r.DatePosted = DateTime.Now;
                storeService.AddReview(r);
                return RedirectToAction("StoreItem", new { StoreId = StoreId, StoreItemId = StoreItemId });
            }

            return RedirectToAction("StoreItem", new { StoreId = StoreId, StoreItemId = StoreItemId });
        }

        // Edit Review
        [HttpGet("/Marketplace/store/{StoreId}/storeitem/{StoreItemId}/review/{ReviewId}/EditReview", Name = "EditReview")]
        public IActionResult EditReview(int StoreId, int StoreItemId, int ReviewId)
        {
            ViewBag.Me = userService.GetPersonBasedOnId(User.Identity.GetPersonId());
            ViewBag.StoreId = StoreId;
            Review review = storeService.GetReview(ReviewId);
            return View(review);
        }

        // Edit Review
        [HttpPost("/Marketplace/store/{StoreId}/storeitem/{StoreItemId}/review/{ReviewId}/EditReview", Name = "SubmitEditReview")]
        public IActionResult EditReview(int StoreId, int StoreItemId, int ReviewId, Review r)
        {
            ViewBag.Me = userService.GetPersonBasedOnId(User.Identity.GetPersonId());
            if (ModelState.IsValid)
            {
                storeService.EditReview(r);
                return RedirectToAction("StoreItem", new { StoreId = StoreId, StoreItemId = StoreItemId });
            }

            return View(r);
        }

        public IActionResult DeleteReview(int StoreId, int StoreItemId, int ReviewId)
        {
            storeService.DeleteReview(ReviewId);
            return RedirectToAction("StoreItem", new { StoreId = StoreId, StoreItemId = StoreItemId });
        }

        // View Cart
        [HttpGet("/Marketplace/cart", Name = "Cart")]
        public IActionResult Cart()
        {
            ViewBag.Me = userService.GetPersonBasedOnId(User.Identity.GetPersonId());
            List<Order> cart = storeService.GetCart(User.Identity.GetPersonId());
            return View(cart);
        }

        // View Order History
        [HttpGet("/Marketplace/orderhistory", Name = "OrderHistory")]
        public IActionResult OrderHistory()
        {
            ViewBag.Me = userService.GetPersonBasedOnId(User.Identity.GetPersonId());
            List<Order> orderhistory = storeService.GetOrderHistory(User.Identity.GetPersonId());
            return View(orderhistory);
        }

        // View Order History
        [HttpGet("/Marketplace/saleshistory", Name = "SalesHistory")]
        public IActionResult SalesHistory()
        {
            ViewBag.Me = userService.GetPersonBasedOnId(User.Identity.GetPersonId());
            List<Order> saleshistory = storeService.GetSalesHistory(User.Identity.GetPersonId());
            return View(saleshistory);
        }

        public IActionResult DeleteOrderItem(int OrderItemId)
        {
            storeService.DeleteOrderItem(OrderItemId);
            return RedirectToAction("Cart");
        }

        public IActionResult AddToCart(int StoreId, int StoreItemId, int Quantity, double OrderItemPrice)
        {
            OrderItem oi = new OrderItem();
            oi.StoreItemId = StoreItemId;
            oi.OrderItemQuantity = Quantity;
            oi.OrderItemPrice = OrderItemPrice;
            storeService.AddToCart(oi, StoreId, User.Identity.GetPersonId());
            return RedirectToAction("StoreItem", new { StoreId = StoreId, StoreItemId = StoreItemId });
        }

        public IActionResult Checkout()
        {
            storeService.Checkout(User.Identity.GetPersonId());
            return RedirectToAction("Cart");
        }
    }
}