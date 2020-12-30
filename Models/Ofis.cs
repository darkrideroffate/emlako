using System;
using System.Collections.Generic;

namespace Emlakkko
{
    public partial class Ofis
    {
        public Ofis()
        {
            IlanKoy = new HashSet<IlanKoy>();
            Personel = new HashSet<Personel>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<IlanKoy> IlanKoy { get; set; }
        public virtual ICollection<Personel> Personel { get; set; }
    }
}
