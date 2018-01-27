namespace WebServer.ByTheCakeApplication.ViewModels
{
    using System.Collections.Generic;

    public class ShoppingCart
    {
        public const string SessionKey = "%^Current_Shopping_Cart^%";

        public Dictionary<int,int> Products { get; private set; } = new Dictionary<int, int>();

        public void Clear() => this.Products.Clear();
    }
}
