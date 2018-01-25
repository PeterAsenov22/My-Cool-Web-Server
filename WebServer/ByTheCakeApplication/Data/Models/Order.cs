namespace WebServer.ByTheCakeApplication.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Collections.Generic;

    public class Order
    {
        public Order()
        {
            this.Products = new List<OrderProduct>();
        }

        public int Id { get; set; }

        public DateTime CreationTime { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        public User User { get; set; }

        public List<OrderProduct> Products { get; set; }
    }
}
