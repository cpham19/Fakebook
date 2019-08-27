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

        public bool CheckUserHasStore(int PersonId)
        {
            Store store = db.Stores.Where(s => s.StoreOwnerId == PersonId).SingleOrDefault();

            if (store == null)
            {
                return false;
            }

            return true;
        }
        public bool checkStoreOwner(int StoreId, int PersonId)
        {
            Store store = db.Stores.Where(s => s.StoreId == StoreId && s.StoreOwnerId == PersonId).SingleOrDefault();

            if (store == null)
            {
                return false;
            }

            return true;
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

        public Order GetCart(int PersonId)
        {
            Order cart = db.Orders.Where(order => order.PersonId == PersonId && order.OrderStatus == 0).SingleOrDefault();
            if(cart == null)
            {
                Order neworder = new Order();
                neworder.PersonId = PersonId;
                db.Orders.Add(neworder);
                db.SaveChanges();
                cart = db.Orders.Where(o => o.PersonId == PersonId && o.OrderStatus == 0).SingleOrDefault();
            }

            List<OrderItem> orderitems = db.OrderItems.Where(item => item.OrderId == cart.OrderId).ToList();
            double total = 0.00;
            foreach(var item in orderitems)
            {
                StoreItem si = db.StoreItems.Where(s => s.StoreItemId == item.StoreItemId).SingleOrDefault();
                Store store = db.Stores.Where(st => st.StoreId == si.StoreId).SingleOrDefault();
                item.StoreId = si.StoreId;
                item.StoreName = store.StoreName;
                item.OrderItemName = si.ItemName;
                item.OrderItemImageUrl = si.ItemImageUrl;
                item.OrderItemPrice = double.Parse(Math.Round(si.Price, 2).ToString());
                total += item.OrderItemPrice * item.OrderItemQuantity;
            }

            cart.Total = Math.Truncate(total * 100) / 100; ;
            cart.OrderItems = orderitems;

            return cart;
        }

        public void AddToCart(OrderItem orderitem, int PersonId)
        {
            Order order = db.Orders.Where(o => o.PersonId == PersonId && o.OrderStatus == 0).SingleOrDefault();
            // Have to make a new order (cart) everytime someone checks out their cart or doesn't have a cart/order yet. Might not be a good way to approach
            if (order == null)
            {
                Order neworder = new Order();
                neworder.PersonId = PersonId;
                db.Orders.Add(neworder);
                db.SaveChanges();
                order = db.Orders.Where(o => o.PersonId == PersonId && o.OrderStatus == 0).SingleOrDefault();
            }

            // Check if item is in cart already
            OrderItem c = db.OrderItems.Where(ci => ci.StoreItemId == orderitem.StoreItemId && ci.OrderId == order.OrderId).SingleOrDefault();
            // Add Item to cart
            if (c == null)
            {
                orderitem.OrderId = order.OrderId;
                db.OrderItems.Add(orderitem);
            }
            // Just change the quantity (if user added 20 items, and he decides to add 10 more, then there should be 30 items in the card
            else
            {
                StoreItem storeItem = db.StoreItems.Where(si => si.StoreItemId == c.StoreItemId).SingleOrDefault();
                if (storeItem.Quantity >= c.OrderItemQuantity + orderitem.OrderItemQuantity)
                {
                    c.OrderItemQuantity += orderitem.OrderItemQuantity;
                }
            }
            db.SaveChanges();
        }

        public void DeleteOrderItem(int OrderItemId)
        {
            OrderItem ci = db.OrderItems.Where(c => c.OrderItemId == OrderItemId).SingleOrDefault();
            db.OrderItems.Remove(ci);
            db.SaveChanges();
        }

        public void Checkout(int OrderId)
        {
            Order order = db.Orders.Where(o => o.OrderId == OrderId && o.OrderStatus == 0).SingleOrDefault();
            order.OrderStatus = 1;
            order.OrderDate = DateTime.Now;

            List<OrderItem> orderitems = db.OrderItems.Where(oi => oi.OrderId == order.OrderId).ToList();
            foreach(var item in orderitems)
            {
                StoreItem storeitem = db.StoreItems.Where(st => st.StoreItemId == item.StoreItemId).SingleOrDefault();
                storeitem.Quantity -= item.OrderItemQuantity;
            }

            // Make a new cart/order for user after checking out
            int PersonId = order.PersonId;
            Order neworder = new Order();
            neworder.PersonId = PersonId;
            db.Orders.Add(neworder);
            db.SaveChanges();
        }

        public List<Order> GetOrderHistory(int PersonId)
        {
            List<Order> orders = db.Orders.Where(o => o.PersonId == PersonId && o.OrderStatus == 1).OrderBy(o => o.OrderDate).ToList();
            foreach(var order in orders)
            {
                order.OrderItems = db.OrderItems.Where(oi => oi.OrderId == order.OrderId).ToList();
                double total = 0.00;
                foreach(var oi in order.OrderItems)
                {
                    StoreItem si = db.StoreItems.Where(s => s.StoreItemId == oi.StoreItemId).SingleOrDefault();
                    Store store = db.Stores.Where(st => st.StoreId == si.StoreId).SingleOrDefault();
                    oi.StoreId = si.StoreId;
                    oi.StoreName = store.StoreName;
                    oi.OrderItemName = si.ItemName;
                    oi.OrderItemImageUrl = si.ItemImageUrl;
                    total += oi.OrderItemPrice * oi.OrderItemQuantity;
                }
                order.Total = total;
            }

            return orders;
        }
    }
}

