namespace WebServer.ByTheCakeApplication.Models
{
    public class Cake
    {
        public Cake(string name, decimal price)
        {
            this.Name = name;
            this.Price = price;
        }

        public Cake(int id, string name, decimal price) 
            : this(name,price)
        {
            this.Id = id;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public override string ToString()
        {
            return $"{this.Name} - ${this.Price:f2}";
        }
    }
}
