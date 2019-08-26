using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fakebook.Models
{
    public class Store
    {
        public int StoreId { get; set; }
        public string StoreImageUrl { get; set; }
        public string StoreName { get; set; }
        public string StoreDescription { get; set; }
        public DateTime DateCreated { get; set; }
        public int StoreOwnerId { get; set; }
        [NotMapped]
        public string StoreOwnerName { get; set; }
        [NotMapped]
        public List<StoreItem> Items { get; set; } = new List<StoreItem>();
    }

    public class StoreItem
    {
        public int StoreItemId { get; set; }
        public int StoreId { get; set; }
        public string ItemImageUrl { get; set; }
        public string ItemName { get; set;}
        public string ItemCondition { get; set; }
        public string ItemDescription { get; set; }
        public DateTime DateCreated { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        [NotMapped]
        public List<Review> Reviews { get; set; } = new List<Review>();
    }

    public class Review
    {
        public int ReviewId { get; set; }
        public int StoreItemId { get; set; }
        public int PosterId { get; set; }
        public string Description { get; set; }
        public DateTime DatePosted { get; set; }
        [NotMapped]
        public string PosterName { get; set; }
    }
}
