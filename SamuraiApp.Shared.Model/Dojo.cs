using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamuraiApp_Model
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
        public int Id {  get; set; }
        private ICollection<Samurai> SamCollect { get; set; } =
            new List<Samurai>();

        public override string ToString()
        {
            return $@"Dojo {Name} in the {Region} region. (ID: {Id})";
        }

        public void AddSamurai(Samurai s)
        {
            SamCollect.Add(s);
        }

        public void ListSamurai()
        {
            Console.WriteLine($"Students from Dojo {Name}.");
            if (SamCollect.Count > 0)
            {
                foreach (Samurai s in SamCollect)
                {
                    Console.WriteLine($"Student: {s}");
                }
            }
            else
            {
                Console.WriteLine("No students in this Dojo.");
            }
        }
    }
}
