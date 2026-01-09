
using System;
using System.Collections.Generic;

namespace Labb_två_Bokhandel.Models
{
    public partial class Book
    {
        public Book()
        {
            Authors = new List<Author>();
            Inventories = new List<Inventory>();
            OrderDetails = new List<OrderDetail>();
        }

        // Primärnyckel
        public string Isbn13 { get; set; } = null!;

        // Bokinformation
        public string Title { get; set; } = null!;
        public string Language { get; set; } = null!;
        public decimal Price { get; set; }
        public DateOnly PublishDate { get; set; }

        // Publisher (valfri)
        public int? PublisherId { get; set; }
        public virtual Publisher? Publisher { get; set; }

        // Navigation properties
        public virtual ICollection<Author> Authors { get; set; }
        public virtual ICollection<Inventory> Inventories { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
