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
    }
}

