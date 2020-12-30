using System;
using System.Collections.Generic;

namespace Emlakkko
{
    public partial class Adres
    {
        public Adres()
        {
            Ev = new HashSet<Ev>();
        }

        public int Id { get; set; }
        public int EvId { get; set; }
        public string Ilce { get; set; }
        public string Il { get; set; }
        public string Satir1 { get; set; }

        public virtual ICollection<Ev> Ev { get; set; }
    }
}
