using System;
using System.Collections.Generic;

namespace Pustok.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }
}
