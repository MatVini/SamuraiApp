using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamuraiApp_Model
{
    public class Samurai
    {
        public Samurai()
        {
        }

        public Samurai(string name, string clan)
        {
            Name = name;
            Clan = clan;
        }

        public string Name { get; set; }
        public string Clan { get; set; }
        public virtual Dojo? Dojo { get; set; }
        public int Id {  get; set; }

        public override string ToString()
        {
            return $@"Samurai {Name} from Clan {Clan}. (ID: {Id})";
        }
    }
}
