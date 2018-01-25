namespace WebServer.ByTheCakeApplication.Data
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using ViewModels;

    public class CakesData
    {
        private const string DatabasePath = @"ByTheCakeApplication\Data\database.csv";

        public void Add(string name, string price)
        {
            var streamReader = new StreamReader(DatabasePath);
            var id = streamReader.ReadToEnd().Split(Environment.NewLine).Length;
            streamReader.Dispose();

            using (var streamWriter = new StreamWriter(DatabasePath, true))
            {
                streamWriter.WriteLine($"{id},{name},{price}");
            }
        }

        public IEnumerable<Cake> All()
        {
            return File
                .ReadAllLines(DatabasePath)
                .Where(l => l.Contains(","))
                .Select(l => l.Split(","))
                .Select(l => new Cake(int.Parse(l[0]), l[1], decimal.Parse(l[2])));
        }

        public Cake Find(int id) => this.All().FirstOrDefault(c => c.Id == id);
    }
}
