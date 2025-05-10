using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamuraiApp_Console
{
    public class Dojo
    {
        public Dojo()
        {
        }

        public Dojo(string name, string region)
        {
            Name = name;
            Region = region;
        }

        public string Name { get; set; }
        public string Region { get; set; }
        private List<Samurai> Samurais = new();

        public override string ToString()
        {
            return $@"Dojo {Name} in the {Region} region.";
        }

        public void AddSamurai(Samurai s)
        {
            Samurais.Add(s);
        }

        public void ListSamurai()
        {
            Console.WriteLine($"Students from Dojo {Name}.");
            foreach (Samurai s in Samurais)
            {
                Console.WriteLine($"Student: {s}");
            }
        }
    }
}
