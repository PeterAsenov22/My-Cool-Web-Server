namespace WebServer.ByTheCakeApplication.ViewModels.Shopping
{
    using System;

    public class OrderFromDbViewModel
    {
        public int Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public decimal Sum { get; set; }
    }
}
