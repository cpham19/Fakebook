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
            return db.StoreItems.Where(st => st.StoreItemId == StoreItemId).SingleOrDefault();
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
            foreach(var store in stores)
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
            foreach(var storeitem in store.Items)
            {
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
            db.StoreItems.Remove(st);
            db.SaveChanges();
        }
    }
}

