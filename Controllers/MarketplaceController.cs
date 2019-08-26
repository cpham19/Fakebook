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

        public MarketplaceController(StoreService storeService)
        {
            this.storeService = storeService;
        }

        // Get all stores
        [HttpGet("/Marketplace", Name = "MarketplaceIndex")]
        public IActionResult Index()
        {
            ViewBag.Stores = storeService.GetStores();
            return View();
        }

        // Get store
        [HttpGet("/Marketplace/store/{StoreId}", Name = "StoreIndex")]
        public IActionResult Store(int StoreId)
        {
            ViewBag.PersonId = User.Identity.GetPersonId();
            ViewBag.Store = storeService.GetStore(StoreId);
            return View();
        }

        // Add store
        [HttpGet("/Marketplace/AddStore", Name = "AddStore")]
        public IActionResult AddStore()
        {
            return View();
        }

        // Add store
        [HttpPost("/Marketplace/AddStore", Name = "SubmitAddStore")]
        public IActionResult AddStore(Store store)
        {
            store.StoreOwnerId = User.Identity.GetPersonId();
            store.DateCreated = DateTime.Now;
            storeService.AddStore(store);
            return RedirectToAction(nameof(Index));
        }

        // Edit store
        [HttpGet("/Marketplace/store/{StoreId}/EditStore", Name = "EditStore")]
        public IActionResult EditStore(int StoreId)
        {
            Store store = storeService.GetStore(StoreId);
            return View(store);
        }

        // Edit store
        [HttpPost("/Marketplace/store/{StoreId}/EditStore", Name = "SubmitEditStore")]
        public IActionResult EditStore(int StoreId, Store store)
        {
            storeService.EditStore(store);
            return RedirectToAction("Store", new { StoreId = StoreId });
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
            ViewBag.PersonId = User.Identity.GetPersonId();
            StoreItem storeItem = storeService.GetStoreItem(StoreItemId);
            return View(storeItem);
        }

        // Add store item
        [HttpGet("/Marketplace/store/{StoreId}/AddItem", Name = "AddStoreItem")]
        public IActionResult AddStoreItem(int StoreId)
        {
            ViewBag.StoreId = StoreId;
            return View();
        }

        // Add store item
        [HttpPost("/Marketplace/store/{StoreId}/AddItem", Name = "SubmitAddStoreItem")]
        public IActionResult AddStoreItem(int StoreId, StoreItem storeItem)
        {
            storeItem.StoreId = StoreId;
            storeItem.DateCreated = DateTime.Now;
            storeService.AddStoreItem(storeItem);
            return RedirectToAction("Store", new { StoreId = StoreId });
        }

        // Edit store item
        [HttpGet("/Marketplace/store/{StoreId}/storeitem/{StoreItemId}/EditItem", Name = "EditStoreItem")]
        public IActionResult EditStoreItem(int StoreId, int StoreItemId)
        {
            StoreItem si = storeService.GetStoreItem(StoreItemId);
            return View(si);
        }
        
        // Edit store item
        [HttpPost("/Marketplace/store/{StoreId}/storeitem/{StoreItemId}/EditItem", Name = "SubmitEditStoreItem")]
        public IActionResult EditStoreItem(int StoreId, int StoreItemId, StoreItem si)
        {
            si.StoreId = StoreId;
            si.StoreItemId = StoreItemId;
            storeService.EditStoreItem(si);
            return RedirectToAction("StoreItem", new { StoreId = StoreId, StoreItemId = StoreItemId});
        }

        public IActionResult DeleteStoreItem(int StoreId, int StoreItemId)
        {
            storeService.DeleteStoreItem(StoreItemId);
            return RedirectToAction("Store", new { StoreId = StoreId});
        }

        public IActionResult AddReview(int StoreId, int StoreItemId, Review r)
        {
            r.StoreItemId = StoreItemId;
            r.PosterId = User.Identity.GetPersonId();
            r.DatePosted = DateTime.Now;
            storeService.AddReview(r);
            return RedirectToAction("StoreItem", new { StoreId = StoreId, StoreItemId = StoreItemId });
        }

        // Edit Review
        [HttpGet("/Marketplace/store/{StoreId}/storeitem/{StoreItemId}/review/{ReviewId}/EditReview", Name = "EditReview")]
        public IActionResult EditReview(int StoreId, int StoreItemId, int ReviewId)
        {
            Review review = storeService.GetReview(ReviewId);
            return View(review);
        }

        // Edit Review
        [HttpPost("/Marketplace/store/{StoreId}/storeitem/{StoreItemId}/review/{ReviewId}/EditReview", Name = "SubmitEditReview")]
        public IActionResult EditReview(int StoreId, int StoreItemId, int ReviewId, Review r)
        {
            storeService.EditReview(r);
            return RedirectToAction("StoreItem", new { StoreId = StoreId, StoreItemId = StoreItemId });
        }

        public IActionResult DeleteReview(int StoreId, int StoreItemId, int ReviewId)
        {
            storeService.DeleteReview(ReviewId);
            return RedirectToAction("StoreItem", new { StoreId = StoreId, StoreItemId = StoreItemId });
        }

    }
}