using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Fakebook.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Diagnostics;

// Used for wall posts and their replies
namespace Fakebook.Services
{
    public class StoreService
    {
        private readonly AppDbContext db;

        public StoreService(AppDbContext db)
        {
            this.db = db;
        }

        public StoreItem GetStoreItem(int StoreItemId)
        {
            StoreItem storeItem = db.StoreItems.Where(st => st.StoreItemId == StoreItemId).SingleOrDefault();
            storeItem.Reviews = db.Reviews.Where(review => review.StoreItemId == storeItem.StoreItemId).ToList();
            foreach (var review in storeItem.Reviews)
            {
                review.PosterName = db.Persons.Where(p => p.PersonId == review.PosterId).SingleOrDefault().Name;
            }

            // Get Store Owner Id
            Store store = db.Stores.Where(s => s.StoreId == storeItem.StoreId).SingleOrDefault();
            storeItem.PosterId = store.StoreOwnerId;

            return storeItem;
        }

        public Store GetStore(int StoreId)
        {
            Store store = db.Stores.Where(s => s.StoreId == StoreId).SingleOrDefault();
            Person person = db.Persons.Where(p => p.PersonId == store.StoreOwnerId).SingleOrDefault();
            store.StoreOwnerName = person.Name;
            store.Items = db.StoreItems.Where(storeitem => storeitem.StoreId == StoreId).ToList();
            return store;
        }

        public List<Store> GetStores()
        {
            List<Store> stores = db.Stores.OrderBy(store => store.StoreId).ToList();
            foreach (var store in stores)
            {
                Person person = db.Persons.Where(p => p.PersonId == store.StoreOwnerId).SingleOrDefault();
                store.StoreOwnerName = person.Name;
                store.Items = db.StoreItems.Where(storeitem => storeitem.StoreId == store.StoreId).ToList();
            }

            return stores;
        }

        public List<Store> GetStoresOfUser(int PersonId)
        {
            List<Store> stores = db.Stores.Where(store => store.StoreOwnerId == PersonId).ToList();
            return stores;
        }

        public void AddStore(Store s)
        {
            db.Stores.Add(s);
            db.SaveChanges();
        }

        public void EditStore(Store s)
        {
            Store store = this.GetStore(s.StoreId);
            store.StoreName = s.StoreName;
            store.StoreDescription = s.StoreDescription;
            store.StoreImageUrl = s.StoreImageUrl;
            db.SaveChanges();
        }

        public void DeleteStore(int StoreId)
        {
            Store store = this.GetStore(StoreId);
            foreach (var storeitem in store.Items)
            {
                List<Review> reviews = db.Reviews.Where(review => review.StoreItemId == storeitem.StoreItemId).ToList();
                foreach (var review in reviews)
                {
                    db.Reviews.Remove(review);
                }
                db.StoreItems.Remove(storeitem);
            }

            db.Stores.Remove(store);
            db.SaveChanges();
        }

        public void AddStoreItem(StoreItem st)
        {
            db.StoreItems.Add(st);
            db.SaveChanges();
        }

        public void EditStoreItem(StoreItem st)
        {
            StoreItem storeItem = this.GetStoreItem(st.StoreItemId);
            storeItem.ItemName = st.ItemName;
            storeItem.ItemDescription = st.ItemDescription;
            storeItem.ItemCondition = st.ItemCondition;
            storeItem.ItemImageUrl = st.ItemImageUrl;
            storeItem.Quantity = st.Quantity;
            storeItem.Price = st.Price;
            db.SaveChanges();
        }

        public void DeleteStoreItem(int StoreItemId)
        {
            StoreItem st = this.GetStoreItem(StoreItemId);
            List<Review> reviews = db.Reviews.Where(review => review.StoreItemId == StoreItemId).ToList();
            foreach (var review in reviews)
            {
                db.Reviews.Remove(review);
            }
            db.StoreItems.Remove(st);
            db.SaveChanges();
        }

        public Review GetReview(int ReviewId)
        {
            return db.Reviews.Where(r => r.ReviewId == ReviewId).SingleOrDefault();
        }

        public List<StoreItem> GetReviewsOfUser(int PersonId)
        {
            List<Review> reviews = db.Reviews.Where(review => review.PosterId == PersonId).ToList();
            List<StoreItem> itemsReviewed = new List<StoreItem>();
            foreach (var review in reviews)
            {
                StoreItem si = db.StoreItems.Where(item => item.StoreItemId == review.StoreItemId).SingleOrDefault();
                if (!itemsReviewed.Contains(si))
                {
                    itemsReviewed.Add(si);
                }
            }

            foreach (var itemReview in itemsReviewed)
            {
                itemReview.Reviews = db.Reviews.Where(review => review.PosterId == PersonId).ToList();
            }

            return itemsReviewed;
        }

        public void AddReview(Review review)
        {
            db.Reviews.Add(review);
            db.SaveChanges();
        }

        public void DeleteReview(int ReviewId)
        {
            Review review = db.Reviews.Where(r => r.ReviewId == ReviewId).SingleOrDefault();
            db.Reviews.Remove(review);
            db.SaveChanges();
        }

        public void EditReview(Review r)
        {
            Review review = db.Reviews.Where(rev => rev.ReviewId == r.ReviewId).SingleOrDefault();
            review.Description = r.Description;
            db.SaveChanges();
        }

        public Cart GetCart(int PersonId)
        {
            Cart cart = new Cart();
            List<CartItem> cartitems = db.CartItems.Where(ci => ci.PersonId == PersonId).ToList();
            double total = 0.00;
            foreach(var item in cartitems)
            {
                StoreItem si = db.StoreItems.Where(s => s.StoreItemId == item.StoreItemId).SingleOrDefault();
                item.StoreId = si.StoreId;
                item.CartItemName = si.ItemName;
                item.CartItemPrice = float.Parse(Math.Round(si.Price, 2).ToString());
                total += item.CartItemPrice * item.CartItemQuantity;
            }

            cart.Total = Math.Truncate(total * 100) / 100; ;
            cart.Items = cartitems;

            return cart;
        }

        public void AddToCart(CartItem cartitem)
        {
            // Check if item is in cart already
            CartItem c = db.CartItems.Where(ci => ci.StoreItemId == cartitem.StoreItemId && ci.PersonId == cartitem.PersonId).SingleOrDefault();

            // Add Item to cart
            if (c == null)
            {
                db.CartItems.Add(cartitem);
            }
            // Just change the quantity (if user added 20 items, and he decides to add 10 more, then there should be 30 items in the card
            else
            {
                c.CartItemQuantity += cartitem.CartItemQuantity;
            }
            db.SaveChanges();
        }

        public void DeleteCartItem(int CartItemId)
        {
            CartItem ci = db.CartItems.Where(c => c.CartItemId == CartItemId).SingleOrDefault();
            db.CartItems.Remove(ci);
            db.SaveChanges();
        }
    }
}

