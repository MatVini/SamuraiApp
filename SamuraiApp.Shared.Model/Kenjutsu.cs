using SamuraiApp_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamuraiApp.Shared.Model
{
    public class Kenjutsu
    {
        public Kenjutsu()
        {
        }

        public Kenjutsu(string style)
        {
            Style = style;
        }

        public string Style { get; set; }
        private ICollection<Samurai> Samurais { get; set; } =
            new List<Samurai>();
        public int Id { get; set; }
    }
}
